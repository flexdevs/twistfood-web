using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwistFood.Service.Attributes;

namespace TwistFood.Service.Dtos.Accounts
{
    public class AdminUpdateDto
    {
        [MaxLength(40),MinLength(2)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(40), MinLength(2)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(40), MinLength(2), Email]
        public string Email { get; set; } = string.Empty; 
        [MaxFileSize(2), AllowedFiles(new string[] {".jpg", ".png", ".jpeg", ".svg", ".webp" })]
        public IFormFile? Image { get; set; }
    }
}
