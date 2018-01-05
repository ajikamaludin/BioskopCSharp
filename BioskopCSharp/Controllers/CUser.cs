using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using BioskopCSharp.SetupDBS;
using BioskopCSharp.Models;
using BioskopCSharp.Views.UserView;

namespace BioskopCSharp.Controllers
{
    class CUser
    {
        //Class
        private UserView _view;
        private UserLogin _viewlog;
        private UserAct _viewact;
        private Command_SQLite _sql;

        private static CUser _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CUser()
        {
            _sql = new Command_SQLite();
            _view = new UserView();
        }

        public static CUser GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CUser();
                }
                return _ctrl;
            }
        }

        private MUser Entity(dynamic result)
        {
            var entity = new MUser()
            {
                Id = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                Nama = result[_column[1]].ToString() as string,
                Username = result[_column[2]].ToString() as string,
                Password = result[_column[3]].ToString() as string,
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
                _viewact = new UserAct();
                Detail(_table);
                _viewact.ShowDialog();
            }
            else if (viewstr == "Register")
            {
                _viewlog = new UserLogin();
                _table = Read();
                _viewlog.Show();
            }
        }

        public void Dispose()
        {
            if(_ctrl != null)
            {
                _ctrl = null;
            }
            if(_table != null)
            {
                _table = null;
            }
        }

        public bool Register(string user, string password, out string userlog)
        {
            var state = false;
            userlog = string.Empty;
            if (_viewlog.TxtNama.Text == string.Empty)
            {
                MessageBox.Show("User masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewlog.TxtNama.Focus();
            }
            else if (_viewlog.TxtPassword.Password == string.Empty)
            {
                MessageBox.Show("Password masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewlog.TxtPassword.Focus();
            }
            else
            {
                var data = new DataTable();
                try
                {
                    data = _table.Select("username LIKE '" + user + "'").CopyToDataTable();
                    Console.WriteLine(_table.DefaultView.Count);
                }
                catch { }
                if (data.Rows.Count == 1)
                {
                    foreach (DataRow tbl in data.Rows)
                    {
                        if (password.ToLower().Equals(tbl[3].ToString().ToLower()))
                        {
                            state = true;
                            userlog = tbl[2].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Password Salah!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (data.Rows.Count > 1)
                {
                    MessageBox.Show("User Terindikasi duplikat!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("User Tidak terdaftar!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return state;
        }

        public DataTable Read()
        {
            List<MUser> list = null;
  
           _column = new[] { "id_user", "nama_user", "username_user", "password_user" };
           _sql.Query = "SELECT * FROM user";
           list = _sql.ExecuteQuery(Entity);

            var table = new DataTable();
            var header = new string[] { "ID","NO","NAMA","USERNAME","PASSWORD" };
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
                        row[2] = value.Nama as string;
                        row[3] = value.Username as string;
                        row[4] = value.Password as string;
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
                _view.TblDataUser.ItemsSource = table.DefaultView;
                _view.TblDataUser.AutoGenerateColumns = true;
                _view.TblDataUser.CanUserAddRows = false;
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
                        _viewact.TxtNama.Text = tbl[2].ToString();
                        _viewact.TxtUsername.Text = tbl[3].ToString();
                        _viewact.TxtPassword.Password = tbl[4].ToString();
                    }
                }
            }
        }

        public void Create(MUser data)
        {
            if (IsValidate())
            {
                var isflaged = false;

                _sql.Query = string.Format("INSERT INTO user (`nama_user`,`username_user`,`password_user`) VALUES ('{0}', '{1}', '{2}')", data.Nama, data.Username , data.Password);
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

        public void Update(MUser data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                _sql.Query = string.Format("UPDATE user SET nama_user = '{0}', username_user = '{1}', password_user = '{2}' WHERE id_user = '{3}'", data.Nama, data.Username ,data.Password, Code);
                    isflaged = _sql.ExecuteUpdate();

                if (isflaged)
                {
                    //GetInstance.Index();
                    _table = Read();
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
                    _sql.Query = string.Format("DELETE FROM user WHERE id_user = '{0}'", Code);
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
            if (_viewact.TxtNama.Text == string.Empty)
            {
                MessageBox.Show("Nama masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtNama.Focus();
            }
            else if (_viewact.TxtUsername.Text == string.Empty)
            {
                MessageBox.Show("Username masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtUsername.Focus();
            }
            else if (_viewact.TxtPassword.Password == string.Empty)
            {
                MessageBox.Show("Password masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                _viewact.TxtPassword.Focus();
            }
            else
            {
                flag = true;
            }
            return flag;
        }

    }
}
