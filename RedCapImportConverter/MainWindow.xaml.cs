using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using RedCapImportConverter.DataExtractors;
using RedCapImportConverter.Pdf.Reader;
using RedCapImportConverter.Pdf.Parser;
using RedCapImportConverter.Model;
using RedCapImportConverter.Pdf.Rules.Import;
using RedCapImportConverter.Pdf.Rules;
using RedCapImportConverter.Exporters.Csv;

namespace RedCapImportConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PdfReaderPaged pdfPaged = Pdf.Parser.PdfParser.ParsePdfPaged(@"C:\PROJECTS\PDF_To_Text\IO_Files\ABPMasPDF.pdf");
            AbpmModel model = new AbpmModel();
            RuleImporter rules = new RuleImporter(model);
            IList<IPdfParsingRule> reuls = rules.GetRules(@"C:\PROJECTS\PDF_To_Text\IO_Files\ABPMRules.txt");
            foreach (IPdfParsingRule rule in reuls)
            {
                rule.ExecuteRule(pdfPaged);
            }
            CsvExporter csv = new CsvExporter(@"C:\PROJECTS\PDF_To_Text\IO_Files\ABPMRule.csv");
            csv.AddModelEntry(model);
            csv.WriteToCsv();
            InitializeComponent();
        }
    }
}
