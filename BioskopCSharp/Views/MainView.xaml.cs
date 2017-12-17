using System;
using System.Collections.Generic;
using System.Data;
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
        //Class Deklarasi
        private CMain _ctrl;

        //Contructors
        public MainView()
        {
            InitializeComponent();
            
        }

        //Event On Window
        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            UserAktiv.Content = App.UserLog;
            _ctrl = CMain.GetInstance;
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_ctrl != null)
            {
                _ctrl.Dispose();
            }
            GC.Collect();
            App.Current.Shutdown();
        }

        private void TblMainDataFilm_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void TblMainDataFilm_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblMainDataFilm.SelectedItems.Count > 0)
            {
                _ctrl.CodeFilm = ((DataRowView)TblMainDataFilm.SelectedItems[0])[0].ToString();
                _ctrl.GetWaktu();
            }
        }




        // ---------------------------- Menu ------------------------------------->
        //Submenu File
        private void MnuLogout_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            CUser.GetInstance.Index("Register");
            App.UserLog = string.Empty;
        }

        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            App.Current.Shutdown();
        }

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            CJadwal.GetInstance.Index();
        }
        
        //Submenu Edit
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
            CTiket.GetInstance.Index();
        }

        private void MnuFilm_Click(object sender, RoutedEventArgs e)
        {
            CFilm.GetInstance.Index();
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
    }
}
