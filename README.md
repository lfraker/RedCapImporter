**RedCap Import Converter README**
================================

**Overview**
========
The RedCap Import Converter takes in a set of PDFs and a set of pre-defined rules files to parse specific fields out of the PDFs and combine them into one .csv file output.

**Technical Overview**
=======
**-[TODO]**

**User Interface**
===========
The below section covers the two main user interface pages and how to interact with them.

*PDF Parser*
---------------
>The PDF parser page is where the user selects the input PDF files/Rules files and the output file details.
>The PDF Directory a directory which contains all the PDFs to parse.
>The Rule Directory is a directory containing all the rules files to parse.
>PDF file names should be in the format `<prefix>`_pdf.pdf and your rules files in the format `<prefix>`_rules.text, where the rules file prefix matches its corresponding pdf file prefix.
>The Results Directory is any directory where you would like the results to be saved to.
>The Results File Name is name the results file is saved as.
> ![PDF Parser Page](https://github.com/lfraker/RedCapImporter/blob/master/RedCapImportConverter/Images/Screenshots/PDF_Parser_Page.PNG?raw=true)
>
> Upon filling out all required parameters and clicking the "Generate Results File" button, you will be presented with the following screen, showing the results which were saved to the results file.
> ![PDF Parser Page](https://github.com/lfraker/RedCapImporter/blob/master/RedCapImportConverter/Images/Screenshots/Results.PNG?raw=true)

*Rules Editor*
-----------------
>The rules editor view allows the user to edit any rules file.
>Clicking "Load File" under the PDF Text Format Viewer will display a file explorer prompt to select any PDF. Once selected, the textual representation of that PDF will be displayed, with corresponding line numbers. Clicking "Preview PDF" will render the PDF.
>Clicking "Load/Create File" under the Rules File Editor will display a file explorer prompt to select/create any rules file. The window displaying the rules file is editable, and clicking the "Save File" button will save any edits the user makes.
> ![PDF Parser Page](https://github.com/lfraker/RedCapImporter/blob/master/RedCapImportConverter/Images/Screenshots/Rules_Editor_Page.PNG?raw=true)

