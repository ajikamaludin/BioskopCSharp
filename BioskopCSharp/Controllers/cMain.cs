using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BioskopCSharp.Views;
using BioskopCSharp.SetupRDBMS;

namespace BioskopCSharp.Controllers
{
    class CMain
    {
        //Class
        private MainView _view;
        private Command _sql;

        private static CMain _ctrl;
        private bool locData;

        public CMain(bool mode)
        {
            if (mode == true)
            {
                _sql = new Command();
            }
            _view = new MainView();
        }

        public static CMain GetInstance
        {
            get
            {
                if (_ctrl == null)
                {
                    _ctrl = new CMain(App.LocData);
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
