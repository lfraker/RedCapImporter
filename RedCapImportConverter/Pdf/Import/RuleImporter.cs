using RedCapImportConverter.Model;
using RedCapImportConverter.PdfParser.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Pdf.Rules.Import
{
    public class RuleImporter
    {
        private BaseModel modelObject;
        private int? pageNumber;
        private int sectionCount;
        private const string StartPage = "StartPage";
        private const string EndPage = "EndPage";
        private const string StartSection = "StartSection";
        private const string EndSection = "EndSection";
        private const string Value = "Value";

        public RuleImporter(BaseModel model)
        {
            this.modelObject = model;
            this.sectionCount = 0;
            this.pageNumber = null;
        }


        private IPdfParsingRule GenericParse(StreamReader reader, string line)
        {
            string [] lineSplit = line.Split(':');
            switch (GetHeader(line))
            {
                case RuleImporter.StartPage:
                    return PageParse(reader, line);

                case RuleImporter.EndPage:
                    return null;

                case RuleImporter.StartSection:
                    return SectionParse(reader, line);

                case RuleImporter.EndSection:
                    return null;

                case RuleImporter.Value:
                    return ValueParse(line);

                default:
                    throw new Exception($"Invalid rule syntax at line {line}, starting with {lineSplit[0]}");
            }
        }

        private static string GetHeader(string line)
        {
            return line.Split(':')[0].Trim();
        }

        private static string GetValue(string line)
        {
            return line.Split(':')[1].Trim();
        }

        private static int IntParse(string intString, string line)
        {
            int intParse;
            if (int.TryParse(intString, out intParse))
            {
                return intParse;
            }
            else
            {
                throw new Exception($"Unable to parse integer at line {intString} at line {line}");
            }
        }

        private IPdfParsingRule SectionParse(StreamReader reader, string line)
        {
            this.sectionCount++;
            string start = GetValue(line);
            string readLine;
            IList<IPdfParsingRule> childRules = new List<IPdfParsingRule>();
            while (GetHeader((readLine = reader.ReadLine())) != RuleImporter.EndSection) {
                IPdfParsingRule rule;
                if ((rule = GenericParse(reader, readLine)) != null)
                {
                    childRules.Add(rule);
                }
            }
            this.sectionCount--;
            string end = GetValue(readLine);
            return new SectionRule(childRules, start, end);
        }

        private IPdfParsingRule ValueParse(string line)
        {
            string [] ruleParemeters = line.Split(':')[1].Split(',').Select(param => param.Trim()).ToArray();
            int lineCount = IntParse(ruleParemeters[1], line);
            return new CaptureValueRule<string>(lineCount, modelObject, ruleParemeters[2], ruleParemeters[0]);
        }

        private IPdfParsingRule PageParse(StreamReader reader, string line)
        {
            if (this.pageNumber.HasValue)
            {
                throw new Exception($"Attempting to open a page within another page rule {line}");
            }

            this.pageNumber = IntParse(GetValue(line), line);

            string readLine;
            IList<IPdfParsingRule> childRules = new List<IPdfParsingRule>();
            while (GetHeader((readLine = reader.ReadLine())) != RuleImporter.EndPage)
            {
                IPdfParsingRule rule;
                if ((rule = GenericParse(reader, readLine)) != null)
                {
                    childRules.Add(rule);
                }
            }

            int endPageNumber = IntParse(GetValue(readLine), readLine);

            if (endPageNumber != this.pageNumber)
            {
                throw new Exception($"Found closing page number rule: {endPageNumber} when current page number is {this.pageNumber}");
            }

            return new PageRule(childRules, this.pageNumber.Value);
        }

        public IList<IPdfParsingRule> GetRules(string filePath)
        {
            StreamReader file = new StreamReader(filePath);
            string line;
            IList<IPdfParsingRule> pdfParsingRules = new List<IPdfParsingRule>();
            while ((line = file.ReadLine()) != null)
            {
                IPdfParsingRule rule;
                if ((rule = GenericParse(file, line)) != null)
                {
                    pdfParsingRules.Add(rule);
                }
            }
            return pdfParsingRules;
        }
    }
}
