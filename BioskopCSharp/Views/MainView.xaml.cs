using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private bool Pay = false;
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
            TxtUangBayar.Text = string.Empty;
        }

        //Date And Timer
        public void TmrTimer_Tick(object sender, EventArgs e)
        {
            Tgl.Content = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy");
            Tgl2.Content = Convert.ToDateTime(DateTime.Now).ToString("HH:mm:s");
            string Date = Convert.ToDateTime(DateTime.Now).ToString("dd-MM-yyyy / HH:mm:s");
            //Console.WriteLine(Date);
        }

        private void FrmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var msg = MessageBox.Show("Yakin anda ingin menutup aplikasi ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                if (_ctrl != null)
                {
                    _ctrl.Dispose();
                }
                GC.Collect();
                App.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
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
            if (!Pay)
            {
                if (TblMainDataFilm.SelectedItems.Count > 0)
                {
                    _ctrl.CodeFilm = ((DataRowView)TblMainDataFilm.SelectedItems[0])[1].ToString();
                    _ctrl.GetWaktu();
                    CboMainDataWaktu.IsEnabled = true;
                }
            }
        }

        //Kasir Punya
        private void TblMainDataKasir_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblMainDataKasir.SelectedItems.Count > 0)
            {
                _ctrl.CodeTiket = ((DataRowView)TblMainDataKasir.SelectedItems[0])[0].ToString();
                if (!Pay)
                {
                    BtnDelete.IsEnabled = true;
                }
                if (Pay)
                {
                    BtnPrint.IsEnabled = true;
                }
            }
        }

        private void TblMainDataKasir_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Pay) { 
                if (TblMainDataKasir.SelectedItems.Count > 0)
                {
                    _ctrl.CodeTiket = ((DataRowView)TblMainDataKasir.SelectedItems[0])[0].ToString();
                    _ctrl.Index("PrintTiket");
                    BtnPrint.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Payment salah");
                }
            }
            else
            {
                MessageBox.Show("Bayar Terlebih Dahulu", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void TblMainDataKasir_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (TblMainDataKasir.SelectedItems.Count > 0)
            {
                _ctrl.CodeTiket = ((DataRowView)TblMainDataKasir.SelectedItems[0])[0].ToString();
            }
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
                BtnDelete.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Apa yang mau anda hapus ?", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                BtnDelete.IsEnabled = false;
            }
        }

        private void TxtUangBayar_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnBayar_Click(object sender, RoutedEventArgs e)
        {
            if (TblMainDataKasir.Items.Count > 0)
            {   
                if (TxtUangBayar.Text == string.Empty)
                {
                    MessageBox.Show("Anda Belum Memasukan Uang Pembawayan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    TxtUangBayar.Text =  TxtUangBayar.Text.Trim();
                    if (_ctrl.GetKembalian())
                    {
                        BtnDone.IsEnabled = Pay = true;
                        BtnAdd.IsEnabled = BtnRefresh.IsEnabled = BtnBayar.IsEnabled = BtnDelete.IsEnabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Apa yang mau anda bayar ?", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (TblMainDataKasir.SelectedItems.Count > 0)
            {
                _ctrl.CodeTiket = ((DataRowView)TblMainDataKasir.SelectedItems[0])[0].ToString();
                _ctrl.Index("PrintTiket");
                BtnPrint.IsEnabled = false;
            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            
            if (_ctrl.DoneTrans())
            {
                Pay = false;
                BtnBayar.IsEnabled = true;
            }
        }


        // ---------------------------- Menu ------------------------------------->
        //Submenu File
        private void MnuLogout_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Yakin anda ingin keluar ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                UserAktiv.Content = string.Empty;
                Hide();
                CUser.GetInstance.Index("Register");
                App.UserLog = string.Empty;
            }
        }

        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Yakin anda ingin menutup aplikasi ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                if (_ctrl != null)
                {
                    _ctrl.Dispose();
                }
                GC.Collect();
                App.Current.Shutdown();
            }
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

        private void MnuLaporan_Click(object sender, RoutedEventArgs e)
        {
            CTiket.GetInstance.Index("Laporan");
        }

        //Menu About
        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(App.UserLog);
            //Daftar Nama Anggota
            MessageBox.Show("FrameWork menggunakan MVC-DesignPattern\n Oleh: Fairul@Amikom \n\nAplikasi Dikerjakan Oleh : \n" +
                " - Aji Kamaludin ( 16.11.0051 ) \n" +
                " - Evriyana Indra Saputra ( 16.11.0014 ) \n" +
                " - Arief Setyo Nugroho ( 16.11.0058 )\n" +
                " - Harish Setyo Hudnanto ( 16.11.0048 )\n" +
                " - Arik Andrian Putra Purwajanu ( 16.11.00.55 )",
                "About", MessageBoxButton.OK, MessageBoxImage.Information);
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
