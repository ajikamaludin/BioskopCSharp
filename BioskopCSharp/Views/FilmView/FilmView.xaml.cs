using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.FilmView
{
    /// <summary>
    /// Interaction logic for FilmView.xaml
    /// </summary>
    public partial class FilmView : Window
    {
        //Class Deklarasi
        private CFilm _ctrl;

        public FilmView()
        {
            InitializeComponent();
        }

        private void FrmFilm_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CFilm.GetInstance;
            BtnEdit.IsEnabled = BtnDelete.IsEnabled = false;
            TableDecor();
        }

        private void TblDataFilm_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void TblDataFilm_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (TblDataFilm.SelectedItems.Count > 0)
            {
                BtnEdit.IsEnabled = BtnDelete.IsEnabled = true;
                _ctrl.Code = ((DataRowView)TblDataFilm.SelectedItems[0])[0].ToString();
            }
        }

        private void TblDataFilm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _ctrl.Code = ((DataRowView)TblDataFilm.SelectedItems[0])[0].ToString();
            _ctrl.Index("Action");
        }

        private void TblDataFilm_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _ctrl.Code = ((DataRowView)TblDataFilm.SelectedItems[0])[0].ToString();
                _ctrl.Index("Action");
            }
        }

        private void FrmFilm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_ctrl != null)
            {
                _ctrl.Dispose();
            }
        }

        //Click
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Code = string.Empty;
            _ctrl.Index("Action");
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_ctrl.Code == string.Empty)
            {
                _ctrl.Code = ((DataRowView)TblDataFilm.SelectedItems[0])[0].ToString();
                _ctrl.Index("Action");
            }
            else if (_ctrl.Code != string.Empty)
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
                _ctrl.Code = ((DataRowView)TblDataFilm.SelectedItems[0])[0].ToString();
                _ctrl.Delete();
            }
            else if (_ctrl.Code != string.Empty)
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
            Close();
        }

        private void TableDecor()
        {
            if (TblDataFilm.Columns.Count > 0)
            {
                TblDataFilm.Columns[0].Width = DataGridLength.Auto;
                TblDataFilm.Columns[1].Width = DataGridLength.Auto;
                TblDataFilm.Columns[2].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            }
        }
    }
}
