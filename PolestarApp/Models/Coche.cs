using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PolestarApp.Models
{
    /// <summary>
    /// Producto del catálogo. Prototipo académico: precios, autonomías y textos son orientativos y ficticios.
    /// Las fotos son de Wikimedia Commons (autores y licencias en la documentación).
    /// </summary>
    public class Coche
    {
        private static readonly CultureInfo Es = new("es-ES");
        private static readonly Dictionary<string, ImageSource> Fotos = new();
        private Brush? fondo;

        public int Id { get; init; }
        public string Modelo { get; init; } = "";
        public string Version { get; init; } = "";
        public string Tipo { get; init; } = "";
        public int Anio { get; init; }
        public decimal Precio { get; init; }
        public decimal? PrecioOferta { get; init; }
        public int AutonomiaKm { get; init; }
        public bool EsNovedad { get; init; }
        public string Descripcion { get; init; } = "";
        public string Color1 { get; init; } = "#333333";
        public string Color2 { get; init; } = "#777777";

        public string Nombre => $"{Modelo} {Version}";
        public bool TieneOferta => PrecioOferta.HasValue;
        public decimal PrecioFinal => PrecioOferta ?? Precio;

        public string PrecioTexto => PrecioFinal.ToString("C0", Es);
        public string PrecioOriginalTexto => Precio.ToString("C0", Es);
        public string AhorroTexto => TieneOferta ? $"Ahorras {(Precio - PrecioFinal).ToString("C0", Es)}" : "";
        public string TipoYAnio => $"{Tipo} · {Anio}";
        public string AutonomiaTexto => $"≈ {AutonomiaKm} km";

        /// <summary>Texto que leen los lectores de pantalla al enfocar la tarjeta.</summary>
        public string NombreAccesible =>
            $"{Nombre}, {Tipo}, unos {AutonomiaKm} kilómetros de autonomía, {PrecioTexto}" +
            (TieneOferta ? ", en oferta" : "") + (EsNovedad ? ", novedad" : "");

        /// <summary>Color de fondo que se ve mientras carga la foto.</summary>
        public Brush Fondo => fondo ??= Degradado(Color1, Color2);

        /// <summary>
        /// Foto del modelo (Images/Coches/polestarN.jpg). Las versiones de un mismo modelo comparten foto,
        /// que se carga una sola vez y se reutiliza.
        /// </summary>
        public ImageSource Foto
        {
            get
            {
                string clave = Modelo.Replace(" ", "").ToLowerInvariant();   // "Polestar 4" -> "polestar4"
                if (!Fotos.TryGetValue(clave, out ImageSource? foto))
                {
                    var bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource = new Uri($"pack://application:,,,/Images/Coches/{clave}.jpg");
                    bmp.DecodePixelWidth = 720;
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    bmp.Freeze();
                    foto = bmp;
                    Fotos[clave] = foto;
                }
                return foto;
            }
        }

        internal static Brush Degradado(string c1, string c2)
        {
            var b = new LinearGradientBrush(
                (Color)ColorConverter.ConvertFromString(c1),
                (Color)ColorConverter.ConvertFromString(c2), 20);
            b.Freeze();
            return b;
        }
    }
}
