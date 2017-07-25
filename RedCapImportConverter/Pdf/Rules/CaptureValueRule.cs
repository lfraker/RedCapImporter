using RedCapImportConverter.DataExtractors;
using RedCapImportConverter.Model;
using RedCapImportConverter.Pdf.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Rules
{
    public class CaptureValueRule<T> : IPdfParsingRule
    {
        private int RemainingLines;

        private BaseModel ModelObject;

        private string PropertyName;

        private string PdfTextField;

        private bool RuleExecuting;

        public CaptureValueRule(int remainingLines, BaseModel modelObject, string propertyName, string pdfTextField)
        {
            this.RemainingLines = remainingLines;
            this.ModelObject = modelObject;
            this.PropertyName = propertyName;
            this.PdfTextField = pdfTextField;
            this.RuleExecuting = false;
        }


        private bool CheckRuleApplicability(string line)
        {

            return (this.RuleExecuting = this.RuleExecuting || line.Contains(this.PdfTextField));
        }

        public bool ExecuteRule(PdfReaderPaged reader)
        {
            string line = reader.GetCurrentLine();
            if (!this.CheckRuleApplicability(line))
            {
                return false;
            }

            if (this.RuleExecuting)
            {
                if (this.RemainingLines > 0)
                {
                    this.RemainingLines--;
                }
                else
                {
                    //ExtractorUtilities.UpdateProperty(this.ModelObject, this.PropertyName, line);
                    this.ModelObject.SetProperty(this.PropertyName, line);
                    return true;
                }
            }
            return false;
        }
    }
}
