using RedCapImportConverter.Pdf.Reader;
using RedCapImportConverter.Pdf.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Rules
{
    public class PageRule : IPdfParsingRule
    {
        private IList<IPdfParsingRule> ParsingRules;
        private int PageNumber;

        public PageRule(IList<IPdfParsingRule> children, int pageNumber)
        {
            this.ParsingRules = new List<IPdfParsingRule>();
            this.PageNumber = pageNumber - 1;
            this.ParsingRules = children;
        }

        public void AddRule(IPdfParsingRule rule)
        {
            this.ParsingRules.Add(rule);
        }

        public bool ExecuteRule(PdfReaderPaged pdfReader)
        {
            pdfReader.SkipToPage(this.PageNumber);
            string line = pdfReader.GetCurrentLine();

            while ((line = pdfReader.ReadLine()) != null)
            {
                this.ParsingRules = this.ParsingRules.Where(pRule => !pRule.ExecuteRule(pdfReader)).ToList();
            }
            return !this.ParsingRules.Any();
        }
    }
}
