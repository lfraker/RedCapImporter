using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Reader
{
    public class PdfReaderPaged : BasePdfReader
    {
        private readonly IList<string> pages;
        private int pageIndex;

        public string Page(int pageNumber) => this.pages[pageNumber];

        public int PageNumber => this.pageIndex;

        public int CurrentPage() => this.pageIndex;

        public int PageCount() => this.pages.Count();

        public PdfReaderPaged(IList<string> parsedTextPages)
            : base()
        {
            this.pages = parsedTextPages;
            this.pageIndex = 0;
            this.Reset();
        }

        public override void Reset()
        {
            this.Reader = new StringReader(this.pages[this.pageIndex]);
        }

        public override void SkipToLine(int lineNumber)
        {
            int ctr = 0;

            while (ctr < lineNumber)
            {
                this.CurrentLine = this.Read();
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
            string text = this.Reader.ReadToEnd();
            this.NextPage();
            return text;
        }

        public override string ReadLine()
        {
            this.CurrentLine = this.Read();
            this.RunValidations(this.GetCurrentLine());
            if (this.GetCurrentLine() == null)
            {
                this.NextPage();
            }
            return this.GetCurrentLine();
        }
    }
}
