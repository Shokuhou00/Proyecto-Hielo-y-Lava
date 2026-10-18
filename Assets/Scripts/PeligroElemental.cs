using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// BELLINGHAM-16
/// Como jugador, quiero que cada personaje sea vulnerable solo a su
/// elemento opuesto, para que evitar obstáculos letales sea parte del reto.
///
/// Se coloca sobre cualquier zona de peligro del nivel (lava, fuego, agua
/// o hielo) con un Collider2D marcado como "Is Trigger". Reglas de
/// vulnerabilidad:
///   - Hielo muere al tocar Lava o Fuego.
///   - Lava muere al tocar Agua o Hielo.
///   - El propio elemento de cada personaje nunca lo elimina
///     (Hielo no muere con Agua/Hielo; Lava no muere con Lava/Fuego).
///
/// Identifica al personaje por su nombre ("Hielo" / "Lava"), igual que
/// BloqueFundible.cs, para no depender de un componente adicional sobre
/// los personajes ya existentes en la escena.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PeligroElemental : MonoBehaviour
{
    public enum TipoElemento { Lava, Fuego, Agua, Hielo }

    [Header("Configuración del peligro")]
    [Tooltip("Elemento que representa esta zona de peligro")]
    public TipoElemento tipoElemento = TipoElemento.Lava;

    [Header("Eventos (opcional: sonido, partículas, etc.)")]
    public UnityEvent alMatarPersonaje;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject personaje = collision.gameObject;

        if (!EsLetalPara(personaje.name))
        {
            return;
        }

        Debug.Log($"{personaje.name} murió al tocar {tipoElemento}");
        alMatarPersonaje.Invoke();

        PersonajeRespawn respawn = personaje.GetComponent<PersonajeRespawn>();
        if (respawn != null)
        {
            respawn.Reaparecer();
        }
        else
        {
            Debug.LogWarning($"{personaje.name} no tiene PersonajeRespawn asignado; no se pudo reaparecer.");
        }
    }

    /// <summary>
    /// Determina si el elemento de esta zona es letal para el personaje
    /// dado su nombre. Cada personaje solo es vulnerable a su elemento
    /// opuesto; su propio elemento nunca lo afecta.
    /// </summary>
    private bool EsLetalPara(string nombrePersonaje)
    {
        bool esHielo = nombrePersonaje.Contains("Hielo");
        bool esLava = nombrePersonaje.Contains("Lava");

        if (esHielo)
        {
            return tipoElemento == TipoElemento.Lava || tipoElemento == TipoElemento.Fuego;
        }

        if (esLava)
        {
            return tipoElemento == TipoElemento.Agua || tipoElemento == TipoElemento.Hielo;
        }

        return false; // no es ninguno de los dos personajes conocidos
    }
}
