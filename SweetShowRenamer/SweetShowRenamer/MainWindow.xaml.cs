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


namespace SweetShowRenamer
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ShowProcessor shows = new ShowProcessor();
        
        public MainWindow()
        {
            InitializeComponent();

            lblDirectory.Content = System.IO.Directory.GetCurrentDirectory();
        }

        private void btnChooseDir_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            DialogResult result = folderDialog.ShowDialog();


            if (!string.IsNullOrEmpty(folderDialog.SelectedPath))
            {
                lblDirectory.Content = folderDialog.SelectedPath;
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
