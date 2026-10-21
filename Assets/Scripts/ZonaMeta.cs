using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMeta : MonoBehaviour
{
    private bool hieloEnMeta = false;
    private bool lavaEnMeta = false;

    [Tooltip("Numero de ESTE nivel (1, 2, 3...). Se usa para desbloquear el siguiente.")]
    public int numeroNivelActual = 1;

    [Tooltip("Nombre de la siguiente escena a cargar (normalmente el selector de niveles)")]
    public string siguienteEscena = "LevelSelect";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hielo")
        {
            hieloEnMeta = true;
            Debug.Log("Hielo llegó a la meta.");
        }
        else if (collision.gameObject.name == "Lava")
        {
            lavaEnMeta = true;
            Debug.Log("Lava llegó a la meta.");
        }

        // Si ambos llegaron, se considera victoria
        if (hieloEnMeta && lavaEnMeta)
        {
            CompletarNivel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Hielo")
        {
            hieloEnMeta = false;
        }
        else if (collision.gameObject.name == "Lava")
        {
            lavaEnMeta = false;
        }
    }

    void CompletarNivel()
    {
        Debug.Log("¡Nivel completado por ambos personajes!");

        // Desbloquea el siguiente nivel usando ProgresoJuego (persistido con PlayerPrefs)
        ProgresoJuego.DesbloquearHasta(numeroNivelActual + 1);

        // Carga la siguiente escena (normalmente el selector de niveles)
        SceneManager.LoadScene(siguienteEscena);
    }
}