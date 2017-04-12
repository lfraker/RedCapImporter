using RedCapImportConverter.PdfParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedCapImportConverter.DataExtractors
{
    public abstract class DataExtractor<T> where T : class, BasePdfReader
    {
        protected T pdf;

        public DataExtractor(T pdf)
        {
            this.pdf = pdf;
        }

        public string TextBlockPaged(int page, string start, string end = null)
        {
            if (this.pdf.GetType() == typeof(PdfReader))
            {
                throw new Exception("Must call TextBlock with PdfText.");
            }

            (this.pdf as PdfReaderPaged).SkipToPage(page);
            return this.TextBetween(this.pdf.ReadRest(), start, end);
        }

        public string TextBlock(string start, string end = null)
        {
            if (this.pdf.GetType() == typeof(PdfReaderPaged))
            {
                throw new Exception("Must call TextBlockPaged with PdfTextPaged.");
            }

            this.pdf.Reset();
            return this.TextBetween(this.pdf.ReadRest(), start, end);

        }

        private string TextBetween(string text, string start, string end = null)
        {
            if (Regex.Matches(Regex.Escape(text), start).Count != 1)
            {
                throw new Exception($"Found more than one occurence of {start} in text: {text}");
            }

            string splitFirst = text.Split(new string[] { start }, StringSplitOptions.None)[1];

            if (end == null)
            {
                return splitFirst;
            }

            if (Regex.Matches(Regex.Escape(text), end).Count != 1)
            {
                throw new Exception($"Found more than one occurence of {end} in text: {text}");
            }

            if (text.IndexOf(start) > text.IndexOf(end))
            {
                throw new Exception($"{start} occurs after {end} in text: {text}");
            }

            return splitFirst.Split(new string[] { end }, StringSplitOptions.None)[0];

        }
    }
}
