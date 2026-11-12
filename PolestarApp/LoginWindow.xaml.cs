using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PolestarApp.Services;

namespace PolestarApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                ActualizarVista();
                Animaciones.Entrada(Contenido);   // aparición suave del contenido
                if (!Sesion.Iniciada) this.txtUsuario.Focus();
            };
        }

        // ================= CONTROLAR LOGIN =================
        // Esquema de la asignatura: campos vacíos -> aviso; credenciales incorrectas -> error;
        // correctas -> confirmación. Cada mensaje lleva título e icono según su gravedad.
        private const string TituloMensajes = "Polestar · Acceso clientes";

        private void BtnEntrar_Click(object sender, RoutedEventArgs e)
        {
            string User = this.txtUsuario.Text;
            string Password = this.txtPassword.Password;

            if (User == "" || Password == "")
            {
                MessageBox.Show("Inserte el nombre del usuario o el Password",
                                TituloMensajes, MessageBoxButton.OK, MessageBoxImage.Warning);
                // El foco va al primer campo que falta por rellenar
                if (User == "") this.txtUsuario.Focus();
                else this.txtPassword.Focus();
                return;
            }
            else
            {
                if (User != "Alumno" || Password != "1234")
                {
                    MessageBox.Show("El nombre de usuario o el password es incorrecto",
                                    TituloMensajes, MessageBoxButton.OK, MessageBoxImage.Error);
                    // Se conserva el usuario y se vacía la contraseña para volver a escribirla
                    this.txtPassword.Clear();
                    this.txtPassword.Focus();
                    return;
                }
                else
                {
                    MessageBox.Show("Usuario correcto",
                                    TituloMensajes, MessageBoxButton.OK, MessageBoxImage.Information);
                    Sesion.Iniciar(User);
                    ActualizarVista();
                    return;
                }
            }
        }

        // ================= VISTA =================
        /// <summary>Muestra el formulario o la confirmación según haya sesión. Público para que la ventana lo refresque.</summary>
        public void ActualizarVista()
        {
            bool iniciada = Sesion.Iniciada;
            PanelFormulario.Visibility = iniciada ? Visibility.Collapsed : Visibility.Visible;
            PanelSesionIniciada.Visibility = iniciada ? Visibility.Visible : Visibility.Collapsed;
            Title = iniciada ? "Polestar · Mi cuenta" : "Polestar · Acceso clientes";

            if (iniciada)
            {
                TxtBienvenida.Text = $"Hola, {Sesion.Usuario}";
                Animaciones.Entrada(PanelSesionIniciada);
            }
        }

        // Aviso de Bloq Mayús mientras se escribe la contraseña (prevención de errores)
        private void Password_ComprobarMayus(object sender, RoutedEventArgs e) =>
            AvisoMayus.Visibility = Keyboard.IsKeyToggled(Key.CapsLock) ? Visibility.Visible : Visibility.Collapsed;

        private void Password_PerderFoco(object sender, KeyboardFocusChangedEventArgs e) =>
            AvisoMayus.Visibility = Visibility.Collapsed;

        private void BtnIrGaleria_Click(object sender, RoutedEventArgs e)
        {
            Window galeria = new GaleriaWindow();
            Navegacion.MismaPosicion(this, galeria);
            galeria.Show();
            this.Close();
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Sesion.Cerrar();
            this.txtUsuario.Clear();
            this.txtPassword.Clear();
            ActualizarVista();
            this.txtUsuario.Focus();
        }
    }
}
