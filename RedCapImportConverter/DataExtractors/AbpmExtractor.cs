using RedCapImportConverter.Model;
using RedCapImportConverter.Pdf.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.DataExtractors
{
    public class AbpmExtractor : DataExtractor<PdfReaderPaged>
    {

        private BaseModel model;

        public AbpmExtractor(PdfReaderPaged pdf)
            : base(pdf)
        {
            this.model = new BaseModel();
        }
    }
}
