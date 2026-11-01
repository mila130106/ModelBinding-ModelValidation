using System;
using System.ComponentModel.DataAnnotations;

namespace ProductsValidation.Models
{
    public class ProductDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var description = value as string;

            // не обов'язкове поле
            if (string.IsNullOrEmpty(description))
                return ValidationResult.Success;

            var members = new[] { context.MemberName };

            if (description.Length <= 2)
                return new ValidationResult("Description must be longer than 2 symbols.", members);

            var name = (context.ObjectInstance as Product)?.Name;
            if (string.IsNullOrEmpty(name))
                return ValidationResult.Success; // помилку покаже Required на Name

            if (string.Equals(description, name, StringComparison.Ordinal))
                return new ValidationResult("Description must not be equal to Name.", members);

            if (!description.StartsWith(name, StringComparison.Ordinal))
                return new ValidationResult("Description must start with the Name of the product.", members);

            return ValidationResult.Success;
        }
    }
}