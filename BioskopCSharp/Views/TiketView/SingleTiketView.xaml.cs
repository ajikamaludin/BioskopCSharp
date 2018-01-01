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
    /// Interaction logic for SingleTiketView.xaml
    /// </summary>
    public partial class SingleTiketView : Window
    {
        private CMain _ctrl;

        public SingleTiketView()
        {
            InitializeComponent();
        }

        private void FrmSingleTiket_Loaded(object sender, RoutedEventArgs e)
        {
            FillReport();
        }

        private void FillReport()
        {
            _ctrl = CMain.GetInstance;
            _ctrl.Export("TiketDataset", @"Print\Tiket.rdlc", "tiket", RptViewer);
        }
    }
}
