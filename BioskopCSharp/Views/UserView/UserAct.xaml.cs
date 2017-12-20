using System.Windows;
using BioskopCSharp.Models;
using BioskopCSharp.Controllers;
using System.Windows.Input;

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
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
        }

        private MUser CreateModel
        {
            get
            {
                return new MUser()
                {
                    Nama = TxtNama.Text,
                    Username = TxtUsername.Text,
                    Password = TxtPassword.Password
                };
            }
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
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
