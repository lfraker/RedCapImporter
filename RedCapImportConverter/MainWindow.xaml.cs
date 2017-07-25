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
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace RedCapImportConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string GetFilePrefix(string filePath)
        {
            return System.IO.Path.GetFileName(filePath).Split('_')[0];
        }

        private void ParseFilesButton_Click(object sender, RoutedEventArgs e)
        {
            IList<string> pdfFiles = Directory.GetFiles(PdfFileDirectory.Text, "*.pdf");
            IDictionary<string, string> pdfFilesByPrefixes = pdfFiles.ToDictionary(f => GetFilePrefix(f), f => f);
            IList<string> ruleFiles = Directory.GetFiles(RuleFileDirectory.Text, "*.*").Where(s => s.EndsWith(".txt") || s.EndsWith(".RCITRule")).ToList();
            IDictionary<string, string> ruleFilesByPrefixes = ruleFiles.ToDictionary(f => GetFilePrefix(f), f => f);
            HashSet<string> prefixDifference;
            if ((prefixDifference = new HashSet<string>(pdfFilesByPrefixes.Keys.Except<string>(ruleFilesByPrefixes.Keys))).Any())
            {
                throw new ArgumentException($"Could not find rule file for {string.Join(",", prefixDifference)} files.");
            }
            string csvPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\RedCapImportConverter\IO\Output\{string.Format("{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now)}_Output.csv";
            CsvExporter csv = new CsvExporter(csvPath);
            foreach (KeyValuePair<string, string> pdfFileEntry in pdfFilesByPrefixes)
            {
                PdfReaderPaged pdfPagedEntry = Pdf.Parser.PdfParser.ParsePdfPaged(pdfFileEntry.Value);
                BaseModel model = new BaseModel();
                RuleImporter ruleImporter = new RuleImporter(model);
                IList<IPdfParsingRule> rules = ruleImporter.GetRules(ruleFilesByPrefixes[pdfFileEntry.Key]);
                foreach (IPdfParsingRule rule in rules)
                {
                    rule.ExecuteRule(pdfPagedEntry);
                }
                csv.AddModelEntry(model);
            }

            csv.WriteToCsv();
            PdfFileDirectory.Text = "";
            RuleFileDirectory.Text = "";
            Environment.Exit(0);
        }

        private void PdfFileDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = GetDirectory(sender, e);
            PdfFileDirectory.Text = directory;
            PdfFileDirectory.ToolTip = directory;
        }

        private void ResultsFileDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = GetDirectory(sender, e);
            ResultsFileDirectory.Text = directory;
            ResultsFileDirectory.ToolTip = directory;
        }

        private void RuleFileDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            string directory = GetDirectory(sender, e);
            RuleFileDirectory.Text = directory;
            RuleFileDirectory.ToolTip = directory;
        }

        private string GetDirectory(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    return fileDialog.SelectedPath;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    return null;
            }
        }

        private string GetFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    return fileDialog.FileName;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    return null;
            }
        }

        private void PdfFileViewerButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = GetFile(sender, e);
            PdfFileViewerPath.Text = filePath;
            PdfFileViewerPath.ToolTip = filePath;
            ParsedPdfFileViewerBlock.Text = Pdf.Parser.PdfParser.ParsePdfViewerContent(filePath);
        }

        private void RuleFileEditorButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = GetFile(sender, e);
            RuleFileEditorPath.Text = filePath;
            RuleFileEditorPath.ToolTip = filePath;
            if (File.Exists(filePath))
            {
                FileStatusText.Content = "";
                FileStatusText.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                Thread.Sleep(1000);
                RuleFileEditorBlock.Text = File.ReadAllText(filePath);
                FileStatusText.Content = "File Loaded";
            }
        }

        private void RuleFileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileStatusText.Content = "";
            FileStatusText.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
            Thread.Sleep(1000);
            File.WriteAllText(RuleFileEditorPath.Text, RuleFileEditorBlock.Text);
            FileStatusText.Content = "File Saved";
        }

        private static Action EmptyDelegate = delegate () { };
    }
}
