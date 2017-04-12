using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser
{
    public class PdfReader : BasePdfReader
    {
        private readonly string text;
        private StringReader reader;

        public string Text => this.text;

        public PdfReader(string parsedText)
        {
            this.text = parsedText;
            this.Reset();
        }

        public override string ReadLine()
        {
            string line = this.reader.ReadLine().Trim();
            this.RunValidations(line);
            return line;
        }

        public override string ReadRest()
        {
            return this.reader.ReadToEnd();
        }

        public override void Reset()
        {
            this.reader = new StringReader(this.text);
        }

        public override void SkipToLine(int lineNumber)
        {
            int ctr = 0;

            while (ctr < lineNumber)
            {
                reader.ReadLine();
                ctr++;
            }
        }
    }
}
