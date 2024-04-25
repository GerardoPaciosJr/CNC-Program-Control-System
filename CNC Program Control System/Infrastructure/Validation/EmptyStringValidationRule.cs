using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace CNC_Program_Control_System
{
    public class EmptyStringValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //if ((string)value == null || (string)value == string.Empty)
            //{
            //    var msg = "Invalid empty field";
            //    return new ValidationResult(false, msg);
            //}
 
            //return new ValidationResult(true, null);

            // Get and convert the value
            string stringValue = GetBoundValue(value) as string;

            if (String.IsNullOrWhiteSpace(stringValue))
            {
                return new ValidationResult(false, "Must not be empty");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

        private object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                // ValidationStep was UpdatedValue or CommittedValue (Validate after setting)
                // Need to pull the value out of the BindingExpression.
                BindingExpression binding = (BindingExpression)value;

                // Get the bound object and name of the property
                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;

                // Extract the value of the property.
                object propertyValue = dataItem.GetType().GetProperty(propertyName).GetValue(dataItem, null);

                // This is what we want.
                return propertyValue;
            }
            else
            {
                // ValidationStep was RawProposedValue or ConvertedProposedValue
                // The argument is already what we want!
                return value;
            }
        }
    }
}
