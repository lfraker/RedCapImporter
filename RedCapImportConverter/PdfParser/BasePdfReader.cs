using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser
{
    public abstract class BasePdfReader
    {
        protected IList<IPdfValidation> Validations;

        public abstract void Reset();

        public abstract void SkipToLine(int lineNumber);

        public abstract string ReadRest();

        public abstract string ReadLine();

        public bool RunValidations(string line)
        {
            IList<IPdfValidation> remainingValidations = new List<IPdfValidation>();
            bool validationResults = true;

            foreach (IPdfValidation validation in this.Validations)
            {
                bool? validationResult = validation.Validate(line);
                if (!validationResult.HasValue)
                {
                    remainingValidations.Add(validation);
                }
                else
                {
                    validationResults = validationResults && validationResult.Value;
                }
            }
            this.Validations = remainingValidations;
            return validationResults;
        }

    }
}
