using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.JadwalView
{
    /// <summary>
    /// Interaction logic for JadwalView.xaml
    /// </summary>
    public partial class JadwalView : Window
    {
        private CJadwal _ctrl;
        
        public JadwalView()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void TblDataJadwal_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "IDRUANG":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "IDFILM":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void FrmJadwal_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CJadwal.GetInstance;
            TableDecor();
            BtnEdit.IsEnabled = BtnDelete.IsEnabled = false;
        }

        private void TblDataJadwal_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblDataJadwal.SelectedItems.Count > 0)
            {
                BtnEdit.IsEnabled = BtnDelete.IsEnabled = true;
                _ctrl.Code = ((DataRowView)TblDataJadwal.SelectedItems[0])[0].ToString();
            }
        }

        private void TblDataJadwal_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _ctrl.Code = ((DataRowView)TblDataJadwal.SelectedItems[0])[0].ToString();
                _ctrl.Index("Action");
            }
        }

        private void TblDataJadwal_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TblDataJadwal.SelectedItems.Count > 0)
            {
                _ctrl.Code = ((DataRowView)TblDataJadwal.SelectedItems[0])[0].ToString();
                _ctrl.Index("Action");
            }
        }

        private void FrmJadwal_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if( _ctrl != null)
            {
                _ctrl.Dispose();
            }
            
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Code = string.Empty;
            _ctrl.Index("Action");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if(_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataJadwal.SelectedItems[0])[0].ToString();
            }
            _ctrl.Index("Action");
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataJadwal.SelectedItems[0])[0].ToString();
            }
            _ctrl.Delete();
            BtnDelete.IsEnabled = BtnEdit.IsEnabled = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TableDecor()
        {
            if (TblDataJadwal.Columns.Count > 0)
            {
                TblDataJadwal.Columns[0].Visibility = Visibility.Hidden;
                TblDataJadwal.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                TblDataJadwal.Columns[2].Width = DataGridLength.Auto;
                TblDataJadwal.Columns[3].Visibility = Visibility.Hidden;
                TblDataJadwal.Columns[4].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                TblDataJadwal.Columns[5].Visibility = Visibility.Hidden;
                TblDataJadwal.Columns[6].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            }
        }

    }
}
