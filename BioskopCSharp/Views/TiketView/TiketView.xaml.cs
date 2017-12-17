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
        }

        private void FrmTiket_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CTiket.GetInstance;
            BtnPrint.IsEnabled = BtnLaporan.IsEnabled = false;
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

        }

        private void TblDataTiket_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(TblDataTiket.Items.Count > 0)
            {
                BtnPrint.IsEnabled = BtnLaporan.IsEnabled = true;
            }
        }

        private void TblDataTiket_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnLaporan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TblDataTiket_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
