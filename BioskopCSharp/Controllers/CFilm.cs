using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using BioskopCSharp.Models;
using BioskopCSharp.Views.FilmView;
using BioskopCSharp.SetupDBS;

namespace BioskopCSharp.Controllers
{
    class CFilm
    {
        //Class
        private FilmView _view;
        private FilmAct _viewact;
        private Command_SQLite _sql;

        private static CFilm _ctrl;

        //Variable
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CFilm()
        {
            _sql = new Command_SQLite();
            _view = new FilmView();
        }

        public static CFilm GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CFilm();
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
            _view.Show();
            _table = Read();
        }

        public void Index(string viewstr)

        {
            if (viewstr == "Action")
            {
                _viewact = new FilmAct();
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
            List<MFilm> list = null;

            _column = new[] { "id_film", "judul_film", "harga_film"};
            _sql.Query = "SELECT * FROM film";
            list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID", "NO", "JUDUL", "HARGA"};
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
                _view.TblDataFilm.ItemsSource = table.DefaultView;
                _view.TblDataFilm.AutoGenerateColumns = true;
                _view.TblDataFilm.CanUserAddRows = false;
            }
            return table;
        }

        public void Detail(DataTable table)
        {
            if (Code != string.Empty)
            {
                table = table.Select("id LIKE '%" + Code + "%'").CopyToDataTable();
                if (table.Rows.Count == 1)
                {
                    foreach (DataRow tbl in table.Rows)
                    {
                        _viewact.TxtJudul.Text = tbl[2].ToString();
                        _viewact.TxtHarga.Text = tbl[3].ToString();
                    }
                }
            }
        }

        public void Create(MFilm data)
        {
            if (IsValidate())
            {
                var isflaged = false;

                _sql.Query = string.Format("INSERT INTO film (`judul_film`,`harga_film`) VALUES ('{0}', '{1}')", data.Judul, data.Harga);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    _table = Read();
                    _viewact.Close();
                }
                else
                {
                    MessageBox.Show("Proses simpan gagal!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void Update(MFilm data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                _sql.Query = string.Format("UPDATE film SET judul_film = '{0}', harga_film = '{1}' WHERE id_film = '{2}'", data.Judul, data.Harga, Code);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    GetInstance.Index();
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
                    _sql.Query = string.Format("DELETE FROM film WHERE id_film = '{0}'", Code);
                    isflaged = _sql.ExecuteUpdate();

                    if (isflaged)
                    {
                        _table = Read();
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
            if (_viewact.TxtJudul.Text == string.Empty)
            {
                MessageBox.Show("Judul masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtJudul.Focus();
            }
            else if (_viewact.TxtHarga.Text == string.Empty)
            {
                MessageBox.Show("Harga masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtHarga.Focus();
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}
