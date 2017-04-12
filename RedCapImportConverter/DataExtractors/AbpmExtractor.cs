using RedCapImportConverter.Model;
using RedCapImportConverter.PdfParser;
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

        private IModelObject abpmModel;

        public AbpmExtractor(PdfReaderPaged pdf)
            : base(pdf)
        {
            this.abpmModel = new AbpmModel();
            IDictionary<string, string> allEntries = this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "ALL", "DAY"));
            IDictionary<string, string> dayEntries = this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "DAY", "NIGHT"));
            IDictionary<string, string> nightEntries = this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "NIGHT"));
            DebugWriteToCsv(@"C:\PROJECTS\PDF_To_Text\IO_Files\ABPMasPDFDebug.csv", allEntries, dayEntries, nightEntries);

            this.SetFieldValues(this.abpmModel, "all", this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "ALL", "DAY")));
            this.SetFieldValues(this.abpmModel, "day", this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "DAY", "NIGHT")));
            this.SetFieldValues(this.abpmModel, "nit", this.GetValuesByTimeframe(TextBlockPaged((this.pdf.PageCount() - 1), "NIGHT")));

        }

        public void DebugWriteToCsv(string filePath, IDictionary<string, string> allVals, IDictionary<string, string> dayVals, IDictionary<string, string> nightVals)
        {
            IList<string> linesToWrite = new List<string>();
            string lineOne = "";
            foreach (string key in allVals.Keys)
            {
                lineOne += key.Replace("-", "all") + ",";
            }

            foreach (string key in dayVals.Keys)
            {
                lineOne += key.Replace("-", "day") + ",";
            }

            foreach (string key in nightVals.Keys)
            {
                lineOne += key.Replace("-", "nit") + ",";
            }

            string lineTwo = "";

            foreach (string value in allVals.Values)
            {
                lineTwo += value + ",";
            }

            foreach (string value in dayVals.Values)
            {
                lineTwo += value + ",";
            }

            foreach (string value in nightVals.Values)
            {
                lineTwo += value + ",";
            }
            linesToWrite.Add(lineOne);
            linesToWrite.Add(lineTwo);
            File.WriteAllLines(filePath, linesToWrite);
        }

        public void SetFieldValues(IModelObject model, string time, IDictionary<string, string> properties)
        {
            Type modelType = typeof(AbpmModel);
            foreach (KeyValuePair<string, string> kvp in properties)
            {
                string fieldName = kvp.Key.Replace("-", time);
                FieldInfo property = modelType.GetField(fieldName,
                    BindingFlags.Public | BindingFlags.Instance);

                property.SetValue(model, kvp.Value);
            }
        }

        public IDictionary<string, string> GetValuesByTimeframe(string subText)
        {
            StringReader reader = new StringReader(subText);
            IDictionary<string, string> valueDictionary = new Dictionary<string, string>();
            this.SkipToLine(reader, "Valid Measurements");
            valueDictionary["numval-_abp"] = reader.ReadLine().Trim();

            int numVal;
            int.TryParse(valueDictionary["numval-_abp"].Split(' ')[0].Trim(), out numVal);
            valueDictionary["val15-_abp"] = (numVal > 15) ? "Yes" : "No";

            this.SkipToLine(reader, "Pulse Pressure Average");
            valueDictionary["ppres_-_abp"] = reader.ReadLine().Trim();

            if (subText.Contains("MABP(% Dipping)"))
            {
                this.SkipToLine(reader, "MABP(% Dipping)");
                valueDictionary["dip_abp"] = reader.ReadLine().Trim();
            }

            this.SkipToLine(reader, "Systolic");
            valueDictionary["av-_sys_abp"] = reader.ReadLine().Trim();
            valueDictionary["std-_sys_abp"] = reader.ReadLine().Trim();

            this.SkipToLine(reader, "Diastolic");
            valueDictionary["av-_dia_abp"] = reader.ReadLine().Trim();
            valueDictionary["std-_dia_abp"] = reader.ReadLine().Trim();

            this.SkipToLine(reader, "Heart Rate");
            valueDictionary["av-_hr_abp"] = reader.ReadLine().Trim();
            valueDictionary["std-_hr_abp"] = reader.ReadLine().Trim();

            this.SkipToLine(reader, "MABP");
            valueDictionary["av-_mabp_abp"] = reader.ReadLine().Trim();
            valueDictionary["std-_mabp_abp"] = reader.ReadLine().Trim();

            return valueDictionary;
        }

        private void SkipToLine(StringReader reader, string breakPoint)
        {
            while (!reader.ReadLine().Contains(breakPoint)) { }
        }
    }
}
