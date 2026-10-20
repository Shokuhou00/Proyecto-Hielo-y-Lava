using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// BELLINGHAM-21 (HU-03): controla los botones generales de la escena
/// LevelSelect (Volver y Reiniciar progreso) y el mensaje de aviso
/// compartido que también usan los botones de nivel (ver BotonNivel.cs).
///
/// Se agrega a un GameObject vacío (p. ej. "GestorSeleccionNivel") en la
/// escena LevelSelect.
/// </summary>
public class SeleccionNivelController : MonoBehaviour
{
    [Tooltip("Nombre de la escena del menu principal")]
    public string escenaMenuPrincipal = "MainMenu";

    [Tooltip("Texto compartido de avisos de la pantalla (mismo objeto que usan los BotonNivel)")]
    public TMP_Text textoAviso;

    // Boton "Volver"
    public void Volver()
    {
        SceneManager.LoadScene(escenaMenuPrincipal);
    }

    // Boton "Reiniciar progreso" (util para pruebas y demos)
    public void ReiniciarProgreso()
    {
        ProgresoJuego.Reiniciar();
        if (textoAviso != null)
        {
            textoAviso.text = "Progreso reiniciado. Solo el Nivel 1 esta desbloqueado.";
        }

        // Recarga la escena para que los botones de nivel actualicen su etiqueta
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
