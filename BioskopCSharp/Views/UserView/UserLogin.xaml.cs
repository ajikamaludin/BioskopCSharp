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

namespace BioskopCSharp.Views.UserView
{
    /// <summary>
    /// Interaction logic for UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        private CUser _ctrl;
        private CMain _mctrl;

        public UserLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(TxtNama.Text == string.Empty)
            {
                MessageBox.Show("Username masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxtNama.Focus();
            }
            else if(TxtPassword.Password == string.Empty)
            {
                MessageBox.Show("Password masih kosong!!!", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TxtPassword.Focus();
            }
            else
            {
                if (_ctrl.Register(TxtNama.Text, TxtPassword.Password, out App.UserLog))
                {
                    _mctrl.Index();
                    Close();
                }
            }
        }

        private void Btnkeluar_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            App.Current.Shutdown();
        }

        private void FrmUserLogin_Loaded(object sender, RoutedEventArgs e)
        {
            _ctrl = CUser.GetInstance;
            _mctrl = CMain.GetInstance;
        }
    }
}
