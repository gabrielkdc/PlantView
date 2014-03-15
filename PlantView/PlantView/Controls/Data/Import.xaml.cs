using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Utilities;
using System.ComponentModel;
using PlantView.Properties;
using DataContracts;
using FirstFloor.ModernUI.Windows.Controls;

namespace PlantView.Controls.Data
{
    /// <summary>
    /// Lógica de interacción para Import.xaml
    /// </summary>
    public partial class Import : UserControl
    {
        private BackgroundWorker _background;
        private string[] _filenames;
        private int _index;
        private int _totalRecords;

        public Import()
        {
            InitializeComponent();
            _background = new BackgroundWorker();
            _background.DoWork += ImportFiles;
            _background.ProgressChanged += NotifyChanged;
            _background.RunWorkerCompleted += ImportFinished;
            _background.WorkerReportsProgress = true;

        }

        private void ImportFinished(object sender, EventArgs e)
        {
            processIndicator.IsActive = false;
        }

        private void NotifyChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_index < _filenames.Count())
            {              
                TextEvents.Text = string.Format("{0}% --- File {1} : Operation {2}  \n {3}", e.ProgressPercentage,  _filenames[_index], e.UserState, TextEvents.Text);
            }
        }

        private void ImportFiles(object sender, EventArgs e)
        {          
            while (_index < _filenames.Count())
            {
                _background.ReportProgress(Convert.ToInt16(_index / (_filenames.Count()*1.00m) * 100), "Decoding file:" );
                var file = DataFileDecoder.DecodeBeltwayFile(_filenames[_index], Settings.Default.yearIndex, Settings.Default.monthIndex, Settings.Default.dayIndex, Settings.Default.dateDivider);
                var fileId = ImportedFileOperations.AddImportedFile(_filenames[_index]);
                if (fileId > 0)
                {
                    _background.ReportProgress(1, "File created:");
                    _totalRecords += file.Data.Count;
                    decimal index = 1;
                    foreach (var weighingRecord in file.Data)
                    {
                        WeighingRecordOperations.AddNewWeighing(weighingRecord.Date, weighingRecord.ProductId, weighingRecord.PlantId, weighingRecord.Weight, weighingRecord.TonsPerHour, fileId);
                        index += 1;
                        _background.ReportProgress(Convert.ToInt16(index/file.Data.Count * 100) , "Record added:");
                        
                    }
                }
                else
                {
                    new ModernDialog
                    {
                        Title = Localization.Strings.ErrorMessageTitle,
                        Content =  string.Format("{0} : {1}", _filenames[_index], Localization.Strings.AnErrorOccurred)
                    }.ShowDialog();
                }
                _index++;
            }
        }


        private void selectFilesButton_Click(object sender, RoutedEventArgs e)
        {             
            try
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "TEXT Files(*.txt)|*.txt|CSV Files(*.csv)|*.csv";
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    totalFilesLabel.BBCode = string.Format(Localization.Strings.TotalSelectedFiles, openFileDialog.FileNames.Count());
                    filesLabel.Content = string.Join(";", openFileDialog.SafeFileNames);
                    _filenames = openFileDialog.FileNames;                  
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Log(ex);
            }

        }

        private void processFilesButton_Click(object sender, RoutedEventArgs e)
        {
            processIndicator.IsActive = true;
            _index = 0;
            _totalRecords = 0;
            _background.RunWorkerAsync();
        }
    }
}
