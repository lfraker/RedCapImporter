using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedCapImportConverter.Pdf.Reader;

namespace RedCapImportConverter.Pdf.Rules
{
    public interface IPdfParsingRule
    {
        bool ExecuteRule(PdfReaderPaged pdfReader);
    }
}
