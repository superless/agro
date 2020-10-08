using System;

namespace trifenix.connect.mdm.validation_attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class RangeNumber : Attribute
    {

        public string ErrorMessage { private get; set; }

        public RangeNumber(string message)
        {
            ErrorMessage = message;
        }
        public bool IsValid(object value)
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
