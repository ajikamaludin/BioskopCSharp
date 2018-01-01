using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using BioskopCSharp.Models;
using BioskopCSharp.Views.JadwalView;
using BioskopCSharp.SetupDBS;

namespace BioskopCSharp.Controllers
{
    class CJadwal
    {
        //Class
        private JadwalView _view;
        private JadwalAct _viewact;
        private Command_SQLite _sql;

        private static CJadwal _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CJadwal()
        {
            _sql = new Command_SQLite();
            _view = new JadwalView();
        }

        private MJadwal Entity(dynamic result)
        {
            var entity = new MJadwal()
            {
                IdJadwal = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Waktu = result[_column[1]].ToString() as string,
            };


            entity.Ruang.Id = Convert.ToInt32(result[_column[2]]) as int? ?? 0;
            entity.Ruang.Nama = result[_column[3]].ToString() as string;
            entity.Film.Id = Convert.ToInt32(result[_column[4]]) as int? ?? 0;
            entity.Film.Judul = result[_column[5]].ToString() as string;
            
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
                Detail(_table);
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

        public DataTable Read()
        {
            List<MJadwal> list = null;

            _column = new[] { "id_jadwal", "waktu", "id_ruang", "nama_ruang" , "id_film", "judul_film"};

            _sql.Query = "SELECT " +
                "jadwal.id_jadwal, jadwal.waktu, " +
                "ruang.id_ruang, ruang.nama_ruang, " +
                "film.id_film, film.judul_film " +
                "FROM `jadwal` " +
                "JOIN film ON jadwal.id_film = film.id_film " +
                "JOIN ruang ON jadwal.id_ruang=ruang.id_ruang";

            list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID", "NO", "WAKTU", "IDRUANG", "NAMA RUANG", "IDFILM", "JUDUL FILM"};
            int i = 1;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.IdJadwal as int? ?? 0; //hide
                        row[1] = i;
                        row[2] = value.Waktu as string;
                        row[3] = value.Ruang.Id as int? ?? 0; //hide
                        row[4] = value.Ruang.Nama as string;
                        row[5] = value.Film.Id as int? ?? 0; //hide
                        row[6] = value.Film.Judul as string;
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
            }
            return table;
        }

        public void Detail(DataTable table)
        {
            //Fill ComboBox Ruang
            _sql.Query = "SELECT * FROM ruang";
            _sql.FillCombo(_viewact.CboRuang);

            //Fill ComboBox Film
            _sql.Query = "SELECT * FROM film";
            _sql.FillCombo(_viewact.CboFilm);

            if (Code != string.Empty)
            {
                table = table.Select("id LIKE '%" + Code + "%'").CopyToDataTable();
                if (table.Rows.Count == 1)
                {
                    foreach (DataRow tbl in table.Rows)
                    {
                        _viewact.CboRuang.SelectedIndex = Convert.ToInt32(tbl[3].ToString());
                        _viewact.CboFilm.SelectedIndex = Convert.ToInt32(tbl[5].ToString());
                        _viewact.TxtWaktu.Text = tbl[2].ToString();
                    }
                }
            }
        }

        public void Create(MJadwal data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                _sql.Query = string.Format("INSERT INTO jadwal (`id_ruang`,`waktu`,`id_film`) VALUES ('{0}','{1}','{2}')", data.Ruang.Id, data.Waktu, data.Film.Id);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    _table = Read();
                    CMain.GetInstance.Index("ReadFilm");
                    _viewact.Close();
                }
                else
                {
                    MessageBox.Show("Proses simpan gagal!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void Update(MJadwal data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                _sql.Query = string.Format("UPDATE jadwal SET id_ruang = '{0}',waktu = '{1}',id_film = '{2}' WHERE id_jadwal = '{3}'", data.Ruang.Id, data.Waktu, data.Film.Id, Code);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    _table = Read();
                    CMain.GetInstance.Index("ReadFilm");
                    _viewact.Close();
                }
                else
                {
                    MessageBox.Show("Proses update gagal!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void Delete()
        {
            if (Code != string.Empty)
            {
                var msg = MessageBox.Show("Yakin akan dihapus?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (msg == MessageBoxResult.Yes)
                {
                    var isflaged = false;
                    _sql.Query = string.Format("DELETE FROM jadwal WHERE id_jadwal = '{0}'", Code);
                    isflaged = _sql.ExecuteUpdate();

                    if (isflaged)
                    {
                        _table = Read();
                        CMain.GetInstance.Index("ReadFilm");
                    }
                    else
                    {
                        MessageBox.Show("Proses hapus gagal!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
        }

        private bool IsValidate()
        {
            var flag = false;
            if (_viewact.TxtWaktu.Text == string.Empty)
            {
                MessageBox.Show("Waktu masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtWaktu.Focus();
            }
            else if (_viewact.CboFilm.SelectedIndex == 0) {
                MessageBox.Show("Film belum dipilih!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.CboFilm.Focus();
            }
            else if (_viewact.CboRuang.SelectedIndex == 0)
            {
                MessageBox.Show("Ruang belum dipilih!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.CboRuang.Focus();
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}
