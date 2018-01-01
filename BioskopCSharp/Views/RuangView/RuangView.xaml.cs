using System.Windows;
using System.Windows.Controls;
using System.Data;
using BioskopCSharp.Models;
using BioskopCSharp.Controllers;
using System.Windows.Input;

namespace BioskopCSharp.Views.RuangView
{
    /// <summary>
    /// Interaction logic for RuangView.xaml
    /// </summary>
    public partial class RuangView : Window
    {

        //Class
        private CRuang _ctrl;

        public RuangView()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private MRuang CreateModel
        {
            get
            {
                return new MRuang()
                {
                    Nama = TxtNama.Text,
                };
            }
        }

        //Window Event
        private void FrmRuang_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CRuang.GetInstance;
            BtnEdit.IsEnabled = BtnDelete.IsEnabled = TxtNama.IsEnabled = false;
            BtnSav.Visibility = BtnCancel.Visibility = Visibility.Collapsed;
            TableDecor();
        }

        private void TblDataRuang_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void TblDataRuang_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblDataRuang.SelectedItems.Count > 0)
            {
                BtnEdit.IsEnabled = BtnDelete.IsEnabled = true;
                _ctrl.Code = ((DataRowView)TblDataRuang.SelectedItems[0])[0].ToString();
            }
        }

        private void FrmRuang_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_ctrl != null)
            {
                _ctrl.Dispose();
            }
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Code = string.Empty;
            TxtNama.IsEnabled = true;
            BtnNew.Visibility = Visibility.Collapsed;
            BtnSav.Visibility = BtnCancel.Visibility = Visibility.Visible;
        }

        private void BtnSav_Click(object sender, RoutedEventArgs e)
        {

            if(_ctrl.Code != string.Empty)
            {
                _ctrl.Update(CreateModel);
            }
            else if(_ctrl.Code == string.Empty)
            {
                _ctrl.Create(CreateModel);
            }
            else
            {
                MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
            _ctrl.Code = string.Empty;
            TxtNama.IsEnabled = false;
            TxtNama.Text = string.Empty;
            BtnNew.Visibility = Visibility.Visible;
            BtnSav.Visibility = BtnCancel.Visibility = Visibility.Collapsed;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            TxtNama.IsEnabled = true;
            TxtNama.Text = ((DataRowView)TblDataRuang.SelectedItems[0])[2].ToString();
            TxtNama.Focus();
            BtnNew.Visibility = Visibility.Collapsed;
            BtnSav.Visibility = BtnCancel.Visibility = Visibility.Visible;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataRuang.SelectedItems[0])[0].ToString();
            }
            _ctrl.Delete();
            BtnDelete.IsEnabled = BtnEdit.IsEnabled = false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            TxtNama.IsEnabled = false;
            TxtNama.Text = string.Empty;
            BtnNew.Visibility = Visibility.Visible;
            BtnSav.Visibility = BtnCancel.Visibility = Visibility.Collapsed;
        }
        private void TableDecor()
        {
            if (TblDataRuang.Columns.Count > 0)
            {
                TblDataRuang.Columns[1].Width = DataGridLength.Auto;
                TblDataRuang.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            }
        }
    }
}
