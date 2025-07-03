using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAppJobs4UK.Models
{


    public partial class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Department is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a department.")]
        public int Department { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [NoFutureDate(ErrorMessage = "Hire Date cannot be in the future.")]
        public DateTime HireDate { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Salary must be positive.")]
        public decimal Salary { get; set; }

    }
    public class NoFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Today)
                {
                    return new ValidationResult(ErrorMessage ?? "Date cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

