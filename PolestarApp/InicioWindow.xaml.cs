using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PolestarApp.Models;
using PolestarApp.Services;

namespace PolestarApp
{
    public partial class InicioWindow : Window
    {
        private const double TinAnual = 0.0599;
        private static readonly CultureInfo Es = new("es-ES");

        private readonly IReadOnlyList<Campana> campanas = Catalogo.Campanas;
        private readonly DispatcherTimer temporizador = new() { Interval = TimeSpan.FromSeconds(8) };
        private readonly List<Button> puntos = new();
        private int indice;
        private bool ratonEncima, focoDentro;
        private bool cargado;

        public InicioWindow()
        {
            InitializeComponent();

            MostrarProductos(animar: false);   // pestaña inicial: Novedades

            CrearPuntos();
            MostrarCampana(0, animar: false);
            temporizador.Tick += (_, _) => MostrarCampana(indice + 1, animar: true);
            Loaded += (_, _) =>
            {
                temporizador.Start();
                Animaciones.Entrada(Contenido);   // aparición suave del contenido
            };
            Closed += (_, _) => temporizador.Stop();

            // Calculadora
            CmbModelo.ItemsSource = Catalogo.Todos;
            CmbModelo.SelectedIndex = 0;
            CmbPlazo.ItemsSource = new[] { "24 meses", "36 meses", "48 meses", "60 meses" };
            CmbPlazo.SelectedIndex = 2;
            cargado = true;
            ActualizarCuota();
        }

        // ================= CARRUSEL =================
        private void CrearPuntos()
        {
            for (int i = 0; i < campanas.Count; i++)
            {
                int destino = i;
                var punto = new Button
                {
                    Style = (Style)FindResource("BtnPunto"),
                    ToolTip = campanas[i].Etiqueta,
                };
                System.Windows.Automation.AutomationProperties.SetName(punto, $"Campaña {i + 1} de {campanas.Count}");
                punto.Click += (_, _) => { MostrarCampana(destino, animar: true); ReiniciarTemporizador(); };
                puntos.Add(punto);
                PanelPuntos.Children.Add(punto);
            }
        }

        private void MostrarCampana(int nuevoIndice, bool animar)
        {
            indice = (nuevoIndice % campanas.Count + campanas.Count) % campanas.Count;
            var c = campanas[indice];

            FondoCampana.Background = c.Fondo;
            TxtEtiquetaCampana.Text = c.Etiqueta;
            TxtTituloCampana.Text = c.Titulo;
            TxtSubtituloCampana.Text = c.Subtitulo;
            BtnCampana.Content = c.TextoBoton;

            for (int i = 0; i < puntos.Count; i++)
                puntos[i].Tag = i == indice ? "Activo" : null;

            if (animar) Animaciones.Entrada(ContenidoCampana, desplazamiento: 6, ms: 300);
        }

        private void ReiniciarTemporizador()
        {
            temporizador.Stop();
            if (!ratonEncima && !focoDentro) temporizador.Start();
        }

        private void BtnAnterior_Click(object sender, RoutedEventArgs e)
        {
            MostrarCampana(indice - 1, animar: true);
            ReiniciarTemporizador();
        }

        private void BtnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            MostrarCampana(indice + 1, animar: true);
            ReiniciarTemporizador();
        }

        private void Carrusel_PausarRaton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ratonEncima = true;
            temporizador.Stop();
        }

        private void Carrusel_ReanudarRaton(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ratonEncima = false;
            ReiniciarTemporizador();
        }

        private void Carrusel_FocoCambiado(object sender, DependencyPropertyChangedEventArgs e)
        {
            focoDentro = (bool)e.NewValue;
            ReiniciarTemporizador();
        }

        private void BtnCampana_Click(object sender, RoutedEventArgs e)
        {
            var c = campanas[indice];
            Window ventana;
            switch (c.Destino)
            {
                case DestinoCampana.Ofertas:
                    ventana = new GaleriaWindow(soloOfertas: true);
                    break;
                case DestinoCampana.Coche:
                    ventana = new GaleriaWindow(cocheId: c.CocheId);
                    break;
                default:
                    // Pruebas de conducción: si ya hay sesión, a la galería para elegir modelo
                    ventana = Sesion.Iniciada ? new GaleriaWindow() : new LoginWindow();
                    break;
            }

            Navegacion.MismaPosicion(this, ventana);
            ventana.Show();
            this.Close();
        }

        // ================= PESTAÑAS DE PRODUCTOS =================
        private void Pestana_Cambiada(object sender, RoutedEventArgs e)
        {
            // Durante InitializeComponent la primera pestaña se marca antes de que exista la lista
            if (ListaProductos != null) MostrarProductos(animar: true);
        }

        private void MostrarProductos(bool animar)
        {
            if (TabPrecios.IsChecked == true)
            {
                ListaProductos.ItemsSource = Catalogo.MejoresPrecios.ToList();
                TxtDescripcionLista.Text = "Los más asequibles, por precio final";
            }
            else if (TabOfertas.IsChecked == true)
            {
                ListaProductos.ItemsSource = Catalogo.Ofertas.ToList();
                TxtDescripcionLista.Text = "Descuentos de invierno ya aplicados";
            }
            else
            {
                ListaProductos.ItemsSource = Catalogo.Novedades.ToList();
                TxtDescripcionLista.Text = "Los últimos modelos en llegar a tienda";
            }

            if (animar) Animaciones.Entrada(ListaProductos, desplazamiento: 6, ms: 200);
        }

        // ================= TARJETAS =================
        private void Tarjeta_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement fe && fe.DataContext is Coche coche)
            {
                Window galeria = new GaleriaWindow(cocheId: coche.Id);
                Navegacion.MismaPosicion(this, galeria);
                galeria.Show();
                this.Close();
            }
        }

        // ================= CALCULADORA =================
        private void Calculadora_Cambiada(object sender, RoutedEventArgs e)
        {
            if (cargado) ActualizarCuota();
        }

        private void ActualizarCuota()
        {
            if (CmbModelo.SelectedItem is not Coche coche) return;

            int meses = (CmbPlazo.SelectedIndex + 2) * 12;          // 24, 36, 48, 60
            decimal entrada = coche.PrecioFinal * (decimal)(SldEntrada.Value / 100);
            double capital = (double)(coche.PrecioFinal - entrada);
            double r = TinAnual / 12;
            double cuota = capital * r / (1 - Math.Pow(1 + r, -meses));

            LblEntrada.Content = $"_Entrada: {SldEntrada.Value:0} % ({entrada.ToString("C0", Es)})";
            RunCuota.Text = cuota.ToString("C0", Es);
            TxtResumenCuota.Text =
                $"Precio {coche.PrecioTexto} · {meses} cuotas · financiado {capital.ToString("C0", Es)}";
        }
    }
}
