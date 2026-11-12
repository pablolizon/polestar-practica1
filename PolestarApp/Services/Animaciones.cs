using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PolestarApp.Services
{
    /// <summary>
    /// Animaciones lanzadas desde código. Todas son cortas (150-300 ms) y se desactivan
    /// si el usuario ha desactivado las animaciones en Windows
    /// (Configuración > Accesibilidad > Efectos visuales).
    /// </summary>
    public static class Animaciones
    {
        public static bool Activadas => SystemParameters.ClientAreaAnimation;

        private static readonly IEasingFunction Suavizado = new CubicEase { EasingMode = EasingMode.EaseOut };

        /// <summary>Aparición con fundido y un pequeño desplazamiento vertical.</summary>
        public static void Entrada(UIElement elemento, double desplazamiento = 8, int ms = 220)
        {
            if (!Activadas)
            {
                elemento.Opacity = 1;
                return;
            }

            var mover = new TranslateTransform(0, desplazamiento);
            elemento.RenderTransform = mover;

            var duracion = TimeSpan.FromMilliseconds(ms);
            elemento.BeginAnimation(UIElement.OpacityProperty,
                new DoubleAnimation(0, 1, duracion) { EasingFunction = Suavizado });
            mover.BeginAnimation(TranslateTransform.YProperty,
                new DoubleAnimation(desplazamiento, 0, duracion) { EasingFunction = Suavizado });
        }

        /// <summary>Lanza un Storyboard definido en XAML (editable desde Blend) respetando la preferencia del sistema.</summary>
        public static void Lanzar(FrameworkElement contenedor, string clave)
        {
            if (contenedor.TryFindResource(clave) is not Storyboard sb) return;
            sb.Begin(contenedor, true);
            if (!Activadas)
                sb.SkipToFill(contenedor);   // salta directamente al estado final
        }
    }
}
