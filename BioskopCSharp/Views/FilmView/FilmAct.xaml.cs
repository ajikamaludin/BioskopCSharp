using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BioskopCSharp.Models;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.FilmView
{
    /// <summary>
    /// Interaction logic for FilmAct.xaml
    /// </summary>
    public partial class FilmAct : Window
    {
        public FilmAct()
        {
            InitializeComponent();
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private MFilm CreateModel
        {
            get
            {
                return new MFilm()
                {
                    Judul = TxtJudul.Text,
                    Harga = Convert.ToInt32(TxtHarga.Text) as int? ?? 0,
                };
            }
        }

        private void FrmFilm_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void TxtHarga_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void FrmFilm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            if(CFilm.GetInstance.Code != string.Empty)
            {
                CFilm.GetInstance.Update(CreateModel);
            }
            else if(CFilm.GetInstance.Code == string.Empty)
            {
                CFilm.GetInstance.Create(CreateModel);
            }
            else
            {
                MessageBox.Show("System Mengalami Kesalahan", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void BtnBatal_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
