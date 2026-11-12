# Registro de uso de IA — Práctica 1: Prototipo Polestar Store

**Autor:** Pablo Lizón Garay · **Herramienta:** Claude Opus 5.5 (Anthropic), en la aplicación de Claude



| Uso | Fecha | Tema |
|---|---|---|
| 1 | 3 nov 2026 | Esqueleto del diseño y estructura del proyecto |
| 2 | 4 nov 2026 | Primera sesión de corrección de fallos (revisión de código) |
| 3 | 9 nov 2026 | Segunda sesión de corrección de fallos (diseño sin barra de desplazamiento) |
| 4 | 4 nov 2026 | Script para generar el GIF de batería |
| 5 |11 nov 2026 | Auditoría de diseño |

---

## Uso 1 — Esqueleto del diseño (3 nov 2026)

**Prompt**
> Tengo que hacer un prototipo en WPF con Blend para tiendas de coches Polestar. El enunciado pide pantalla
> de inicio con novedades, mejores precios, ofertas de temporada y campañas; login con contraseña y gestión
> de errores; galería con todos los productos; y menú y logotipo comunes a todas las pantallas, con animaciones
> sin abusar. En clase cada sección es una ventana independiente que se abre con `Show()`. ¿Cómo organizo el
> proyecto y qué estructura de carpetas y clases me recomiendas?

**Respuesta (resumen)**
- Propuso tres ventanas (`InicioWindow`, `LoginWindow`, `GaleriaWindow`) y dos controles de usuario comunes
  (`Cabecera` con logotipo, menú e indicador de sesión, y `Pie`).
- Separar datos y presentación: `Models` (`Coche`, `Campana`) y `Services` (`Catalogo` con datos ficticios,
  `Sesion` simulada, `Navegacion`, `Animaciones`).
- Definir colores, tipografía y estilos una sola vez en `App.xaml` y una plantilla de tarjeta de coche reutilizable.
- Animaciones cortas (150–300 ms) ligadas a acciones del usuario, que respeten la opción de Windows de quitar animaciones.

**Qué hice yo**
- Adapté el login al esquema «Controlar Login» de la asignatura (mensajes con `MessageBox`).
- Decidí los modelos, precios y campañas ficticios del catálogo.

---

## Uso 2 — Primera sesión de corrección de fallos (4 nov 2026)

**Prompt**
> Revisa el código del proyecto y dime qué fallos de usabilidad o de código ves, sobre todo en el login,
> la navegación y la ficha de la galería.

**Respuesta (resumen)** — los problemas que recoge el apartado 6.2 de la memoria:
- Tras un error de credenciales el foco volvía a Usuario y la contraseña se mantenía → vaciar la contraseña y
  poner el foco en ella, conservando el usuario.
- Los `MessageBox` no tenían título ni icono → añadir título y el icono según la gravedad (advertencia, error, información).
- Quedaban estilos sin usar de una versión anterior del login (`TxtErrorCampo`, `Tag="Error"`, `BrError`) → eliminarlos.
- La respuesta a «Solicitar prueba de conducción» quedaba fuera de la vista → mostrarla en un cuadro de mensaje.
- Subir el tamaño mínimo de las ventanas a 1200 × 740 para que todo cupiera.
- Al cambiar de sección, abrir la ventana nueva en la misma posición y tamaño (`Navegacion.MismaPosicion`),
  también si estaba maximizada.

**Qué hice yo**
- Elegí qué cambios aplicar y los comprobé con las pruebas 1–4, 6, 7 y 10 del apartado 6.1.

---

## Uso 3 — Segunda sesión de corrección de fallos (9 nov 2026)

**Prompt**
> Ahora mismo la pantalla de inicio es demasiado larga y el login no queda centrado. ¿Cómo lo reorganizo?

**Respuesta (resumen)**
- Login centrado en la ventana.
- Avisó de la limitación en pantallas pequeñas o con escala alta (1366 × 768, o 150 %), que anoté en 6.2.

**Qué hice yo**
- Comprobé en tamaño por defecto, mínimo y maximizado
  (capturas 16–21).

---

## Uso 4 — GIF de batería (4 nov 2026)

**Prompt**
> Necesito un GIF sencillo de una batería cargándose para el bloque «100 % eléctricos» de la pantalla de inicio.
> ¿Puedes hacerme un script en Python que lo genere?

**Respuesta (resumen)**
- Script de Python con Pillow que dibuja los fotogramas de una batería llenándose y los guarda como `carga.gif`.
- Para limitar la reproducción a 3 repeticiones en WPF, recomendó el paquete NuGet `WpfAnimatedGif`
  (`RepeatBehavior="3x"`), de modo que se quede en el último fotograma.

**Qué hice yo**
- Ejecuté el script, revisé el resultado e integré el GIF en Inicio.

---

## Uso 5 — Auditoría de diseño (11 nov 2026)

**Prompt**
> Haz una auditoría de diseño del prototipo con las capturas de las tres pantallas: consistencia, jerarquía
> visual, prevención de errores, feedback y accesibilidad. Dime qué es confuso para un cliente en tienda.

**Respuesta (resumen)**
- Confirmó la consistencia entre pantallas (cabecera, pie, tarjetas) y recomendó un único color de acento
  y una escala tipográfica fija.
- Accesibilidad: comprobar el contraste del texto, añadir `AutomationProperties.Name` a botones de icono y tarjetas,
  ampliar la zona de clic de los puntos del carrusel y permitir pausar el avance automático.
- Señaló que el foco de desplegables y del control deslizante es poco visible y que no todas las animaciones
  respetan la preferencia de Windows (pruebas 12 y 13).
- Recomendó dejar claro en el pie que los datos son orientativos y avisar en la ficha de que la conducción
  autónoma completa aún no se vende en Europa.

**Qué hice yo**
- Contrasté la auditoría con la prueba del prototipo con familiares.
