﻿using System.ComponentModel.DataAnnotations;

namespace LoggingAndMonitoringAPIExample.Logic.Models
{
    public class CustomerForCreationDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }
}
