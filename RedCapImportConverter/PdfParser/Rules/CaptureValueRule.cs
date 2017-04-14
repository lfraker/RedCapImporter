using RedCapImportConverter.DataExtractors;
using RedCapImportConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Rules
{
    public class CaptureValueRule<T> : IPdfParsingRule
    {
        private int RemainingLines;

        private IModelObject ModelObject;

        private string PropertyNameLookup;

        private string PdfTextField;

        private bool RuleExecuting;

        public CaptureValueRule(int remainingLines, IModelObject modelObject, string propertyName, string pdfTextField)
        {
            this.RemainingLines = remainingLines;
            this.ModelObject = modelObject;
            this.PropertyNameLookup = propertyName;
            this.PdfTextField = pdfTextField;
            this.RuleExecuting = false;
        }


        public bool CheckRuleApplicability(string line)
        {

            return (this.RuleExecuting = this.RuleExecuting || line.Contains(this.PdfTextField));
        }

        public string ExecuteRule(BasePdfReader line)
        {
            throw new NotImplementedException("Execute rule with PdfReader not valid for this rule");
        }

        public bool ExecuteRule(string line)
        {
            if (this.RuleExecuting)
            {
                if (this.RemainingLines > 0)
                {
                    this.RemainingLines--;
                }
                else
                {
                    ExtractorUtilities.UpdatePropertyName(this.ModelObject, this.PropertyNameLookup, line);
                    return false;
                }
            }
            return true;
        }
    }
}
