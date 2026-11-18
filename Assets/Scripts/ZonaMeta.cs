using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMeta : MonoBehaviour
{
    private bool hieloEnMeta = false;
    private bool lavaEnMeta = false;
    private bool completado = false;

    [Tooltip("Numero de ESTE nivel (1, 2, 3...). Se usa para desbloquear el siguiente.")]
    public int numeroNivelActual = 1;

    [Tooltip("Nombre de la siguiente escena a cargar (normalmente el selector de niveles)")]
    public string siguienteEscena = "LevelSelect";

    [Tooltip("(Opcional) Menu del nivel. Si se asigna, muestra la pantalla de victoria en lugar de cargar siguienteEscena.")]
    public MenuNivel menu;

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
        if (hieloEnMeta && lavaEnMeta && !completado)
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
        completado = true;
        Debug.Log("¡Nivel completado por ambos personajes!");

        // Desbloquea el siguiente nivel usando ProgresoJuego (persistido con PlayerPrefs)
        ProgresoJuego.DesbloquearHasta(numeroNivelActual + 1);

        if (menu != null)
        {
            menu.MostrarVictoria();
        }
        else
        {
            SceneManager.LoadScene(siguienteEscena);
        }
    }
}