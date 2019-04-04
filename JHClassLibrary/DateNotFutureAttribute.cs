using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JHClassLibrary
{
    public class DateNotFutureAttribute:ValidationAttribute
    {
        public DateNotFutureAttribute()
        {
            ErrorMessage = "{0} Date is invalid. Date cannot be in the future.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime) value!=null)
            {
                if ((DateTime)value > DateTime.Now)
                    return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
