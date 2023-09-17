﻿using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class RegisterRequiredDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
