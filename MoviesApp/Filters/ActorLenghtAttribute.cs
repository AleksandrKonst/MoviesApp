using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Filters;

public class ActorLenghtAttribute : ValidationAttribute
{
    public ActorLenghtAttribute(int lenght)
    {
        WordLenght = lenght;
    }

    private int WordLenght { get; }
    
    public string GetErrorMessage() =>
        $"Lenght must be more {WordLenght} .";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string)
        {
            var length = ((string) value).Length;

            if (length < WordLenght)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
        
        return new ValidationResult("Bad type");
    }
}