using RedCapImportConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Rules
{
    public class CaptureValueRule : IPdfParsingRule
    {
        private int RemainingLines;

        private IModelObject ModelObject;

        private string PropertyName;

        private string PdfTextField;

        public CaptureValueRule(int remainingLines, IModelObject modelObject, string propertyName, string pdfTextField)
        {
            this.RemainingLines = remainingLines;
            this.ModelObject = modelObject;
            this.PropertyName = propertyName;
            this.PdfTextField = pdfTextField;
        }


        public bool CheckRuleApplicability(string line)
        {
            return (line.Contains(this.PdfTextField));
        }

        public string ExecuteRule(BasePdfReader pdfReader)
        {
            throw new NotImplementedException();
        }
    }
}
