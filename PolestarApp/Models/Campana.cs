using System.Windows.Media;

namespace PolestarApp.Models
{
    public enum DestinoCampana { Ofertas, Coche, PruebaConduccion }

    /// <summary>Campaña publicitaria del carrusel de la pantalla de inicio.</summary>
    public class Campana
    {
        private Brush? fondo;

        public string Etiqueta { get; init; } = "";
        public string Titulo { get; init; } = "";
        public string Subtitulo { get; init; } = "";
        public string TextoBoton { get; init; } = "";
        public DestinoCampana Destino { get; init; }
        public int? CocheId { get; init; }
        public string Color1 { get; init; } = "#222222";
        public string Color2 { get; init; } = "#555555";

        public Brush Fondo => fondo ??= Coche.Degradado(Color1, Color2);
    }
}
