using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaMeta : MonoBehaviour
{
    private bool hieloEnMeta = false;
    private bool lavaEnMeta = false;

    [Tooltip("Nombre de la siguiente escena a cargar")]
    public string siguienteEscena = "MenuPrincipal";

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
        
        // Aquí puedes usar tu clase estática ProgresoJuego para guardar el nivel desbloqueado si lo requieres
        // ProgresoJuego.DesbloquearHasta(2); 

        // Carga la siguiente escena o recarga la actual
        SceneManager.LoadScene(siguienteEscena);
    }
}