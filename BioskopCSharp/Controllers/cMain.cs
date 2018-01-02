using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using BioskopCSharp.Views;
using BioskopCSharp.Views.TiketView;
using BioskopCSharp.Models;
using BioskopCSharp.SetupDBS;
using Microsoft.Reporting.WinForms;

namespace BioskopCSharp.Controllers
{
    
    class CMain
    {
        //Class
        private MainView _view;
        private SingleTiketView _viewPrintTiket;
        private Command_SQLite _sql;

        private static CMain _ctrl;

        //Variable
        private string[] _column;
        private int THarga;
        public string CodeFilm { get; set; }
        public string CodeJadwal { get; set; }
        public string CodeTiket { get; set; }

        private DataTable _tableJadwalFilm;
        private List<MKursi> ListKursi; //Ini List Model Tiket Digunakan untuk menampilkan kursi
        private List<MTiket> ListTiket;

        public CMain()
        {
            _sql = new Command_SQLite();
            _view = new MainView();
            ListKursi = new List<MKursi>();
            #region List Kursi Data
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA1, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA2, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA3, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA4, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA5, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB1, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB2, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB3, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB4, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB5, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC1, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC2, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC3, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC4, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC5, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD1, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD2, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD3, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD4, Database = false });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD5, Database = false });
            #endregion
        }

        public static CMain GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CMain();
                }
                return _ctrl;
            }
        }

        private MJadwal EntityFilm(dynamic result)
        {
            var entity = new MJadwal()
            {
                IdJadwal = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
            };
            entity.Film.Id = Convert.ToInt32(result[_column[1]]) as int? ?? 0;
            entity.Film.Judul = result[_column[2]].ToString() as string;
            entity.Film.Harga = Convert.ToInt32(result[_column[3]]) as int? ?? 0;
            return entity;
        }

        private MTiket EntityTiket(dynamic result)
        {
            var entity = new MTiket()
            {
                IdTiket = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Kursi = Convert.ToInt32(result[_column[3]]) as int? ?? 0,
                TglTiket = result[_column[6]].ToString() as string,
            };

            entity.Jadwal.Film.Judul = result[_column[1]].ToString() as string;
            entity.Jadwal.Ruang.Nama = result[_column[2]].ToString() as string;
            entity.Jadwal.Waktu = result[_column[4]].ToString() as string;
            entity.Jadwal.Film.Harga = Convert.ToInt32(result[_column[5]]) as int? ?? 0;

            return entity;
        }

        //call this window
        public void Index()
        {
            _view.UserAktiv.Content = App.UserLog;
            _tableJadwalFilm = ReadFilm();
            _view.Show();
        }

        //call print window
        public void Index(string act)
        {
            //Dispose();
            _viewPrintTiket = new SingleTiketView();
            if (act == "PrintTiket")
            {
                _viewPrintTiket.Show();
            }else if(act == "ReadFilm")
            {
                _tableJadwalFilm = ReadFilm();
            }
        }

        //null class
        public void Dispose()
        {
            if(_ctrl != null)
            {
                _ctrl = null;
            }
        }

        //menampilkan film
        private DataTable ReadFilm()
        {
            List<MJadwal> list = null;

            _column = new[] { "id_jadwal", "id_film", "judul_film", "harga_film" };
            _sql.Query = "SELECT jadwal.id_jadwal, jadwal.id_film, film.judul_film, film.harga_film FROM jadwal JOIN film ON jadwal.id_film = film.id_film GROUP BY film.judul_film";
            list = _sql.ExecuteQuery(EntityFilm);

            var table = new DataTable();
            var header = new string[] { "IDJADWAL", "IDFILM", "NO", "JUDUL", "HARGA" };
            int i = 1;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.IdJadwal as int? ?? 0;
                        row[1] = value.Film.Id as int? ?? 0;
                        row[2] = i;
                        row[3] = value.Film.Judul as string;
                        string Harga = "Rp. ";
                        Harga += value.Film.Harga as int? ?? 0;
                        row[4] = Harga;
                        table.Rows.Add(row);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _view.TblMainDataFilm.ItemsSource = table.DefaultView;
                _view.TblMainDataFilm.AutoGenerateColumns = true;
                _view.TblMainDataFilm.CanUserAddRows = false;
            }
            return table;
        }

        //menampilkan waktu dari film yang dipilih
        public void GetWaktu()
        {
            if(CodeFilm != string.Empty)
            {
                //Fill ComboBox Ruang
                _sql.Query = "SELECT id_jadwal,waktu FROM jadwal WHERE id_film ='" + CodeFilm + "'";
                _sql.FillCombo(_view.CboMainDataWaktu);
            }
        }

        //menampilkan kursi yang tersedia dan tidak tersedia
        private List<MKursi> ReadKursi()
        {

            string today = DateTime.Today.ToString("yyyy-MM-dd");

            _column = new[] { "id_tiket", "judul_film", "nama_ruang", "kursi", "waktu", "harga_film", "tgl_tiket" };
            _sql.Query = "SELECT tiket.id_tiket, film.judul_film, ruang.nama_ruang, tiket.kursi, jadwal.waktu, film.harga_film, tiket.tgl_tiket " +
                "FROM tiket " +
                "JOIN jadwal ON tiket.id_jadwal = jadwal.id_jadwal " +
                "JOIN film ON jadwal.id_film = film.id_film " +
                "JOIN ruang ON jadwal.id_ruang = ruang.id_ruang " +
                "WHERE tiket.id_jadwal = '" + CodeJadwal +"' AND tiket.tgl_tiket LIKE '" + today + "%'";
            ListTiket = _sql.ExecuteQuery(EntityTiket);
            
            int x = 0;
            foreach (var value in ListKursi.ToArray())
            {
                ListKursi[x].Kursi.IsEnabled = true;
                ListKursi[x].Kursi.IsChecked = false;
                x++;
            }
            try
            {
                if (ListTiket.Count > 0)
                {

                    foreach (var value in ListTiket.ToArray())
                    {
                        if (value.Kursi > 0)
                        {
                            ListKursi[value.Kursi - 1].Kursi.IsEnabled = false;
                            ListKursi[value.Kursi - 1].Kursi.IsChecked = true;
                            ListKursi[value.Kursi - 1].Database = true;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ListKursi;


        }

        //Read Tiket Lempar ke Datagrid Kasir
        private List<MTiket> ReadTiket()
        {

            _column = new[] { "id_tiket", "judul_film", "nama_ruang", "kursi", "waktu", "harga_film", "tgl_tiket" };
            _sql.Query = "SELECT tiket.id_tiket, film.judul_film, ruang.nama_ruang, tiket.kursi, jadwal.waktu, film.harga_film, tiket.tgl_tiket " +
                "FROM tiket " +
                "JOIN jadwal ON tiket.id_jadwal = jadwal.id_jadwal " +
                "JOIN film ON jadwal.id_film = film.id_film " +
                "JOIN ruang ON jadwal.id_ruang = ruang.id_ruang " +
                "WHERE status = '1'";
            ListTiket = _sql.ExecuteQuery(EntityTiket);

            var table = new DataTable();
            var header = new string[] { "IDTIKET", "NO","JUDUL", "RUANG", "KURSI", "JAM", "HARGA" };
            int i = 1;
            THarga = 0;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (ListTiket.Count > 0)
                {
                    foreach (var value in ListTiket.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.IdTiket as int? ?? 0;
                        row[1] = i;
                        row[2] = value.Jadwal.Film.Judul as string;
                        row[3] = value.Jadwal.Ruang.Nama as string;
                        row[4] = ConvertKursi(value.Kursi as int? ?? 0);
                        row[5] = value.Jadwal.Waktu as string;
                        string Harga = "Rp. ";
                        Harga += value.Jadwal.Film.Harga as int? ?? 0;
                        row[6] = Harga;
                        THarga += value.Jadwal.Film.Harga as int? ?? 0;
                        table.Rows.Add(row);
                        i++;
                    }
                    // Total Harga
                    _view.LbTotalHarga.Content = THarga;
                    _view.LbKembalian.Content = "0";
                }
                else if(ListTiket.Count == 0)
                {
                    _view.BtnDone.IsEnabled = _view.BtnPrint.IsEnabled = false;
                    _view.LbTotalHarga.Content = THarga;
                    _view.LbKembalian.Content = "0";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _view.TblMainDataKasir.ItemsSource = table.DefaultView;
                _view.TblMainDataKasir.AutoGenerateColumns = true;
                _view.TblMainDataKasir.CanUserAddRows = false;
            }

            return ListTiket;
        }

        //Konversi dari kursi ke angka
        private string ConvertKursi(int x)
        {
            string NamaKursi;
            #region Swicth Case Kursi
            switch (x)
            {
                case 1:
                    NamaKursi = "A1";
                    break;
                case 2:
                    NamaKursi = "A2";
                    break;
                case 3:
                    NamaKursi = "A3";
                    break;
                case 4:
                    NamaKursi = "A4";
                    break;
                case 5:
                    NamaKursi = "A5";
                    break;
                case 6:
                    NamaKursi = "B1";
                    break;
                case 7:
                    NamaKursi = "B2";
                    break;
                case 8:
                    NamaKursi = "B3";
                    break;
                case 9:
                    NamaKursi = "B4";
                    break;
                case 10:
                    NamaKursi = "B5";
                    break;
                case 11:
                    NamaKursi = "C1";
                    break;
                case 12:
                    NamaKursi = "C2";
                    break;
                case 13:
                    NamaKursi = "C3";
                    break;
                case 14:
                    NamaKursi = "C4";
                    break;
                case 15:
                    NamaKursi = "C5";
                    break;
                case 16:
                    NamaKursi = "D1";
                    break;
                case 17:
                    NamaKursi = "D2";
                    break;
                case 18:
                    NamaKursi = "D3";
                    break;
                case 19:
                    NamaKursi = "D4";
                    break;
                case 20:
                    NamaKursi = "D5";
                    break;
                default:
                    NamaKursi = "00";
                    break;
            }
            #endregion
            return NamaKursi;
        }

        //mengambil tiket
        public void GetTiket()
        {
            ListTiket = ReadTiket();
        }

        //Lempar kursi ke depan
        public void GetKursi()
        {
            ListKursi = ReadKursi();
        }

        //disable semua kursi
        public void DisableKursi()
        {
            foreach(var value in ListKursi)
            {
                value.Kursi.IsEnabled = false;
                value.Kursi.IsChecked = false;
            }
        }

        //Ambil tiket
        public void CreateTiket()
        {
            bool flag = true;
            bool isflaged = false;
            int i = 0;
            if(CodeFilm == null)
            {
                MessageBox.Show("Anda Belum Memilih Film", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (CodeJadwal == null)
            {
                MessageBox.Show("Anda Belum Memilih Waktu", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            foreach (var value in ListKursi.ToArray())
            {
                i++;
                if (value.Kursi.IsChecked == true)
                {
                    if (!value.Database)
                    {
                        string Today = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
                        _sql.Query = string.Format("INSERT INTO tiket (`id_jadwal`,`kursi`,`tgl_tiket`,`status`) VALUES ('{0}', '{1}','{2}' ,'1')", CodeJadwal, i, Today);
                        isflaged = _sql.ExecuteUpdate();
                    }
                    flag = false;
                }
            }
            if (isflaged)
            {
                SetClear();
                ListTiket = ReadTiket();
            }
            if (flag)
            {
                MessageBox.Show("Anda Belum Memilih Kursi", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        //Delete Tiket
        public void DeleteTiket()
        {
            var msg = MessageBox.Show("Yakin akan menghapus tiket ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                _sql.Query = string.Format("DELETE FROM tiket WHERE id_tiket = '{0}'", CodeTiket);
                bool isflaged = _sql.ExecuteUpdate();
                if (isflaged)
                {
                    ListTiket = ReadTiket();
                }
                else
                {
                    MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        //Clear All
        public void SetClear()
        {
            CodeFilm = null;
            CodeJadwal = null;
            CodeTiket = null;
            THarga = 0;
            DisableKursi();
            _view.TxtUangBayar.Text = " ";
            _view.CboMainDataWaktu.ItemsSource = null;
            _view.CboMainDataWaktu.Items.Clear();
            _view.CboMainDataWaktu.IsEnabled = false;
        }

        //Kembalian
        public bool GetKembalian()
        {
            int UangBayar;
            if (_view.TxtUangBayar.Text == string.Empty)
            {
                MessageBox.Show("Anda Belum Memasukan Uang Pembawayan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (_view.TxtUangBayar.Text == string.Empty)
            {
                UangBayar = 0;
            }
            else
            {
                UangBayar = Convert.ToInt32(_view.TxtUangBayar.Text) as int? ?? 0;
            }

            if (UangBayar == 0)
            {
                MessageBox.Show("Anda Belum Memasukan Uang Pembawayan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if(UangBayar < THarga)
            {
                MessageBox.Show("Uang Pembawayan Kurang", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            var msg = MessageBox.Show("Yakin akan melakukan pembayaran ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                _view.LbKembalian.Content = UangBayar - THarga;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        //Transaksi Selesai
        public bool DoneTrans()
        {
            var msg = MessageBox.Show("Yakin akan menyelesaikan transaksi ?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msg == MessageBoxResult.Yes)
            {
                _sql.Query = string.Format("UPDATE tiket SET status = ''");
                bool isflaged = _sql.ExecuteUpdate();
                if (isflaged)
                {
                    ListTiket = ReadTiket();
                    _view.BtnAdd.IsEnabled = _view.BtnRefresh.IsEnabled = true;
                    _view.BtnDone.IsEnabled = _view.BtnDone.IsEnabled = _view.BtnDelete.IsEnabled = _view.BtnPrint.IsEnabled = false;
                    SetClear();
                    return isflaged;
                }
                else
                {
                    MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Export(string datasetname, string filename, string srctable, ReportViewer rpt)
        {
            try
            {
                var data = new DataSet();
                
                _sql.Query = "SELECT tiket.id_tiket as IdTiket, " +
                    "film.judul_film as Judul, " +
                    "ruang.nama_ruang as Ruang, " +
                    "tiket.kursi as Kursi, " +
                    "jadwal.waktu as Waktu, " +
                    "film.harga_film as Harga, " +
                    "tiket.tgl_tiket as TglTiket " +
                    "FROM tiket JOIN jadwal ON tiket.id_jadwal = jadwal.id_jadwal " +
                    "JOIN film ON jadwal.id_film = film.id_film JOIN ruang ON jadwal.id_ruang = ruang.id_ruang WHERE tiket.id_tiket = '" + CodeTiket + "'";
                _sql.Report(srctable, out data);

                var datasource = new ReportDataSource(datasetname, data.Tables[srctable]);
                rpt.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory + filename;
                rpt.ProcessingMode = ProcessingMode.Local;

                rpt.LocalReport.DataSources.Clear();
                rpt.LocalReport.DataSources.Add(datasource);
                rpt.SetDisplayMode(DisplayMode.PrintLayout);
                rpt.RefreshReport();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Controller.FillDataReport at " + e.StackTrace);
            }
        }
    }
}
