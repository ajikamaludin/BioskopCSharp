using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private DataTable _table;

        public CMain()
        {
            _sql = new Command_SQLite();
            _view = new MainView();
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

        private MFilm Entity(dynamic result)
        {
            var entity = new MFilm()
            {
                Id = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Judul = result[_column[1]].ToString() as string,
                Harga = Convert.ToInt32(result[_column[2]]) as int? ?? 0,
            };
            return entity;
        }


        public void Index()
        {
            _table = ReadFilm();
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
            List<MFilm> list = null;

            _column = new[] { "id_film", "judul_film", "harga_film" };
            _sql.Query = "SELECT jadwal.id_film, film.judul_film, film.harga_film FROM jadwal JOIN film ON jadwal.id_film = film.id_film GROUP BY film.judul_film";
            list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID", "NO", "JUDUL", "HARGA" };
            int i = 1;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.Id as int? ?? 0;
                        row[1] = i;
                        row[2] = value.Judul as string;
                        row[3] = value.Harga as int? ?? 0;
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
        private void ReadKursi(MJadwal data)
        {

        }
        //Lempar kursi ke depan
        public void GetKursi()
        {

        }

    }
}
