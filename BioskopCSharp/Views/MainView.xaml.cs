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
using System.Windows.Shapes;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        //Class Deklarate
        private CUser _ctrl;

        //Contructors
        public MainView()
        {
            InitializeComponent();
        }

        //Event On Window
        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CUser.GetInstance;
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GC.Collect();
            App.Current.Shutdown();
        }

        //Submenu File
        private void MnuLogout_Click(object sender, RoutedEventArgs e)
        {
            App.UserLog = string.Empty;
            Close();
            _ctrl.Index("Register");
        }

        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            App.Current.Shutdown();
        }

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {

        }
        
        //Submenu Edit
        private void MnuFilm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnuRuang_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnuUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnuTiket_Click(object sender, RoutedEventArgs e)
        {

        }

        //Menu About
        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(App.UserLog);
            MessageBox.Show("FraemWork menggunakan MVC-DesignPattern\n Oleh: Fairul@Amikom \nAplikasi Dikerjakan Oleh : \n  - Aji Kamaludin \n - Indra \n - Arik \n - Harish \n - Arief", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }



    }
}
