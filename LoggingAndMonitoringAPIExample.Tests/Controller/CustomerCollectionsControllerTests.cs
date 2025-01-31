﻿using AutoMapper;
using FluentAssertions;
using LoggingAndMonitoringAPIExample.Controllers;
using LoggingAndMonitoringAPIExample.Handler;
using LoggingAndMonitoringAPIExample.Logic;
using LoggingAndMonitoringAPIExample.Logic.Entities;
using LoggingAndMonitoringAPIExample.Logic.Models;
using LoggingAndMonitoringAPIExample.Logic.Parameters;
using LoggingAndMonitoringAPIExample.Logic.Services;
using LoggingAndMonitoringAPIExample.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace LoggingAndMonitoringAPIExample.Tests.Controller
{
    public class CustomerCollectionsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly CustomerCollectionsController _customerCollectionController;
        
        public CustomerCollectionsControllerTests()
        {
            var mockDependencyHandler = new Mock<CustomerDependencyHandler>();
            var mokDistributedCache = new Mock<IDistributedCache>();
            
            mockDependencyHandler = ServiceMocks.SetUpMemoryCache(mockDependencyHandler);
            
            // Create a mock object that implements the ICustomerService interface
            var mockCustomerService = ServiceMocks.SetupCustomerService();

            // Set up the mock object to return the desired value when the GetAllCustomersAsync method is called

            mockDependencyHandler
                .Setup(x => x.GetCustomerService())
                .Returns(mockCustomerService.Object);

            _mapper = ServiceMocks.SetupMapper();

            mockDependencyHandler
                .Setup(x => x.GetMapper())
                .Returns(_mapper);

            Mock<ILoggerFactory> mockLoggerFactory = ServiceMocks.SetupMockLoggerFactory();

            mockDependencyHandler
                .Setup(x => x.GetLoggerFactory())
                .Returns(mockLoggerFactory.Object);

            _customerCollectionController = new CustomerCollectionsController(mockDependencyHandler.Object, mokDistributedCache.Object);
        }
        


        [Fact]
        public async Task CreateCustomerCollection_WithNoSpecificSettings_ReturnsCreatedCollection()
        {
            var entities = _mapper.Map<List<CustomerForCreationDto>>(await CustomerMocks.GetTestCustomersAsync());
            var result = await _customerCollectionController.CreateCustomerCollection(entities);
            result.Result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAtRouteResult = result.Result as CreatedAtRouteResult;

            createdAtRouteResult?.StatusCode.Should().Be(201);
            createdAtRouteResult?.Value.Should().BeEquivalentTo(await CustomerMocks.GetTestCustomersAsync());
        }

        [Fact]
        public async Task GetCustomers_WithSpecifiedIds_ReturnCustomersWithId()
        {
            int[] customerIds = new int[] { 1, 2, 4 };

            var result = await _customerCollectionController.GetCustomers(customerIds);
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);

            var expected = CustomerMocks.GetTestCustomersAsync().Result.Where(x => x.Id == 1 || x.Id == 2 || x.Id == 4).ToList();
            okResult?.Value.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task GetCustomers_WithNoFilters_ReturnAllCustomers()
        {
            var result = await _customerCollectionController.GetCustomers(new CustomerResourceParameters());

            //result should be 200

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult?.StatusCode.Should().Be(200);
            okResult?.Value.Should().BeEquivalentTo(await CustomerMocks.GetTestCustomersAsync());
        }
    }
}

