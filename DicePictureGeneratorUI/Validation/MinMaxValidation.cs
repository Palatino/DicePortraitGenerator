using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DicePictureGeneratorUI.Validation
{
    internal class MinMaxValidation : ValidationRule
    {

        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int valueInt = 0;
            
            try
            {
                if (((string)value).Length > 0)
                    valueInt = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if (valueInt < Min)
            {
                return new ValidationResult(false, $"Minimum value is {Min}");
            }

            if (valueInt > Max)
            {
                return new ValidationResult(false, $"Maximum value is {Max}");
            }

            return ValidationResult.ValidResult;

        }
    }
}
