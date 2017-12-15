using System.Windows;
using System.Windows.Controls;
using System.Data;
using BioskopCSharp.Models;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.UserView
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        //Class Deklarasi
        private CUser _ctrl;

        private MUser CreateModel
        {
            get
            {
                return new MUser()
                {
                    nama = ((DataRowView)TblDataUser.SelectedItems[0])[1].ToString(),
                    username = ((DataRowView)TblDataUser.SelectedItems[0])[2].ToString(),
                    password = ((DataRowView)TblDataUser.SelectedItems[0])[3].ToString(),
                };
            }
        }

        public UserView()
        {
            InitializeComponent();
        }

        //Window Event
        private void FrmUser_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CUser.GetInstance;
            BtnEdit.IsEnabled = BtnDelete.IsEnabled = false;
            TableDecor();
        }

        private void TblDataUser_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "PASSWORD":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void TblDataUser_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblDataUser.SelectedItems.Count > 0)
            {
                BtnEdit.IsEnabled = BtnDelete.IsEnabled = true;
                _ctrl.Code = ((DataRowView)TblDataUser.SelectedItems[0])[0].ToString();
            }
        }

        private void FrmUser_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _ctrl.Dispose();
        }

        //Click
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Code = string.Empty;
            _ctrl.Index("Action");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataUser.SelectedItems[0])[0].ToString();
                _ctrl.Index("Action");
            }
            else if(_ctrl.Code != string.Empty)
            {
                _ctrl.Index("Action");
            }
            else
            {
                MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataUser.SelectedItems[0])[0].ToString();
                _ctrl.Delete();
            }
            else if(_ctrl.Code != string.Empty)
            {
                _ctrl.Delete();
            }
            else
            {
                MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Dispose();
            Close();
        }
        private void TableDecor()
        {
            if (TblDataUser.Columns.Count > 0)
            {
                TblDataUser.Columns[0].Width = DataGridLength.Auto;
                TblDataUser.Columns[1].Width = DataGridLength.Auto;
                TblDataUser.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                TblDataUser.Columns[3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                
            }
        }

        
    }
}
