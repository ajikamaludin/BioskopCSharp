using System;
using System.Windows;
using BioskopCSharp.Controllers;
using BioskopCSharp.Models;

namespace BioskopCSharp.Views.JadwalView
{
    /// <summary>
    /// Interaction logic for JadwalAct.xaml
    /// </summary>
    public partial class JadwalAct : Window
    {
        
        private MJadwal CreateEntity
        {
            get
            {
                return new MJadwal()
                {
                    Ruang = {
                        Id = Convert.ToInt32(CboRuang.SelectedValue.ToString()),
                    },

                    Waktu = TxtWaktu.Text.ToString(),

                    Film = {
                        Id = Convert.ToInt32(CboFilm.SelectedValue.ToString()),
                    }
                };
            }
        }

        public JadwalAct()
        {
            InitializeComponent();
        }

        private void FrmJadwalEditor_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void FrmJadwalEditor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(CJadwal.GetInstance.Code != string.Empty)
            {
                CJadwal.GetInstance.Update(CreateEntity);
            }
            else if(CJadwal.GetInstance.Code == string.Empty)
            {
                CJadwal.GetInstance.Create(CreateEntity);
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
    }
}
