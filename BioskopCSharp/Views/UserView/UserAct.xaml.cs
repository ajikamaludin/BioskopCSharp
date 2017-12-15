using System.Windows;
using BioskopCSharp.Models;
using BioskopCSharp.Controllers;

namespace BioskopCSharp.Views.UserView
{
    /// <summary>
    /// Interaction logic for UserAct.xaml
    /// </summary>
    public partial class UserAct : Window
    {
        
        public UserAct()
        {
            InitializeComponent();
        }

        private MUser CreateModel
        {
            get
            {
                return new MUser()
                {
                    nama = TxtNama.Text,
                    username = TxtUsername.Text,
                    password = TxtPassword.Password
                };
            }
        }

        private void BtnSimpan_Click(object sender, RoutedEventArgs e)
        {
            if(CUser.GetInstance.Code != string.Empty)
            {
                CUser.GetInstance.Update(CreateModel);
            }
            else
            {
                CUser.GetInstance.Create(CreateModel);
            }
            
        }

        private void BtnBatal_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
