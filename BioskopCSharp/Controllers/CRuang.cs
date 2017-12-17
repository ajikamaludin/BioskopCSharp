using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using BioskopCSharp.Models;
using BioskopCSharp.SetupDBS;
using BioskopCSharp.Views.RuangView;

namespace BioskopCSharp.Controllers
{
    class CRuang
    {
        //Class
        private RuangView _view;
        private Command_SQLite _sql;

        private static CRuang _ctrl;

        //Variable
        private string[] _column;

        private static DataTable _table;
        
        public string Code { get; set; }

        private CRuang()
        {
            _sql = new Command_SQLite();
            _view = new RuangView();
        }

        public static CRuang GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CRuang();
                }
                return _ctrl;
            }
        }

        private MRuang Entity(dynamic result)
        {
            var entity = new MRuang()
            {
                Id = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Nama = result[_column[1]].ToString() as string,
            };
            return entity;
        }


        public void Index()
        {
            _view.Show();
            _table = Read();
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
            List<MRuang> list = null;
            _column = new[] { "id_ruang", "nama_ruang"};
            _sql.Query = "SELECT * FROM ruang";
            list = _sql.ExecuteQuery(Entity);
            var i = 1;
            var table = new DataTable();
            var header = new string[] {"ID","NO","NAMA"};
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
                        row[2] = value.Nama as string;
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
                _view.TblDataRuang.ItemsSource = table.DefaultView;
                _view.TblDataRuang.AutoGenerateColumns = true;
                _view.TblDataRuang.CanUserAddRows = false;
            }
            return table;
        }

        public void Create(MRuang data)
        {
            if (IsValidate())
            {
                var isflaged = false;

                _sql.Query = string.Format("INSERT INTO ruang (`nama_ruang`) VALUES ('{0}')", data.Nama);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    _table = Read();
                }
                else
                {
                    MessageBox.Show("Proses simpan gagal!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void Update(MRuang data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                _sql.Query = string.Format("UPDATE ruang SET nama_ruang = '{0}' WHERE id_ruang = '{1}'", data.Nama, Code);
                isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    GetInstance.Index();
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
                    _sql.Query = string.Format("DELETE FROM ruang WHERE id_ruang = '{0}'", Code);
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
            if (_view.TxtNama.Text == string.Empty)
            {
                MessageBox.Show("Nama masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _view.TxtNama.Focus();
            }
            else
            {
                flag = true;
            }
            return flag;
        }
    }
}
