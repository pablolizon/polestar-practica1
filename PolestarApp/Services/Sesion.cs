namespace PolestarApp.Services
{
    /// <summary>
    /// Sesión simulada del prototipo. Guarda qué usuario ha iniciado sesión para que
    /// la cabecera y la galería lo sepan. La comprobación de credenciales está en LoginWindow.
    /// </summary>
    public static class Sesion
    {
        public static string? Usuario { get; private set; }
        public static bool Iniciada => Usuario != null;

        /// <summary>Se lanza al iniciar o cerrar sesión para que la cabecera se actualice.</summary>
        public static event EventHandler? Cambiada;

        public static void Iniciar(string usuario)
        {
            Usuario = usuario;
            Cambiada?.Invoke(null, EventArgs.Empty);
        }

        public static void Cerrar()
        {
            Usuario = null;
            Cambiada?.Invoke(null, EventArgs.Empty);
        }
    }
}
