﻿using System;
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
        private DateTime today = DateTime.Today;

        //Contructors
        public MainView()
        {
            InitializeComponent();
            
        }

        //Event On Window
        private void FrmMain_Loaded(object sender, RoutedEventArgs e)
        {
            UserAktiv.Content = App.UserLog;
            Tgl.Content = today.ToString("yyyy-MM-dd");
            _ctrl = CMain.GetInstance;
            CboMainDataWaktu.IsEnabled = false;
            _ctrl.DisableKursi();

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
                _ctrl.CodeJadwal = ((DataRowView)TblMainDataFilm.SelectedItems[0])[0].ToString();
                _ctrl.CodeFilm = ((DataRowView)TblMainDataFilm.SelectedItems[0])[1].ToString();
                _ctrl.GetWaktu();
                CboMainDataWaktu.IsEnabled = true;
            }
        }

        //Kasir Punya
        private void TblMainDataKasir_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void TblMainDataKasir_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CboMainDataWaktu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CboMainDataWaktu.SelectedIndex == 0)
            {
                //DO: Nothing
            }
            else
            {
                _ctrl.GetKursi();
            }
        }


        //Click
        //Pilih Film
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        //Kasir Punya
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnBayar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {

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
