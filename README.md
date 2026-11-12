# Polestar Store — Prototipo WPF (Práctica 1, Interfaces de Usuario)

Prototipo de aplicación de escritorio para tiendas de venta de coches Polestar, hecho con
**Blend for Visual Studio** sobre un proyecto **WPF (.NET 10)**. Precios, ofertas y datos son ficticios.

## Cómo abrirlo
1. Instalar Visual Studio con la carga de trabajo **Desarrollo de escritorio de .NET** (incluye Blend).
2. Abrir `PolestarApp.sln` en Blend o en Visual Studio.
3. Ejecutar con **F5**.

Cuenta de prueba: usuario `Alumno`, contraseña `1234`

## Estructura
Cada sección es una ventana WPF independiente (Proyecto → Agregar ventana (WPF)). Al cambiar de sección se abre
la ventana nueva con `Show()` en la misma posición y se cierra la anterior.

```
PolestarApp/
├─ App.xaml              Estilos, colores y plantilla de tarjeta compartidos (arranca en InicioWindow)
├─ InicioWindow          Campañas, GIF «100 % eléctricos», novedades, mejores precios, ofertas y cuota
├─ LoginWindow           Acceso de clientes (comprobación con MessageBox)
├─ GaleriaWindow         Catálogo con búsqueda, filtros, orden y ficha de detalle
├─ Controls/             Cabecera (logo, menú, sesión) y Pie, comunes a las tres ventanas
├─ Models/               Coche, Campana
├─ Services/             Catalogo, Sesion, Navegacion, Animaciones
└─ Images/carga.gif      GIF animado (se repite 3 veces con WpfAnimatedGif)
Docs/documentacion-tecnica.md  Documentación técnica de la práctica
Docs/registro-ia.md      Registro de uso de IA
```

Dependencia: paquete NuGet `WpfAnimatedGif` (Apache 2.0), se descarga solo al compilar.
