using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace trifenix.connect.mdm.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RangeNumber : ValidationAttribute
    {
        public RangeNumber(string message)
        {
            ErrorMessage = message;
        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (!double.TryParse(value.ToString(), out var numbr))
            {
                return false;
            }

            if (numbr==0 || numbr <0)
            {
                return false;
            }

            return true;

            
        }
    }
}
