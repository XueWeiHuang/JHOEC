using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace JHClassLibrary
{
    public class JHZipCodeValidation:ValidationAttribute
    {
        public JHZipCodeValidation()
        {
            ErrorMessage = "{0} Zip Code is invalid. The format doesnt match '123456' or '12345-6789'";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex zipCodePattern = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$", RegexOptions.IgnoreCase);

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            //need to remove any other things excetp numbers
            else if ((Regex.Replace(value.ToString(), "[^0-9]", "")).Length != 5 || (Regex.Replace(value.ToString(), "[^0-9]", "")).Length != 9)
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
            else
            {
                if (zipCodePattern.IsMatch(value.ToString()))
                {
                    if (value.ToString().Length == 5)
                    {
                        value = value.ToString();
                    }
                    else if (value.ToString().Length == 9)
                    {
                        value = value.ToString().Insert(5, "-");
                       
                    }
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
                }
            }
        }        
    }
}
