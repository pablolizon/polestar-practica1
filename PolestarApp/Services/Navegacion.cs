using System.Windows;

namespace PolestarApp.Services
{
    /// <summary>
    /// Ayuda para cambiar de ventana. Antes de hacer .Show() la ventana nueva se coloca
    /// en la misma posición y con el mismo tamaño que la actual, para que el usuario
    /// perciba un cambio de sección y no una ventana que salta de sitio.
    /// </summary>
    public static class Navegacion
    {
        public static void MismaPosicion(Window actual, Window nueva)
        {
            Rect r = actual.WindowState == WindowState.Normal
                ? new Rect(actual.Left, actual.Top, actual.ActualWidth, actual.ActualHeight)
                : actual.RestoreBounds;

            nueva.WindowStartupLocation = WindowStartupLocation.Manual;
            if (!r.IsEmpty)
            {
                nueva.Left = r.Left;
                nueva.Top = r.Top;
                nueva.Width = r.Width;
                nueva.Height = r.Height;
            }
            if (actual.WindowState == WindowState.Maximized)
                nueva.WindowState = WindowState.Maximized;
        }
    }
}
