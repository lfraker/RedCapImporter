using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Reader
{
    public class PdfReader : BasePdfReader
    {
        private readonly string text;

        public string Text => this.text;

        public PdfReader(string parsedText)
            : base()
        {
            this.text = parsedText;
            this.Reset();
        }

        public override string ReadLine()
        {
            this.CurrentLine = this.Read();
            this.RunValidations(this.CurrentLine);
            return this.CurrentLine;
        }

        public override string ReadRest()
        {
            return this.Reader.ReadToEnd();
        }

        public override void Reset()
        {
            this.Reader = new StringReader(this.text);
        }

        public override void SkipToLine(int lineNumber)
        {
            int ctr = 0;

            while (ctr < lineNumber)
            {
                this.Read();
                ctr++;
            }
        }
    }
}
