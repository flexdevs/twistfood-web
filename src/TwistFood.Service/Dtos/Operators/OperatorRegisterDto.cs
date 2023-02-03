﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using TwistFood.Domain.Entities.Employees;
using TwistFood.Service.Attributes;
using TwistFood.Service.Common.Attributes;

namespace TwistFood.Service.Dtos.Operators
{
    public class OperatorRegisterDto
    {
        [Required, MaxLength(60), MinLength(2)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(60), MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [Required, PhoneNumber]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }
        [Required, MaxFileSize(2), AllowedFiles(new string[] { ".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public IFormFile? Image { get; set; }
        [Required, Integer]
        public double Salary { get; set; }
        [Required, PassportSeria]
        public string PassportSeriaNumber { get; set; } = string.Empty;

        [Required, Email]
        public string Email { get; set; } = string.Empty;

        [Required, StrongPassword]
        public string Password { get; set; } = string.Empty;

        public static implicit operator Operator(OperatorRegisterDto dto)
        {
            return new Operator()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate.ToUniversalTime(),
                Salary = dto.Salary,
                PassportSeriaNumber = dto.PassportSeriaNumber,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
