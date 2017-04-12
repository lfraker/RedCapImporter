using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser
{
    public interface IPdfValidation
    {
        bool? Validate(string stringToValidate);
    }
}
