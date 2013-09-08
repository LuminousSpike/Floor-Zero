using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            //WebBrowser.Source = new Uri("http://www.bituser.com/nathan/Floor_Zero/News.html");
            Lbl_CurrentVersion.Content = "Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            TidyUp();
            if (!File.Exists("Updated"))
            {
                CheckForUpdate();
            }
            
        }

        private void TB_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TB_Minimise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.MainWindow.DragMove();
        }

        private void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Floor Zero.exe");
            this.Close();
        }

        private void CheckForUpdate()
        {
            Helpers.DownloadFile(null, "Update.xml");
            Helpers.DownloadFile(null, "ReleaseNotes.xml");
            // Phrase the XML for Version
            // And if newer, download.
            XmlReader reader = XmlReader.Create("Update.xml");
            string version = null, fileName = null;
            while (reader.Read())
            {
                
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Version")
                {
                    version = reader.ReadElementContentAsString();
                }
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "FileName")
                {
                    fileName = reader.ReadElementContentAsString();
                }
            }

            if (version != System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                UpdateDialog dialog = new UpdateDialog(UpdateDialogType.Launcher, version, fileName);
                dialog.Show();
            }

            PhraseReleaseNotes();
        }

        private void PhraseReleaseNotes()
        {
            XmlReader reader = XmlReader.Create("ReleaseNotes.xml");

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Version")
                {
                    RTB_ReleaseNotes.AppendText(reader.ReadElementContentAsString() + "\n");
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "Note")
                {
                    RTB_ReleaseNotes.AppendText(reader.ReadElementContentAsString() + "\n");
                }
            }
        }

        private void TidyUp()
        {
            if (File.Exists("Updated"))
            {
                MessageBox.Show("Update Complete!");
                File.Delete("Updated");
                File.Delete("Launcher Backup.exe");
                File.Delete("Launcher Update.exe");
            }
            else if (File.Exists("Updating"))
            {
                MessageBox.Show("Updating");
                File.Replace("Launcher Updater.exe", "Launcher.exe", "Launcher Backup.exe");
                File.Delete("Updating");
                File.Create("Updated");
                Process.Start("Launcher.exe");
                this.Close();
            }
            
        }
    }
}
