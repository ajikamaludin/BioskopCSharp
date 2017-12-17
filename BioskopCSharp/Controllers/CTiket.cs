using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BioskopCSharp.SetupDBS;
using BioskopCSharp.Models;
using BioskopCSharp.Views.TiketView;

namespace BioskopCSharp.Controllers
{
    class CTiket
    {
        //Class
        private TiketView _view;
        private Command_SQLite _sql;

        private static CTiket _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CTiket()
        {
            _sql = new Command_SQLite();
            _view = new TiketView();
        }

        private MTiket Entity(dynamic result)
        {
            var entity = new MTiket()
            {
                IdTiket = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Kursi = result[_column[1]].ToString(),
                TglTiket = result[_column[2]].ToString(),
            };

            entity.Jadwal.Waktu = result[_column[6]].ToString();
            entity.Jadwal.Film.Judul = result[_column[3]].ToString();
            entity.Jadwal.Film.Harga = Convert.ToInt32(result[_column[4]]) as int? ?? 0;
            entity.Jadwal.Ruang.Nama = result[_column[5]].ToString();

            return entity;
        }

        public static CTiket GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CTiket();
                }
                return _ctrl;
            }
        }
        public void Index()
        {
            _table = Read();
            _view.Show();
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

        public DataTable Read()
        {
            List<MTiket> list = null;

            string SQL = "SELECT tiket.id_tiket, tiket.kursi, tiket.tgl_tiket, " +
                "jadwal.waktu , " +
                "film.judul_film,film.harga_film, " +
                "ruang.nama_ruang " +
                "FROM tiket " +
                "JOIN jadwal ON tiket.id_jadwal = jadwal.id_jadwal " +
                "JOIN film ON jadwal.id_film = film.id_film " +
                "JOIN ruang ON jadwal.id_ruang = ruang.id_ruang";

            _column = new[] { "id_tiket", "kursi", "tgl_tiket", "judul_film","harga_film", "nama_ruang", "waktu" };
            _sql.Query = SQL;
            list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID", "NO" , "NO KURSI", "TANGGAL TIKET","JUDUL FILM","HARGA","NAMA RUANG", "WAKTU" };
            int i = 1;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.IdTiket as int? ?? 0;
                        row[1] = i;
                        row[2] = value.Kursi as string;
                        row[3] = value.TglTiket as string;
                        row[4] = value.Jadwal.Film.Judul as string;
                        row[5] = value.Jadwal.Film.Harga as int? ?? 0;
                        row[6] = value.Jadwal.Ruang.Nama as string;
                        row[7] = value.Jadwal.Waktu as string;
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
                _view.TblDataTiket.ItemsSource = table.DefaultView;
                _view.TblDataTiket.AutoGenerateColumns = true;
                _view.TblDataTiket.CanUserAddRows = false;
            }
            return table;
        }
    }
}
