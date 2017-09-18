using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using RedCapImportConverter.Pdf.Reader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Parser
{
    public static class PdfParser
    {
        public static PdfReaderPaged ParsePdfPaged(string filePath)
        {
            PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(filePath);
            IList<string> pageTexts = new List<string>();
            foreach (PdfPage page in document.Pages) {
                CObject content = ContentReader.ReadContent(page);
                IEnumerable<string> extractedText = ExtractText(content);
                pageTexts.Add(string.Join("\n", extractedText));
            }

            string debugText = string.Join("\n\n** Page Break **\n\n", pageTexts);
            string debugPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\RedCapImportConverter\IO\Temp\{System.IO.Path.GetFileName(filePath).Split('_')[0]}.txt";
            //File.WriteAllText(debugPath, debugText);

            return new PdfReaderPaged(pageTexts);
        }

        public static string ParsePdfViewerContent(string filePath)
        {
            PdfDocument document = PdfSharp.Pdf.IO.PdfReader.Open(filePath);
            int pageNumber = 0;
            string parsedText = "";
            foreach (PdfPage page in document.Pages)
            {
                parsedText += $"\n\n** Page Break: [{pageNumber}] **\n\n";
                CObject content = ContentReader.ReadContent(page);
                IEnumerable<string> extractedText = ExtractText(content);
                int lineNumber = 0;
                foreach (string textElement in extractedText)
                {
                    parsedText += $"[Line {lineNumber}]: {textElement}\n";
                    lineNumber++;
                }
                pageNumber++;
            }

            return parsedText;
        }

        public static Reader.PdfReader ParsePdf(string filePath)
        {
            return new Reader.PdfReader(ExtractText(filePath));
        }

        private static String ExtractText(string filePath)
        {
            String outputText = "";
            try
            {
                PdfDocument inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfDocumentOpenMode.ReadOnly);

                foreach (PdfPage page in inputDocument.Pages)
                {
                    for (int index = 0; index < page.Contents.Elements.Count; index++)
                    {

                        PdfDictionary.PdfStream stream = page.Contents.Elements.GetDictionary(index).Stream;
                        outputText += PDFParserHelper.ExtractTextFromPDFBytes(stream.Value);
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return outputText;
        }

        private static IEnumerable<string> ExtractText(CObject cObject)
        {
            var textList = new List<string>();
            if (cObject is COperator)
            {
                var cOperator = cObject as COperator;
                if (cOperator.OpCode.Name == OpCodeName.Tj.ToString() ||
                    cOperator.OpCode.Name == OpCodeName.TJ.ToString())
                {
                    foreach (var cOperand in cOperator.Operands)
                    {
                        textList.AddRange(ExtractText(cOperand));
                    }
                }
            }
            else if (cObject is CSequence)
            {
                var cSequence = cObject as CSequence;
                foreach (var element in cSequence)
                {
                    textList.AddRange(ExtractText(element));
                }
            }
            else if (cObject is CString)
            {
                var cString = cObject as CString;
                textList.Add(cString.Value);
            }
            return textList;
        }
    }
}
