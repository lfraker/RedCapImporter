using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.PdfParser
{
    public class PdfReaderPaged : BasePdfReader
    {
        private readonly IList<string> pages;
        private StringReader reader;
        private int pageIndex;

        public string Page(int pageNumber) => this.pages[pageNumber];

        public int PageNumber => this.pageIndex;

        public int CurrentPage() => this.pageIndex;

        public int PageCount() => this.pages.Count();

        public PdfReaderPaged(IList<string> parsedTextPages)
        {
            this.pages = parsedTextPages;
            this.pageIndex = 0;
            this.Reset();
        }

        public override void Reset()
        {
            this.reader = new StringReader(this.pages[this.pageIndex]);
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

        public void SkipToPage(int pageNumber)
        {
            this.pageIndex = pageNumber;
            this.Reset();
        }

        public void DebugWriteToFile(string filePath)
        {
            IList<string> textOutput = new List<string>();
            string line;
            while ((line = this.ReadLine()) != null)
            {
                textOutput.Add(line);
            }
            File.WriteAllLines(filePath, textOutput);
        }

        public bool NextPage()
        {
            this.pageIndex = (this.pageIndex + 1 < this.pages.Count) ? this.pageIndex + 1 : 0;
            this.Reset();
            return (this.pageIndex == 0);
        }

        public override string ReadRest()
        {
            string text = this.reader.ReadToEnd();
            this.NextPage();
            return text;
        }

        public override string ReadLine()
        {
            string line = this.reader.ReadLine().Trim();
            this.RunValidations(line);
            if (line == null)
            {
                this.NextPage();
            }
            return line;
        }
    }
}
