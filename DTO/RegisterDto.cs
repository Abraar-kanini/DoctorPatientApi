﻿using System.ComponentModel.DataAnnotations;

namespace DoctorPatient.DTO
{
    public class RegisterDto
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

       


        public string[] Roles { get; set; }
    }
}
