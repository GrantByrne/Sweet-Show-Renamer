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
using MahApps.Metro.Controls;
using System.Windows.Forms;
using System.IO;
using SweetShowRenamer.Renamer;
using System.Collections;
using System.Collections.ObjectModel;
using SweetShowRenamer.Lib.Service.Abstract;
using SweetShowRenamer.Lib.Service;
using SweetShowRenamer.Lib.Domain;


namespace SweetShowRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ShowProcessor shows = new ShowProcessor();
        private readonly ISettingsService _settingsService;
        
        public MainWindow()
        {
            _settingsService = new SettingsService();

            InitializeComponent();

            var settings = _settingsService.Get();
            lblDirectory.Content = settings.LastUsedDirectory;
        }

        private void btnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            DialogResult result = folderDialog.ShowDialog();

            if (!string.IsNullOrEmpty(folderDialog.SelectedPath))
            {
                lblDirectory.Content = folderDialog.SelectedPath;
                var settings = new Settings();
                settings.LastUsedDirectory = lblDirectory.Content.ToString();
                _settingsService.Update(settings);

                shows.LoadDirectory(lblDirectory.Content.ToString());
                shows.LoadShowNames();
            }
        }

        private void btnProcessShowNames_Click(object sender, RoutedEventArgs e)
        {
            shows.ProcessShows();
            lvShows.Items.Refresh();
        }

        private void btnWriteChanges_Click(object sender, RoutedEventArgs e)
        {
            shows.WriteFilenames();
        }

        public ObservableCollection<ShowData> ShowCollection
        {
            get
            {
                return shows.ShowsList;
            }
        }

        
    }
}
