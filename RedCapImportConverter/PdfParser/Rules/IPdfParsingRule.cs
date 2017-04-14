using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser.Rules
{
    public interface IPdfParsingRule
    {
        string ExecuteRule(BasePdfReader pdfReader);

        bool ExecuteRule(string line);

        bool CheckRuleApplicability(string line);
    }
}
