#  Hielo y Lava — Videojuego Cooperativo de Plataformas 2D

**Hielo y Lava** es un videojuego cooperativo local de plataformas y puzles en 2D desarrollado en **Unity (Universal Render Pipeline - URP 2D)** con **C#**. Inspirado en mecánicas de cooperación asimétrica y sincronizada, dos jugadores controlan simultáneamente a los personajes elementales (**Hielo** y **Lava**) en un mismo teclado para superar obstáculos elementales, activar mecanismos cooperativos y alcanzar juntos la zona de meta a través de múltiples niveles.

---

## Tabla de Contenidos

1. [Ficha Técnica y Tecnologías](#-1-ficha-técnica-y-tecnologías)
2. [Estructura del Proyecto](#-2-estructura-del-proyecto)
3. [Controles de Juego (Cooperativo Local)](#-3-controles-de-juego-cooperativo-local)
4. [Mecánicas Principales y Reglas del Juego](#-4-mecánicas-principales-y-reglas-del-juego)
5. [Arquitectura de Scripts (C#)](#-5-arquitectura-de-scripts-c)
6. [Flujo de Escenas y Niveles](#-6-flujo-de-escenas-y-niveles)
7. [Sistema de Guardado y Progreso](#-7-sistema-de-guardado-y-progreso)
8. [Instalación y Ejecución](#-8-instalación-y-ejecución)

---

## 1. Ficha Técnica y Tecnologías

* **Motor de Videojuegos:** Unity 6 / Unity 2D (con `Universal Render Pipeline - URP 2D` y `Renderer2D`)
* **Lenguaje de Programación:** C# (`.NET / Mono`)
* **Sistema de Entrada:** Unity **New Input System** (`UnityEngine.InputSystem`)
* **Físicas y Colisiones:** Motor físico 2D de Unity (`Rigidbody2D`, `Collider2D`, Triggers, `Physics2D.OverlapCircle`, `Physics2D.Raycast`)
* **Interfaz de Usuario (UI):** Unity UI (`UnityEngine.UI`) + **TextMesh Pro** (`TMPro`)
* **Persistencia de Datos:** `PlayerPrefs` nativo de Unity
* **Plataforma de Compilación:** Windows x86_64 (Direct3D 12 - Ejecutable incluido en `Hielo-y-Lava_V0.1/HieloYLava.exe`)

---

## 2. Estructura del Proyecto

```text
Proyecto-Hielo-y-Lava-main/
├── Assets/
│   ├── Scenes/                         # Escenas principales del videojuego
│   │   ├── MainMenu.unity              # Menú principal (Jugar, Selección de Nivel, Salir)
│   │   ├── LevelSelect.unity           # Mapa de selección y desbloqueo de niveles
│   │   ├── Nivel1.unity                # Nivel 1: Introducción a mecánicas
│   │   ├── Nivel2.unity                # Nivel 2: Peligros elementales y bloques fundibles
│   │   ├── Nivel3.unity                # Nivel 3: Puzles con mecanismos cooperativos
│   │   └── Nivel4.unity                # Nivel 4: Desafío final
│   ├── PruebaMecanismos.unity          # Escena de pruebas/sandbox de mecánicas
│   ├── Scripts/                        # Lógica del juego en C#
│   │   ├── BloqueFundible.cs           # Bloques de hielo que se derriten con Lava
│   │   ├── BotonNivel.cs               # Control visual e interactivo de botones de nivel
│   │   ├── CoopMechanism.cs            # Botones/placas de presión y puertas deslizantes
│   │   ├── MenuNivel.cs                # HUD, pausa, temporizador y pantalla de victoria
│   │   ├── MenuPrincipal.cs            # Navegación del menú de inicio
│   │   ├── MovimientoHielo.cs          # Controlador físico avanzado del personaje Hielo
│   │   ├── MovimientoLava.cs           # Controlador físico del personaje Lava
│   │   ├── PeligroElemental.cs         # Lógica de trampas (Lava, Hielo, Ácido)
│   │   ├── PersonajeRespawn.cs         # Sistema de reaparición en punto de origen
│   │   ├── ProgresoJuego.cs            # Clase estática de persistencia con PlayerPrefs
│   │   ├── SeleccionNivelController.cs # Controlador de la pantalla de selección de niveles
│   │   └── ZonaMeta.cs                 # Verificación cooperativa de fin de nivel
│   ├── Settings/                       # Configuración de URP 2D, Input System y Build Profiles
│   └── TextMesh Pro/                   # Recursos tipográficos y shaders SDF para UI
├── Hielo-y-Lava_V0.1/                  # Build ejecutable compilado para Windows (HieloYLava.exe)
├── Packages/                           # Manifiesto de paquetes de Unity
└── ProjectSettings/                    # Configuración global del proyecto, tags y capas
```

---

## 3. Controles de Juego (Cooperativo Local)

El juego está diseñado para **2 jugadores en el mismo teclado** utilizando la API de `Keyboard.current` del nuevo Input System de Unity:

| Acción | ❄️ Jugador 1: Hielo (`Tag: Ice`) | 🔥 Jugador 2: Lava (`Tag: Lava`) |
| :--- | :---: | :---: |
| **Mover a la Izquierda** | `Flecha Izquierda` (`←`) | `A` |
| **Mover a la Derecha** | `Flecha Derecha` (`→`) | `D` |
| **Saltar** | `Flecha Arriba` (`↑`) | `W` |
| **Pausar / Reanudar Partida** | `Escape` (`ESC`) | `Escape` (`ESC`) |

---

## 4. Mecánicas Principales y Reglas del Juego

### 1. Afinidad y Peligros Elementales (`PeligroElemental.cs`)
Existen tres tipos de superficies peligrosas configurables mediante el enum `TipoPeligro`:
* **Charco de Lava (`Lava`):** Es seguro para el personaje **Lava**, pero letal para **Hielo** (lo envía de vuelta a su punto de aparición).
* **Charco / Trampa de Hielo (`Hielo`):** Es seguro para el personaje **Hielo**, pero letal para **Lava**.
* **Ácido / Peligro Universal (`Acido`):** Es letal para **ambos jugadores** sin importar su elemento.

### 2. Bloques Fundibles (`BloqueFundible.cs`)
* Son obstáculos sólidos en el escenario que bloquean el paso.
* Si el personaje **Lava** (`CompareTag("Lava")`) entra en colisión o contacto con un bloque fundible, lo derrite instantáneamente (`Destroy(gameObject)`), abriendo el camino para ambos jugadores.

### 3. Mecanismos Cooperativos y Puertas (`CoopMechanism.cs`)
* **Placas de presión / Botones:** Detectan cuántos objetos válidos (`Tag: Ice`, `Tag: Lava` o `Rigidbody2D`) están encima mediante un contador `objectsOnButton`.
* **Puerta vinculada (`door`):** Mientras al menos un personaje mantenga presionado el botón (`isPressed = true`), la puerta se desliza suavemente mediante interpolación vectorial (`Vector3.MoveTowards`) hacia su posición abierta (`closedPosition + openOffset`). Si el personaje se retira, la puerta regresa automáticamente a su posición cerrada.

### 4. Condición de Victoria Cooperativa (`ZonaMeta.cs`)
* La meta requiere que **ambos personajes (`Ice` y `Lava`) estén simultáneamente dentro del área de la meta**.
* Cuando `hieloEnMeta && lavaEnMeta` se cumple, se invoca `MenuNivel.MostrarVictoria()`, deteniendo el cronómetro y desbloqueando el siguiente nivel.

---

## 5. Arquitectura de Scripts (C#)

A continuación se detalla la responsabilidad técnica de los 12 scripts que componen el núcleo del juego:

| Script | Responsabilidad y Detalles Técnicos |
| :--- | :--- |
| **`MovimientoHielo.cs`** | Controla al personaje **Hielo** con las flechas del teclado. Incluye detección de suelo dual (`Physics2D.OverlapCircle` con `groundCheck` o `Physics2D.Raycast`), sistema de **Coyote Time** (`0.08f` s) para saltos más fluidos al borde de plataformas y ajuste dinámico de gravedad (`fallGravityMultiplier = 2.2f` al caer y `lowJumpGravityMultiplier = 2f` en saltos cortos). |
| **`MovimientoLava.cs`** | Controla al personaje **Lava** con las teclas `A`, `D`, `W`. Utiliza `Rigidbody2D.linearVelocity` para el desplazamiento horizontal y salto, junto con detección de suelo por `OverlapCircle` sobre la capa `groundLayer`. |
| **`PersonajeRespawn.cs`** | Guarda en `Awake()` la coordenada inicial (`posicionInicial`) del personaje. Expone el método público `Respawnear()` que detiene por completo la inercia física (`rb.linearVelocity = Vector2.zero`, `rb.angularVelocity = 0f`) y teletransporta al jugador a su origen tras tocar un peligro. |
| **`PeligroElemental.cs`** | Evalúa colisiones tanto por `OnTriggerEnter2D` como por `OnCollisionEnter2D`. Compara el `Tag` del jugador (`Ice` o `Lava`) contra el `TipoPeligro` (`Lava`, `Hielo`, `Acido`). Si es letal, invoca `PersonajeRespawn.Respawnear()` o recarga la escena como respaldo. |
| **`BloqueFundible.cs`** | Escucha `OnCollisionEnter2D` y `OnTriggerEnter2D`. Si el objeto que lo toca tiene el tag `"Lava"`, destruye el `GameObject` del bloque. |
| **`CoopMechanism.cs`** | Gestiona botones de presión y puertas móviles. Usa `Update()` con `Vector3.MoveTowards` para animar la apertura y cierre de la puerta a velocidad `doorSpeed`. |
| **`ZonaMeta.cs`** | Lleva el estado booleano `hieloEnMeta` y `lavaEnMeta` con `OnTriggerEnter2D` y `OnTriggerExit2D`. Al estar ambos presentes, notifica a `MenuNivel` para finalizar la partida. |
| **`MenuNivel.cs`** | Administra el ciclo de vida del nivel: cronómetro en tiempo real (`MM:SS`), menú de pausa con tecla `ESC` (controlando `Time.timeScale`), pantalla de victoria, reinicio de nivel y transición al siguiente nivel (`Nivel1` $\rightarrow$ `Nivel2`, etc.). |
| **`ProgresoJuego.cs`** | Clase estática que persiste el nivel máximo alcanzado usando `PlayerPrefs` bajo la clave `"NivelDesbloqueado"`. Por defecto inicia en el nivel `1`. |
| **`BotonNivel.cs`** | Componente de UI en la pantalla de selección de niveles. Consulta `ProgresoJuego.NivelEstaDesbloqueado(numeroNivel)` para habilitar/deshabilitar el botón (`interactable`), mostrar el candado (`iconoCandado`) y cambiar la opacidad (`colorBloqueado`). |
| **`SeleccionNivelController.cs`** | Controla las acciones de la escena `LevelSelect`: volver al menú principal y un botón de desarrollo (`ReiniciarProgreso`) para resetear los niveles desbloqueados. |
| **`MenuPrincipal.cs`** | Gestiona la escena `MainMenu`: iniciar desde el `Nivel1`, ir al selector de niveles (`LevelSelect`) o cerrar la aplicación (`Application.Quit()`). |

---

## 6. Flujo de Escenas y Niveles

El flujo de navegación del jugador dentro del proyecto sigue esta estructura:

```text
[ MainMenu.unity ]
   ├── Botón "Jugar" ──────────────► [ Nivel1.unity ]
   ├── Botón "Seleccionar Nivel" ──► [ LevelSelect.unity ]
   │                                    ├──► [ Nivel1.unity ] (Desbloqueado por defecto)
   │                                    ├──► [ Nivel2.unity ] (Se desbloquea al pasar Nivel 1)
   │                                    ├──► [ Nivel3.unity ] (Se desbloquea al pasar Nivel 2)
   │                                    └──► [ Nivel4.unity ] (Se desbloquea al pasar Nivel 3)
   └── Botón "Salir" ──────────────► Cierra la aplicación
```

* En cada escena de nivel (`Nivel1` a `Nivel4`), al completar la meta se muestra el **Panel de Victoria** con el tiempo total empleado (`Tiempo: MM:SS`) y las opciones:
  * **Siguiente Nivel** (carga `Nivel{actual + 1}`)
  * **Reiniciar Nivel**
  * **Selección de Nivel** (`LevelSelect`)
  * **Menú Principal** (`MainMenu`)

---

## 7. Sistema de Guardado y Progreso

El progreso del jugador se almacena localmente en el registro del sistema operativo mediante `UnityEngine.PlayerPrefs` gestionado por `ProgresoJuego.cs`:

* **Clave de almacenamiento:** `"NivelDesbloqueado"` (Entero).
* **Lógica de desbloqueo:** Cuando ambos jugadores llegan a la meta del nivel $N$, `MenuNivel.MostrarVictoria()` ejecuta:
  ```csharp
  ProgresoJuego.DesbloquearSiguienteNivel(numeroNivelActual);
  ```
  Lo cual actualiza el valor guardado a $N + 1$ únicamente si $N + 1$ es mayor que el progreso actual registrado.

---

## 8. Instalación y Ejecución

### Opción A: Ejecutar el Juego Compilado (Sin instalar Unity)
El repositorio ya incluye una versión ejecutable lista para jugar en Windows:
1. Abre la carpeta `Hielo-y-Lava_V0.1/`.
2. Ejecuta **`HieloYLava.exe`**.

### Opción B: Abrir y Editar el Proyecto en Unity Editor
1. Instala **Unity Hub** y una versión de **Unity 6 (6000.x)** o **Unity 2022.3+ LTS** con soporte para **Universal Render Pipeline (URP 2D)**.
2. En Unity Hub, haz clic en **Add -> Add project from disk** y selecciona la carpeta raíz `Proyecto-Hielo-y-Lava-main`.
3. Abre la escena inicial en `Assets/Scenes/MainMenu.unity` y presiona el botón **Play (▶)** en el editor.