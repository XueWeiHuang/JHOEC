using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JHClassLibrary
{
    public class JHDateNotInFutureAttribute : ValidationAttribute
    {
        public JHDateNotInFutureAttribute()
        {
            ErrorMessage = "{0} Date is invalid. Date cannot be in the future.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString() == "")
                return ValidationResult.Success;

            else if (Convert.ToDateTime(value) != null)
            {
                if (Convert.ToDateTime(value.ToString()) > DateTime.Now)
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
