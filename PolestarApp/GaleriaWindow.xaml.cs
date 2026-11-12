using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PolestarApp.Models;
using PolestarApp.Services;

namespace PolestarApp
{
    public partial class GaleriaWindow : Window
    {
        private static readonly string[] OpcionesOrden =
        {
            "Novedades primero",
            "Precio: de menor a mayor",
            "Precio: de mayor a menor",
            "Mayor autonomía",
        };

        private bool cargado;
        private Button? tarjetaOrigen;   // para devolver el foco al cerrar la ficha

        public GaleriaWindow() : this(false, null) { }

        /// <param name="soloOfertas">Abre la galería filtrada por ofertas (desde la campaña de invierno).</param>
        /// <param name="cocheId">Abre directamente la ficha de un modelo (desde Inicio).</param>
        public GaleriaWindow(bool soloOfertas = false, int? cocheId = null)
        {
            InitializeComponent();

            CmbTipo.ItemsSource = new[] { "Todos" }.Concat(Catalogo.Tipos).ToList();
            CmbTipo.SelectedIndex = 0;
            CmbOrden.ItemsSource = OpcionesOrden;
            CmbOrden.SelectedIndex = 0;
            ChkSoloOfertas.IsChecked = soloOfertas;

            cargado = true;
            AplicarFiltros();

            Loaded += (_, _) =>
            {
                Animaciones.Entrada(Contenido);   // aparición suave del contenido
                if (cocheId is int id && Catalogo.PorId(id) is Coche coche)
                    AbrirDetalle(coche);
            };
        }

        // ================= FILTROS =================
        private void Filtros_Cambiados(object sender, RoutedEventArgs e)
        {
            if (cargado) AplicarFiltros();
        }

        private void AplicarFiltros()
        {
            IEnumerable<Coche> consulta = Catalogo.Todos;

            string texto = TxtBuscar.Text.Trim();
            if (texto.Length > 0)
                consulta = consulta.Where(c => Contiene(c.Nombre, texto) || Contiene(c.Tipo, texto));

            if (CmbTipo.SelectedIndex > 0 && CmbTipo.SelectedItem is string tipo)
                consulta = consulta.Where(c => c.Tipo == tipo);

            if (ChkSoloOfertas.IsChecked == true)
                consulta = consulta.Where(c => c.TieneOferta);

            consulta = CmbOrden.SelectedIndex switch
            {
                1 => consulta.OrderBy(c => c.PrecioFinal),
                2 => consulta.OrderByDescending(c => c.PrecioFinal),
                3 => consulta.OrderByDescending(c => c.AutonomiaKm),
                _ => consulta.OrderByDescending(c => c.EsNovedad).ThenBy(c => c.PrecioFinal),
            };

            var resultado = consulta.ToList();
            ListaCoches.ItemsSource = resultado;

            string cantidad = resultado.Count switch
            {
                0 => "Ningún modelo",
                1 => "1 modelo",
                _ => $"{resultado.Count} modelos",
            };
            TxtResultados.Text = (resultado.Count == Catalogo.Todos.Count
                ? cantidad
                : $"{cantidad} de {Catalogo.Todos.Count}")
                + (resultado.Count > 0 ? " · pulsa uno para ver su ficha" : "");

            PanelVacio.Visibility = resultado.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>Búsqueda tolerante: ignora mayúsculas y tildes («coupe» encuentra «coupé»).</summary>
        private static bool Contiene(string origen, string buscado) =>
            CultureInfo.CurrentCulture.CompareInfo.IndexOf(origen, buscado,
                CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0;

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            cargado = false;
            TxtBuscar.Clear();
            CmbTipo.SelectedIndex = 0;
            CmbOrden.SelectedIndex = 0;
            ChkSoloOfertas.IsChecked = false;
            cargado = true;
            AplicarFiltros();
            TxtBuscar.Focus();
        }

        // ================= FICHA DE DETALLE =================
        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button b && b.DataContext is Coche coche)
            {
                tarjetaOrigen = b;
                AbrirDetalle(coche);
            }
        }

        private void AbrirDetalle(Coche coche)
        {
            bool yaAbierto = PanelDetalle.Visibility == Visibility.Visible;
            PanelDetalle.DataContext = coche;
            PanelDetalle.Visibility = Visibility.Visible;

            if (!yaAbierto) Animaciones.Lanzar(this, "SbAbrirDetalle");
            BtnCerrarDetalle.Focus();
        }

        private void CerrarDetalle()
        {
            PanelDetalle.Visibility = Visibility.Collapsed;
            tarjetaOrigen?.Focus();
        }

        private void BtnCerrarDetalle_Click(object sender, RoutedEventArgs e) => CerrarDetalle();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && PanelDetalle.Visibility == Visibility.Visible)
            {
                CerrarDetalle();
                e.Handled = true;
            }
        }

        // Solicitud de prueba: la respuesta aparece en un MessageBox para que siempre se vea
        private const string TituloMensajes = "Polestar · Prueba de conducción";

        private void BtnPrueba_Click(object sender, RoutedEventArgs e)
        {
            if (PanelDetalle.DataContext is not Coche coche) return;

            if (!Sesion.Iniciada)
            {
                MessageBoxResult respuesta = MessageBox.Show(
                    "Para reservar una prueba necesitas iniciar sesión con tu cuenta de cliente.\n\n¿Quieres iniciar sesión ahora?",
                    TituloMensajes, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (respuesta == MessageBoxResult.Yes)
                    AbrirLogin();
                return;
            }

            MessageBox.Show($"Solicitud enviada (simulada) para el {coche.Nombre}.\n\n" +
                            "La tienda se pondrá en contacto contigo para confirmar la fecha.",
                            TituloMensajes, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AbrirLogin()
        {
            Window login = new LoginWindow();
            Navegacion.MismaPosicion(this, login);
            login.Show();
            this.Close();
        }
    }
}
