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
using Awesomium.Core;
using System.Threading;
using System.Diagnostics;

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
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {
            WebBrowser.Source = new Uri("http://www.bituser.com/nathan/Floor_Zero/News.html");
            Lbl_CurrentVersion.Content = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
    }
}
