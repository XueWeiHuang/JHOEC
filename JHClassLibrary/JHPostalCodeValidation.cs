using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace JHClassLibrary
{
    public class JHPostalCodeValidation:ValidationAttribute
    {
        public JHPostalCodeValidation()
        {
            ErrorMessage = "{0} Postal Code is invalid. The format doesnt match 'A3B C4B'";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            Regex postCodePattern = new Regex(@"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] ?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$", RegexOptions.IgnoreCase);
            if (value==null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else if (postCodePattern.IsMatch(value.ToString()))
            {
                value.ToString().Trim();
                value = value.ToString().Insert(3, " ").ToUpper();
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
            
        }


    }
}
