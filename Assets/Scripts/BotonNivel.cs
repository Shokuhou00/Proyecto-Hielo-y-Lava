using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// BELLINGHAM-21 (HU-03): lógica de un botón de nivel en la escena
/// LevelSelect. Si el nivel está bloqueado, muestra un aviso en pantalla
/// y NO carga la escena.
///
/// Se agrega uno de estos por cada botón de nivel (Nivel 1, Nivel 2, ...).
/// </summary>
public class BotonNivel : MonoBehaviour
{
    [Tooltip("Numero de este nivel (1, 2, 3...)")]
    public int numeroNivel = 1;

    [Tooltip("Nombre de la escena que carga este nivel")]
    public string nombreEscena = "SampleScene";

    [Tooltip("Texto del propio boton (opcional, para mostrar 'bloqueado')")]
    public TMP_Text etiqueta;

    [Tooltip("Texto compartido de avisos de la pantalla (opcional)")]
    public TMP_Text textoAviso;

    private void Start()
    {
        ActualizarEtiqueta();
    }

    private void ActualizarEtiqueta()
    {
        if (etiqueta == null) return;
        etiqueta.text = ProgresoJuego.EstaDesbloqueado(numeroNivel)
            ? "Nivel " + numeroNivel
            : "Nivel " + numeroNivel + " (bloqueado)";
    }

    // Conectar al On Click () del boton, en el Inspector
    public void AlPulsar()
    {
        if (!ProgresoJuego.EstaDesbloqueado(numeroNivel))
        {
            if (textoAviso != null)
            {
                textoAviso.text = "El nivel " + numeroNivel + " esta bloqueado. Completa el nivel anterior.";
            }
            Debug.Log("Intento de abrir nivel bloqueado: " + numeroNivel);
            return; // no carga la escena
        }

        SceneManager.LoadScene(nombreEscena);
    }
}
