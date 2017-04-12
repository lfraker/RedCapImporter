using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Validations
{
    public class IntValidation : IPdfValidation
    {
        private int ValidationValue;

        private int RemainingLines;

        public IntValidation(int validationValue, int remainingLines)
        {
            this.ValidationValue = validationValue;
            this.RemainingLines = remainingLines;
        }

        public bool? Validate(string intString)
        {
            if (RemainingLines > 0)
            {
                this.RemainingLines--;
                return null;
            }
            else
            {
                int parsedValue;
                if (int.TryParse(intString, out parsedValue))
                {
                    return (this.ValidationValue == parsedValue);
                }
                else
                {
                    throw new Exception($"Validation failed, integer {this.ValidationValue} was expected, but no integer was found in line {intString}");
                }
            }
        }
    }
}
