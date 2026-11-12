using PolestarApp.Models;

namespace PolestarApp.Services
{
    /// <summary>
    /// Origen de datos del prototipo. En una versión real se sustituiría por una API o base de datos.
    /// IMPORTANTE: precios, autonomías y textos son ficticios y orientativos (prototipo académico).
    /// </summary>
    public static class Catalogo
    {
        public static IReadOnlyList<Coche> Todos { get; } = new List<Coche>
        {
            new() { Id = 1, Modelo = "Polestar 2", Version = "Standard range", Tipo = "Berlina", Anio = 2026,
                    Precio = 47900, PrecioOferta = 44900, AutonomiaKm = 500,
                    Descripcion = "Berlina compacta, la opción más asequible de la gama.",
                    Color1 = "#2F3B48", Color2 = "#8A9AAA" },
            new() { Id = 2, Modelo = "Polestar 2", Version = "Long range Dual motor", Tipo = "Berlina", Anio = 2026,
                    Precio = 55900, AutonomiaKm = 550,
                    Descripcion = "La misma berlina, con más autonomía y tracción total.",
                    Color1 = "#1F1F1F", Color2 = "#6B6B6B" },
            new() { Id = 3, Modelo = "Polestar 3", Version = "Long range Dual motor", Tipo = "SUV", Anio = 2026,
                    Precio = 79900, AutonomiaKm = 600, EsNovedad = true,
                    Descripcion = "SUV familiar de cinco plazas.",
                    Color1 = "#3B4A3A", Color2 = "#9DAE95" },
            new() { Id = 4, Modelo = "Polestar 3", Version = "Performance", Tipo = "SUV", Anio = 2026,
                    Precio = 89900, PrecioOferta = 85900, AutonomiaKm = 550,
                    Descripcion = "Versión deportiva del SUV.",
                    Color1 = "#4A3A2A", Color2 = "#C8A27A" },
            new() { Id = 5, Modelo = "Polestar 4", Version = "Long range Single motor", Tipo = "SUV coupé", Anio = 2026,
                    Precio = 59900, AutonomiaKm = 600, EsNovedad = true,
                    Descripcion = "SUV coupé con techo panorámico.",
                    Color1 = "#263A4F", Color2 = "#7FA3C4" },
            new() { Id = 6, Modelo = "Polestar 4", Version = "Long range Dual motor", Tipo = "SUV coupé", Anio = 2026,
                    Precio = 66900, PrecioOferta = 62900, AutonomiaKm = 550,
                    Descripcion = "El SUV coupé con tracción total.",
                    Color1 = "#3A2F4A", Color2 = "#A493BD" },
            new() { Id = 7, Modelo = "Polestar 5", Version = "Dual motor", Tipo = "Gran turismo", Anio = 2026,
                    Precio = 119900, AutonomiaKm = 650, EsNovedad = true,
                    Descripcion = "Gran turismo de cuatro puertas.",
                    Color1 = "#121212", Color2 = "#5C5C5C" },
            new() { Id = 8, Modelo = "Polestar 5", Version = "Performance", Tipo = "Gran turismo", Anio = 2026,
                    Precio = 139900, AutonomiaKm = 650, EsNovedad = true,
                    Descripcion = "La versión más deportiva del gran turismo.",
                    Color1 = "#5A1E1E", Color2 = "#C06A5A" },
        };

        public static IEnumerable<string> Tipos => Todos.Select(c => c.Tipo).Distinct();

        public static IEnumerable<Coche> Novedades => Todos.Where(c => c.EsNovedad).Take(4);
        public static IEnumerable<Coche> MejoresPrecios => Todos.OrderBy(c => c.PrecioFinal).Take(4);
        public static IEnumerable<Coche> Ofertas => Todos.Where(c => c.TieneOferta);

        public static Coche? PorId(int id) => Todos.FirstOrDefault(c => c.Id == id);

        public static IReadOnlyList<Campana> Campanas { get; } = new List<Campana>
        {
            new() { Etiqueta = "OFERTAS DE INVIERNO",
                    Titulo = "Hasta 4.000 € de descuento en modelos seleccionados",
                    Subtitulo = "Promoción ficticia válida hasta el 31 de enero en tiendas adheridas.",
                    TextoBoton = "Ver ofertas", Destino = DestinoCampana.Ofertas,
                    Color1 = "#1C2733", Color2 = "#4E6A85" },
            new() { Etiqueta = "NUEVO",
                    Titulo = "Polestar 5, el gran turismo eléctrico",
                    Subtitulo = "Descubre el gran turismo de cuatro puertas de la gama.",
                    TextoBoton = "Descubrir el Polestar 5", Destino = DestinoCampana.Coche, CocheId = 7,
                    Color1 = "#111111", Color2 = "#4A4A4A" },
            new() { Etiqueta = "PRUEBAS DE CONDUCCIÓN",
                    Titulo = "Pruébalo antes de decidir",
                    Subtitulo = "Reserva una prueba de conducción en tu tienda con tu cuenta de cliente.",
                    TextoBoton = "Reservar prueba", Destino = DestinoCampana.PruebaConduccion,
                    Color1 = "#3A3122", Color2 = "#A8875A" },
        };
    }
}
