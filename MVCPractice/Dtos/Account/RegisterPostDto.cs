﻿using System.ComponentModel.DataAnnotations;

namespace MVCPractice.Dtos.Account
{
    public class RegisterPostDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}