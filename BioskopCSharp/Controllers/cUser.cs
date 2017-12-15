using System;
using System.Collections.Generic;
using System.Windows;
using System.Data;
using BioskopCSharp.SetupRDBMS;
using BioskopCSharp.Models;
using BioskopCSharp.Views.UserView;

namespace BioskopCSharp.Controllers
{
    public class CUser
    {
        //Class
        private UserView _view;
        private UserLogin _viewlog;
        private UserAct _viewact;
        private Command _sql;

        private static CUser _ctrl;

        //Variabel
        private string[] _column;

        private static DataTable _table;

        public string Code { get; set; }

        public CUser()
        {
            _sql = new Command();
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
                id = Convert.ToInt32(result[_column[0]]) as int? ?? 0,
                nama = result[_column[1]].ToString() as string,
                username = result[_column[2]].ToString() as string,
                password = result[_column[3]].ToString() as string,
            };
            return entity;
        }

        public void Index()
        {
            Dispose();
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
            _ctrl = null;
            _table = null;
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
            if (App.LocData)
            {
                _column = new[] { "id_user", "nama_user", "username_user", "password_user" };
                _sql.Query = "SELECT * FROM user";
                list = _sql.ExecuteQuery(Entity);
            }

            var table = new DataTable();
            var header = new string[] { "ID","NO","NAMA","USERNAME" };
            int i = 1;
            try
            {
                foreach (var value in header) table.Columns.Add(value);
                if (list.Count > 0)
                {
                    foreach (var value in list.ToArray())
                    {
                        var row = table.NewRow();
                        row[0] = value.id as int? ?? 0;
                        row[1] = i;
                        row[2] = value.nama as string;
                        row[3] = value.username as string;
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
                        _viewact.TxtNama.Text = tbl[1].ToString();
                        _viewact.TxtPassword.Password = tbl[2].ToString();
                    }
                }
            }
        }

        public void Create(MUser data)
        {
            if (IsValidate())
            {
                var isflaged = false;
                if (App.LocData)
                {
                    _sql.Query = string.Format("INSERT INTO user VALUES ('{0}', '{1}', '{2}')", data.nama, data.username , data.password);
                    isflaged = _sql.ExecuteUpdate();
                }

                if (isflaged)
                {
                    GetInstance.Index();
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
                if (App.LocData)
                {
                    _sql.Query = string.Format("UPDATE user SET nama_user = '{0}', username_user = '{1}', password_user = '{2}' WHERE id_user = '{3}'", data.nama, data.username ,data.password, Code);
                    isflaged = _sql.ExecuteUpdate();
                }

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

        public void Delete(MUser data)
        {
            if (IsValidate())
            {
                var msg = MessageBox.Show("Yakin akan dihapus?", "Pertanyaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (msg == MessageBoxResult.Yes)
                {
                    var isflaged = false;
                    if (App.LocData)
                    {
                        _sql.Query = string.Format("DELETE FROM user WHERE id_user = '{0}'", Code);
                        isflaged = _sql.ExecuteUpdate();
                    }

                    if (isflaged)
                    {
                        GetInstance.Index();
                        _viewact.Close();
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
