using System.Windows;

namespace BioskopCSharp.Views.CetakView
{
    /// <summary>
    /// Interaction logic for CetakPreview.xaml
    /// </summary>
    public partial class CetakPreview : Window
    {
        public CetakPreview()
        {
            InitializeComponent();
        }

        private void FrmPrintLaporanPreview_Loaded(object sender, RoutedEventArgs e)
        {
            BioskopCSharp.Controllers.CTiket.GetInstance.Export
                ("TiketAllDataSet", @"Print\Laporan.rdlc", "tiket", RptViewer);
        }
    }
}
