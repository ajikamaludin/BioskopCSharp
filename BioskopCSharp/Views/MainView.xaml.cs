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
        

        //Contructors
        public MainView()
        {
            InitializeComponent();
        }

        //Event On Window
        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            UserAktiv.Content = App.UserLog;
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GC.Collect();
            App.Current.Shutdown();
        }

        //Submenu File
        private void MnuLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            CUser.GetInstance.Index("Register");
            //App.UserLog = string.Empty;
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
            CRuang.GetInstance.Index();
        }

        private void MnuUser_Click(object sender, RoutedEventArgs e)
        {
            CUser.GetInstance.Index();
        }

        private void MnuTiket_Click(object sender, RoutedEventArgs e)
        {

        }

        //Menu About
        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(App.UserLog);
            MessageBox.Show("FrameWork menggunakan MVC-DesignPattern\n Oleh: Fairul@Amikom \n\nAplikasi Dikerjakan Oleh : \n" +
                " - Aji Kamaludin ( 16.11.0051 ) \n" +
                " - Evriyana Indra Saputra ( 16.11.0014 ) \n" +
                " - Arief Setyo Nugroho ( 16.11.0058 )\n" +
                " - Harish Setyo Hudnanto ( 16.11.0048 )\n" +
                " - Arik Andrian Putra Purwajanu ( 16.11.00.55 )",
                "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
