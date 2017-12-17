using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using BioskopCSharp.Views;
using BioskopCSharp.Models;
using BioskopCSharp.SetupDBS;

namespace BioskopCSharp.Controllers
{
    
    class CMain
    {
        //Class
        private MainView _view;
        private Command_SQLite _sql;

        private static CMain _ctrl;

        //Variable
        private string[] _column;
        public string CodeFilm { get; set; }
        public string CodeJadwal { get; set; }

        private DataTable _tableJadwalFilm;
        private List<MKursi> ListKursi;

        public CMain()
        {
            _sql = new Command_SQLite();
            _view = new MainView();
            ListKursi = new List<MKursi>();
            #region List Kursi Data
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA1 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA2 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA3 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA4 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiA5 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB1 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB2 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB3 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB4 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiB5 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC1 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC2 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC3 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC4 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiC5 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD1 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD2 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD3 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD4 });
            ListKursi.Add(new MKursi() { Kursi = _view.KursiD5 });
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
                Kursi = Convert.ToInt32(result[_column[1]]) as int? ?? 0,
                TglTiket = result[_column[2]].ToString() as string,
            };
            return entity;
        }

        public void Index()
        {
            _tableJadwalFilm = ReadFilm();
            _view.Show();
        }

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
                        row[4] = value.Film.Harga as int? ?? 0;
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
            List<MTiket> list = null;
            string today = DateTime.Today.ToString("yyyy-MM-dd");

            _column = new[] { "id_tiket", "kursi", "tgl_tiket" };
            _sql.Query = "SELECT tiket.id_tiket, tiket.kursi, tiket.tgl_tiket FROM tiket WHERE id_jadwal = '" + CodeJadwal +"' AND tgl_tiket LIKE '%" + today + "%'";
            list = _sql.ExecuteQuery(EntityTiket);
            
            int x = 0;
            try
            {
                Console.WriteLine(list.Count);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        if (value.Kursi > 0)
                        {
                            ListKursi[value.Kursi + 1].Kursi.IsEnabled = false;
                            ListKursi[value.Kursi + 1].Kursi.IsChecked = true;
                            x++;
                        }
                        else
                        {
                            ListKursi[x + 1].Kursi.IsEnabled = true;
                            x++;
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
        //Lempar kursi ke depan
        public void GetKursi()
        {
            ListKursi = ReadKursi();
        }

        public void DisableKursi()
        {
            foreach(var value in ListKursi)
            {
                value.Kursi.IsEnabled = false;
            }
        }
        
    }
}
