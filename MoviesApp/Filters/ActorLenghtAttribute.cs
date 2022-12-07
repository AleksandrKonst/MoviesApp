using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Filters;

public class ActorLenghtAttribute : ValidationAttribute
{
    public ActorLenghtAttribute(int lenght)
    {
    }
    
    public int lenght { get; }
    
    public string GetErrorMessage() =>
        $"Lenght must be more {lenght} .";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is Int32)
        {
            var lenght = (int) value;

            if (lenght < this.lenght)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
        
        return new ValidationResult(GetErrorMessage());
    }
}