using System.Globalization;
using System.Windows.Controls;

namespace CNC_Program_Control_System
{
    public class EmptyStringValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((string)value == null || (string)value == string.Empty)
            {
                var msg = "Invalid empty field";
                return new ValidationResult(false, msg);
            }
 
            return new ValidationResult(true, null);
        }
    }
}
