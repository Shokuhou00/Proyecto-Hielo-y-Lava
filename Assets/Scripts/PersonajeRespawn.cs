using UnityEngine;

/// <summary>
/// BELLINGHAM-16: guarda la posición inicial del personaje y permite
/// hacerlo reaparecer allí cuando toca un elemento letal para él
/// (ver PeligroElemental.cs) o cuando cae fuera del nivel.
/// </summary>
public class PersonajeRespawn : MonoBehaviour
{
    [Tooltip("Si el personaje cae por debajo de esta altura, reaparece automáticamente")]
    public float limiteCaidaY = -10f;

    private Vector3 puntoDeInicio;
    private Rigidbody2D rb;

    private void Start()
    {
        puntoDeInicio = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y < limiteCaidaY)
        {
            Reaparecer();
        }
    }

    /// <summary>
    /// Vuelve a colocar al personaje en su punto de inicio y detiene su velocidad.
    /// Lo invoca PeligroElemental cuando el personaje toca un elemento que le es letal.
    /// </summary>
    public void Reaparecer()
    {
        transform.position = puntoDeInicio;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
