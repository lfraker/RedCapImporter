﻿using System;
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

using RedCapImportConverter.PdfParser;
using RedCapImportConverter.DataExtractors;

namespace RedCapImportConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PdfReaderPaged pdfPaged = PdfParser.PdfParser.ParsePdfPaged(@"C:\PROJECTS\PDF_To_Text\IO_Files\ABPMasPDF.pdf");
            AbpmExtractor extractor = new AbpmExtractor(pdfPaged);
            InitializeComponent();
        }
    }
}
