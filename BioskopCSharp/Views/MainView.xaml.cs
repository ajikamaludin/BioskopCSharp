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
using System.Windows.Threading;
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
        private DispatcherTimer _timer;

        //Contructors
        public MainView()
        {
            InitializeComponent();
        }

        //Event On Window
        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            //Time Set
            _timer = new DispatcherTimer();
            _timer.Tick += new EventHandler(TmrTimer_Tick);
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();

            _ctrl = CMain.GetInstance;
            CboMainDataWaktu.IsEnabled = BtnDelete.IsEnabled = BtnPrint.IsEnabled = BtnDone.IsEnabled = false;
            _ctrl.DisableKursi();
            _ctrl.GetTiket();

        }

        //Date And Timer
        public void TmrTimer_Tick(object sender, EventArgs e)
        {
            Tgl.Content = Convert.ToDateTime(DateTime.Now).ToString("dd-MMMM-yyyy / HH:mm:s");
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

        //Pilih Film
        private void TblMainDataFilm_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "IDJADWAL":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "IDFILM":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void TblMainDataFilm_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblMainDataFilm.SelectedItems.Count > 0)
            {
                _ctrl.CodeFilm = ((DataRowView)TblMainDataFilm.SelectedItems[0])[1].ToString();
                _ctrl.GetWaktu();
                CboMainDataWaktu.IsEnabled = true;
            }
        }

        //Kasir Punya
        private void TblMainDataKasir_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblMainDataKasir.SelectedItems.Count > 0)
            {
                BtnDelete.IsEnabled = true;
            }
        }

        private void TblMainDataKasir_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CboMainDataWaktu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CboMainDataWaktu.SelectedIndex > 0)
            {
                _ctrl.CodeJadwal = CboMainDataWaktu.SelectedValue.ToString();
                _ctrl.GetKursi();
            }
            else
            {
                _ctrl.DisableKursi();
            }
        }


        //Click
        //Pilih Film
        private void TblMainDataKasir_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "IDTIKET":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
           _ctrl.CreateTiket();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.SetClear();
        }

        //Kasir Punya
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (TblMainDataKasir.SelectedItems.Count > 0)
            {
                _ctrl.CodeTiket = ((DataRowView)TblMainDataKasir.SelectedItems[0])[0].ToString();
                _ctrl.DeleteTiket();
            }
            else
            {
                MessageBox.Show("Apa yang mau anda hapus ?", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                BtnDelete.IsEnabled = false;
            }
        }

        private void BtnBayar_Click(object sender, RoutedEventArgs e)
        {
            
            if (TblMainDataKasir.Items.Count > 0)
            {
                if (_ctrl.GetKembalian())
                {
                    BtnPrint.IsEnabled = BtnDone.IsEnabled = true;
                    BtnAdd.IsEnabled = BtnRefresh.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Apa yang mau anda bayar ?", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.DoneTrans();
        }


        // ---------------------------- Menu ------------------------------------->
        //Submenu File
        private void MnuLogout_Click(object sender, RoutedEventArgs e)
        {
            UserAktiv.Content = string.Empty;
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


        // Event On Checked
        private void KursiA1_Checked(object sender, RoutedEventArgs e)
        {

        }

        //To Decor Withd Table Of All
        private void TblDecor_Film()
        {

        }

        private void TblDecor_Kasir()
        {

        }
        
    }
}
