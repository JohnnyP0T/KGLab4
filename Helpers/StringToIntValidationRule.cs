using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KGLab4.Helpers
{
    internal class StringToIntValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i;
            if (int.TryParse(value.ToString(), out i) && i > 0)
                return new ValidationResult(true, null);

            return new ValidationResult(false, "Введите целое число. и больше 0");
        }
    }
}
