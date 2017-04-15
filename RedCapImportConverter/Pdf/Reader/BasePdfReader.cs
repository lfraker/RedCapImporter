using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Reader
{
    public abstract class BasePdfReader
    {
        protected IList<IPdfValidation> Validations;
        protected string CurrentLine;
        protected StringReader Reader;

        public abstract void Reset();

        public abstract void SkipToLine(int lineNumber);

        public abstract string ReadRest();

        public abstract string ReadLine();

        public BasePdfReader()
        {
            this.Validations = new List<IPdfValidation>();
        }

        public string GetCurrentLine()
        {
            return this.CurrentLine;
        }

        protected string Read()
        {
            return this.Reader.ReadLine()?.Trim();
        }

        public bool RunValidations(string line)
        {
            if (line == null)
            {
                return true;
            }
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
