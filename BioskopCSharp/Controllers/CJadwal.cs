using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BioskopCSharp.SetupRDBMS;
using BioskopCSharp.Models;
using BioskopCSharp.Views.JadwalView;

namespace BioskopCSharp.Controllers
{
    class CJadwal
    {
        //Class
        private JadwalView _view;
        private JadwalAct _viewact;
        private Command _sql;

        private static CJadwal _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CJadwal()
        {
            _sql = new Command();
            _view = new JadwalView();
        }

        private MJadwal Entity(dynamic result)
        {
            var entity = new MJadwal()
            {
                IdJadwal = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                IdFilm = Convert.ToInt32(result[_column[1]]) as int? ?? 0,
                IdRuang = Convert.ToInt32(result[_column[2]]) as int? ?? 0,
                Waktu = result[_column[3]].ToString() as string,
            };
            return entity;
        }

        public static CJadwal GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CJadwal();
                }
                return _ctrl;
            }
        }
        public void Index()
        {
            _view.Show();
            _table = Read();
        }

        public void Index(string viewstr)

        {
            if (viewstr == "Action")
            {
                _viewact = new JadwalAct();
                //Detail(_table);
                _viewact.ShowDialog();
            }
        }

        public void Dispose()
        {
            if (_ctrl != null)
            {
                _ctrl = null;
            }
            if (_table != null)
            {
                _table = null;
            }
        }

        //Read : SELECT film.judul_film,waktu,ruang.nama_ruang FROM `jadwal` JOIN film ON jadwal.id_film = film.id_film JOIN ruang ON jadwal.id_ruang=ruang.id_ruang

        public DataTable Read()
        {
            List<MJadwal> list = null;

            _column = new[] { "id_jadwal", "id_ruang", "waktu", "id_film" };
            //_sql.Query = "SELECT film.judul_film,waktu,ruang.nama_ruang FROM `jadwal` JOIN film ON jadwal.id_film = film.id_film JOIN ruang ON jadwal.id_ruang=ruang.id_ruang";
            _sql.Query = "SELECT * FROM jadwal";
            list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID JADWAL", "NO" , "ID RUANG", "WAKTU", "ID FILM"};
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
                        row[1] = i;
                        row[2] = value.IdRuang as int? ?? 0;
                        row[3] = value.Waktu as string;
                        row[4] = value.IdFilm as int? ?? 0;
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
                _view.TblDataJadwal.ItemsSource = table.DefaultView;
                _view.TblDataJadwal.AutoGenerateColumns = true;
                _view.TblDataJadwal.CanUserAddRows = false;
                Console.WriteLine(table.DefaultView.Count);
            }
            return table;
        }
    }
}
