using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Validations
{
    public class StringValidation : IPdfValidation
    {
        private string ValidationValue;

        private int RemainingLines;

        public StringValidation(string validationValue, int remainingLines)
        {
            this.ValidationValue = validationValue.Trim();
            this.RemainingLines = remainingLines;
        }

        public bool? Validate(string lineEntry)
        {
            if (RemainingLines > 0)
            {
                this.RemainingLines--;
                return null;
            }
            else
            {
                return (lineEntry.Contains(this.ValidationValue));
            }
        }
    }
}
