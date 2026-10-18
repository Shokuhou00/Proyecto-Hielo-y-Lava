using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BloqueFundible : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el objeto que colisiona tiene "Lava" en su nombre, el bloque se derrite
        if (collision.gameObject.name.Contains("Lava"))
        {
            Debug.Log("Bloque de hielo derretido por la Lava.");
            Destroy(gameObject);
        }
    }
}