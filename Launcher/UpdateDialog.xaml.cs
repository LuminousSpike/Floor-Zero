﻿using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Launcher
{
    public enum UpdateDialogType
    {
        Launcher,
        Game
    }

    /// <summary>
    ///     Interaction logic for UpdateDialog.xaml
    /// </summary>
    public partial class UpdateDialog : Window
    {
        private readonly string FileName;
        private readonly UpdateDialogType Type;
        private readonly string Version;

        public UpdateDialog(UpdateDialogType type, string version, string fileName)
        {
            Type = type;
            FileName = fileName;
            Version = version;
            InitializeComponent();
            SetDialogText();
            Topmost = true;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Replace with constuction setting please
        private void SetDialogText()
        {
            if (Type == UpdateDialogType.Game)
            {
                Lbl_DialogText.Content = "An update for the game has been found!";
            }
            else if (Type == UpdateDialogType.Launcher)
            {
                Lbl_DialogText.Content = "An update for the launcher has been found!" + "\nCurrent Version: "
                                         + Assembly.GetExecutingAssembly().GetName().Version + "\nNew Version: " +
                                         Version;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TB_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (Type == UpdateDialogType.Game)
            {
                UpdateGame(FileName);
            }
            else if (Type == UpdateDialogType.Launcher)
            {
                UpdateLauncher(FileName);
            }
        }

        private void UpdateLauncher(string fileName)
        {
            Helpers.DownloadFile("Launcher/", fileName);
            File.Create("Updating");
            File.Copy("Launcher Update.exe", "Launcher Updater.exe");
            Process.Start("Launcher Update.exe");
            Application.Current.MainWindow.Close();
            Close();
        }

        private void UpdateGame(string fileName)
        {
            Helpers.DownloadFile("Game/", fileName);
        }
    }
}