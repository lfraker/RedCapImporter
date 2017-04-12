using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Rules
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

        public bool CheckRuleApplicability(string line)
        {
            return (line.Contains(this.StartSection));
        }

        // Readline from start to end section, return line
        public string ExecuteRule(BasePdfReader pdfReader)
        {
            string line;
            while ((line = pdfReader.ReadLine()) != this.EndSection)
            {
                IList<IPdfParsingRule> remainingChildrenRules = new List<IPdfParsingRule>();
                foreach (IPdfParsingRule childRule in this.ChildRules)
                {
                    if (childRule.CheckRuleApplicability(line))
                    {
                        childRule.ExecuteRule(pdfReader);
                    }
                    else
                    {
                        remainingChildrenRules.Add(childRule);
                    }
                }
                this.ChildRules = remainingChildrenRules;
            }

            return line;
        }
    }
}
