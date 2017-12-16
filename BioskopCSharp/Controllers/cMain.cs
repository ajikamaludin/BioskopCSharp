using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioskopCSharp.Views;
using BioskopCSharp.SetupDBS;

namespace BioskopCSharp.Controllers
{
    class CMain
    {
        //Class
        private MainView _view;
        private Command_SQLite _sql;

        private static CMain _ctrl;
        public CMain()
        {
            _sql = new Command_SQLite();
            _view = new MainView();
        }

        public static CMain GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CMain();
                }
                return _ctrl;
            }
        }
        public void Index()
        {
            Dispose();
            _view.Show();
        }

        public void Dispose()
        {
            _ctrl = null;
        }


    }
}
