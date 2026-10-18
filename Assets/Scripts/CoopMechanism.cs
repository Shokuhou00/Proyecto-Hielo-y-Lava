using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class CoopMechanism : MonoBehaviour
{
    [Header("Configuración del Puzle")]
    [Tooltip("Escribe 'Hielo' o 'Lava' según quién deba activarlo")]
    public string personajeRequerido = "Hielo";
    
    [Tooltip("Activa para botón de un solo uso. Desactiva para palanca On/Off")]
    public bool unSoloUso = true;

    [Header("Eventos (Arrastra la puerta o plataforma aquí)")]
    public UnityEvent alActivar;
    public UnityEvent alDesactivar;

    private bool activado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto que lo toca contiene el nombre del personaje requerido
        if (collision.gameObject.name.Contains(personajeRequerido))
        {
            if (!activado)
            {
                activado = true;
                Debug.Log($"Mecanismo activado por {collision.gameObject.name}");
                alActivar.Invoke();
            }
            else if (!unSoloUso)
            {
                activado = false;
                Debug.Log($"Mecanismo desactivado por {collision.gameObject.name}");
                alDesactivar.Invoke();
            }
        }
    }
}