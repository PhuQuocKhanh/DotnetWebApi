using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModelBinding.CustomModelValidation;

namespace ModelBinding.Models
{
    // Represents a validation error response.
    public class ValidationErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<FieldError> Errors { get; set; } = new List<FieldError>();
    }
    // Represents an error for a specific field.
    public class FieldError
    {
        public string Field { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
    public class DataAnnotation
    {
        [Required(ErrorMessage = "User ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "DateofBirth is required.")]
        [DataType(DataType.DateTime)]
        [AgeValidation(18, 99)]
        public DateTime DateofBirth { get; set; }

        [RegularExpression(@"^\d{8}(-\d{4})?$", ErrorMessage = "Invalid Zip Code.")]
        public string ZipCode { get; set; }

        [Url(ErrorMessage = "Invalid Website URL.")]
        public string Website { get; set; }

        [CreditCard(ErrorMessage = "Invalid Credit Card Number.")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}