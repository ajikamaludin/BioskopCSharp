using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BioskopCSharp.Controllers;

namespace BioskopCSharp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CUser _mCtrl;
        public static string UserLog = string.Empty;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _mCtrl = CUser.GetInstance;
            _mCtrl.Index("Register");
        }
    }
}
