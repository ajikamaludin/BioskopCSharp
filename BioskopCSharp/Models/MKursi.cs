using System.Windows.Controls;

namespace BioskopCSharp.Models
{
    public class MKursi
    {
        public CheckBox Kursi { get; set; }
        public bool Database { get; set; }
        public MKursi()
        {
            Kursi = new CheckBox();
        }

    }
}
