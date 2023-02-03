using System.ComponentModel.DataAnnotations;
using TwistFood.Domain.Common;

namespace TwistFood.Domain.Entities;
public abstract class Employee : Auditable
{
    [Required]
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string? ImagePath { get; set; }
    public double Salary { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; } = String.Empty;
    public string PassportSeriaNumber { get; set; } = String.Empty;
}
