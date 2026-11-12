using System.Windows;
using System.Windows.Controls;
using PolestarApp.Services;

namespace PolestarApp.Controls
{
    public partial class Cabecera : UserControl
    {
        /// <summary>Sección de la ventana que contiene la cabecera: "Inicio", "Galeria" o "Acceso".</summary>
        public string Seccion { get; set; } = "";

        private Window VentanaActual => Window.GetWindow(this);

        public Cabecera()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                // Marca la sección activa en el menú (dispara la animación del subrayado)
                BtnNavInicio.Tag  = Seccion == "Inicio"  ? "Activo" : null;
                BtnNavGaleria.Tag = Seccion == "Galeria" ? "Activo" : null;
                BtnNavAcceso.Tag  = Seccion == "Acceso"  ? "Activo" : null;

                ActualizarSesion();
                Sesion.Cambiada += Sesion_Cambiada;
            };
            Unloaded += (_, _) => Sesion.Cambiada -= Sesion_Cambiada;
        }

        // ================= NAVEGACIÓN ENTRE VENTANAS =================
        // Cada opción del menú abre la ventana de su sección y cierra la actual.
        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            if (Seccion == "Inicio") return;

            Window inicio = new InicioWindow();
            Navegacion.MismaPosicion(VentanaActual, inicio);
            inicio.Show();
            VentanaActual.Close();
        }

        private void BtnGaleria_Click(object sender, RoutedEventArgs e)
        {
            if (Seccion == "Galeria") return;

            Window galeria = new GaleriaWindow();
            Navegacion.MismaPosicion(VentanaActual, galeria);
            galeria.Show();
            VentanaActual.Close();
        }

        private void BtnAcceso_Click(object sender, RoutedEventArgs e)
        {
            if (Seccion == "Acceso") return;

            Window login = new LoginWindow();
            Navegacion.MismaPosicion(VentanaActual, login);
            login.Show();
            VentanaActual.Close();
        }

        // ================= SESIÓN =================
        private void Sesion_Cambiada(object? sender, EventArgs e) => ActualizarSesion();

        private void ActualizarSesion()
        {
            if (Sesion.Iniciada)
            {
                TxtSesion.Text = $"Hola, {Sesion.Usuario}";
                BtnCerrarSesion.Visibility = Visibility.Visible;
                BtnNavAcceso.Content = "Mi c_uenta";
                BtnNavAcceso.ToolTip = "Datos de tu cuenta (Alt+U)";
            }
            else
            {
                TxtSesion.Text = "Invitado";
                BtnCerrarSesion.Visibility = Visibility.Collapsed;
                BtnNavAcceso.Content = "_Acceso clientes";
                BtnNavAcceso.ToolTip = "Iniciar sesión (Alt+A)";
            }
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Sesion.Cerrar();
            if (VentanaActual is LoginWindow login) login.ActualizarVista();
        }
    }
}
