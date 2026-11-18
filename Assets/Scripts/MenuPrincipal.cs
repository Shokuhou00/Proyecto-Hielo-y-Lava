using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Maneja los botones de la escena MainMenu.
/// "Jugar" entra al nivel más alto desbloqueado (Nivel1, Nivel2...).
/// Si esa escena no existe en el build, entra a Nivel 1.
///
/// Se agrega a un GameObject vacío (p. ej. "GestorMenu") en la escena MainMenu.
/// </summary>
public class MenuPrincipal : MonoBehaviour
{
    [Tooltip("Escena del primer nivel")]
    public string escenaNivel1 = "Nivel1";

    [Tooltip("Prefijo de las escenas de nivel: Nivel1, Nivel2...")]
    public string prefijoNivel = "Nivel";

    [Tooltip("Nombre de la escena de Seleccion de nivel")]
    public string escenaSeleccionNivel = "LevelSelect";

    // Boton "Jugar": continua en el nivel mas alto desbloqueado
    public void Jugar()
    {
        string escena = prefijoNivel + ProgresoJuego.NivelDesbloqueado;

        if (!Application.CanStreamedLevelBeLoaded(escena))
        {
            escena = escenaNivel1;
        }

        SceneManager.LoadScene(escena);
    }

    // Boton "Seleccion de nivel"
    public void AbrirSeleccionNivel()
    {
        SceneManager.LoadScene(escenaSeleccionNivel);
    }

    // Boton "Salir"
    public void Salir()
    {
        Debug.Log("Salir presionado (en el editor no cierra la aplicacion; si en una build).");
        Application.Quit();
    }
}