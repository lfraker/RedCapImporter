using RedCapImportConverter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedCapImportConverter.Exporters.Csv
{
    public class CsvExporter
    {
        private readonly string FilePath;
        private IDictionary<string, List<string>> CsvStoredValues;
        private IList<BaseModel> ModelEntries;

        public CsvExporter(string filePath)
        {
            this.FilePath = filePath;
            this.ModelEntries = new List<BaseModel>();
        }

        public void AddModelEntry(BaseModel model)
        {
            this.ModelEntries.Add(model);
        }

        public IDictionary<string, List<string>> GetCsvValues => this.CsvStoredValues;

        private void BuildDictionary()
        {
            this.CsvStoredValues = this.ModelEntries.SelectMany(m => m.GetHeaders()).Distinct().ToDictionary(k => k, v => new List<string>());

            foreach(BaseModel model in this.ModelEntries)
            {
                foreach (string header in model.GetHeaders())
                {
                    string property = model.GetPropertyValue<string>(header) ?? "";
                    this.CsvStoredValues[header].Add(property);
                }
            }
        }

        public void BuildGenericDictionary()
        {
            this.CsvStoredValues = new Dictionary<string, List<string>>();
            foreach (BaseModel model in this.ModelEntries)
            {
                model.ModelProperties.ToList().ForEach(kvp => this.CsvStoredValues.Add(kvp.Key, new List<string>() { kvp.Value }));
            }
        }

        public void WriteToCsv()
        {
            this.BuildGenericDictionary();
            string csvLines = string.Join(",", this.CsvStoredValues.Keys);
            int index = 0;
            while (index < this.CsvStoredValues.Values.Max(lst => lst.Count()))
            {
                csvLines += "\n" + string.Join(",", this.CsvStoredValues.Values.Select(lst => lst[index]));
                index++;
            }

            File.WriteAllText(this.FilePath, csvLines);
        }
    }
}
