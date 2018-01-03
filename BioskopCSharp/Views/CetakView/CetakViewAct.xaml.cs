using System.Windows;
using System.Windows.Input;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.CetakView
{
    /// <summary>
    /// Interaction logic for CetakViewAct.xaml
    /// </summary>
    public partial class CetakViewAct : Window
    {
        public CetakViewAct()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void BtnCetak_Click(object sender, RoutedEventArgs e)
        {
            if (DateStart.SelectedDate == null)
            {
                MessageBox.Show("Anda Belum Memilih Tanggal Awal", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if(DateEnd.SelectedDate == null)
            {
                MessageBox.Show("Anda Belum Memilih Tanggal Akhir", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                string dateStart = DateStart.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
                string dateEnd = DateEnd.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
                if (dateEnd.Length < 10 || dateStart.Length < 10)
                {
                    MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    CTiket.GetInstance.DateStart = dateStart;
                    CTiket.GetInstance.DateEnd = dateEnd;
                    CTiket.GetInstance.Index("Preview");
                }
            }
        }
    }
}
