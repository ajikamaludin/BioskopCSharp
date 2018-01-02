
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.TiketView
{
    /// <summary>
    /// Interaction logic for TiketView.xaml
    /// </summary>
    public partial class TiketView : Window
    {

        private CTiket _ctrl;

        public TiketView()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void FrmTiket_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CTiket.GetInstance;
            BtnPrint.IsEnabled = false;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void FrmTiket_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_ctrl != null)
            {
                _ctrl.Dispose();
            }
        }

        private void TblDataTiket_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TblDataTiket.Items.Count > 0)
            {
                CMain.GetInstance.CodeTiket = ((DataRowView)TblDataTiket.SelectedItems[0])[0].ToString();
                CMain.GetInstance.Index("PrintTiket");
            }
        }

        private void TblDataTiket_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(TblDataTiket.Items.Count > 0)
            {
               CMain.GetInstance.CodeTiket = ((DataRowView)TblDataTiket.SelectedItems[0])[0].ToString();
                BtnPrint.IsEnabled = true;
            }
        }

        private void TblDataTiket_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (TblDataTiket.Items.Count > 0)
            {
                CMain.GetInstance.CodeTiket = ((DataRowView)TblDataTiket.SelectedItems[0])[0].ToString();
                CMain.GetInstance.Index("PrintTiket");
            }
            BtnPrint.IsEnabled = false;
        }

        private void BtnLaporan_Click(object sender, RoutedEventArgs e)
        {
            _ctrl.Index("Print");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TblDataTiket_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "ID":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }
        }
    }
}
