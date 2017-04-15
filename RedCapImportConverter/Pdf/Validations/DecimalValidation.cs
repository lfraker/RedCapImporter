using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Validations
{
    public class DecimalValidation : IPdfValidation
    {
        private decimal ValidationValue;

        private int RemainingLines;

        public DecimalValidation(decimal validationValue, int remainingLines)
        {
            this.ValidationValue = validationValue;
            this.RemainingLines = remainingLines;
        }

        public bool? Validate(string decimalString)
        {
            if (RemainingLines > 0)
            {
                this.RemainingLines--;
                return null;
            }
            else
            {
                decimal parsedValue;
                if (decimal.TryParse(decimalString, out parsedValue))
                {
                    return (this.ValidationValue == parsedValue);
                }
                else
                {
                    throw new Exception($"Validation failed, decimal {this.ValidationValue} was expected, but no decimal was found in line {decimalString}");
                }
            }
        }
    }
}
