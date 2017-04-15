using RedCapImportConverter.Pdf.Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Rules
{
    public class SectionRule : IPdfParsingRule
    {
        private IList<IPdfParsingRule> ChildRules;
        private string StartSection;
        private string EndSection;

        public SectionRule(IList<IPdfParsingRule> childRules, string start, string end)
        {
            this.ChildRules = childRules;
            this.StartSection = start;
            this.EndSection = end;
        }

        private bool CheckRuleApplicability(string line)
        {
            return (line.Contains(this.StartSection));
        }

        // Readline from start to end section, return line
        public bool ExecuteRule(PdfReaderPaged pdfReader)
        {
            string line = pdfReader.GetCurrentLine();
            if (!this.CheckRuleApplicability(line))
            {
                return false;
            }
            while (!(line = pdfReader.ReadLine()).Contains(this.EndSection))
            {
                IList<IPdfParsingRule> remainingChildrenRules = new List<IPdfParsingRule>();
                this.ChildRules = this.ChildRules.Where(cRule => !cRule.ExecuteRule(pdfReader)).ToList();
            }

            return true;
        }
    }
}
