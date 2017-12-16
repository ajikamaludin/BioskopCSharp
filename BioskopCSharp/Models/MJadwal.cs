namespace BioskopCSharp.Models
{
    public class MJadwal
    {

        public int IdJadwal { get; set; }
        public MRuang Ruang { get; set; }
        public string Waktu { get; set; }
        public MFilm Film { get; set; }

        public MJadwal() {
            Film = new MFilm();
            Ruang = new MRuang();
        }

    }
}
