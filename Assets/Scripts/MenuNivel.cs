using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// Menú dentro del nivel. Esc abre/cierra la pausa. ZonaMeta lo reutiliza
/// como pantalla de victoria (MostrarVictoria).
///
/// Convención: las escenas de nivel se llaman Nivel1, Nivel2, Nivel3...
/// El número de nivel se toma del nombre de la escena automáticamente.
/// Usa tu ProgresoJuego existente para saber qué niveles están desbloqueados.
/// </summary>
public class MenuNivel : MonoBehaviour
{
    [Header("Escenas")]
    [Tooltip("Prefijo de las escenas de nivel: Nivel1, Nivel2...")]
    public string prefijoEscena = "Nivel";
    public string escenaSeleccionNivel = "LevelSelect";
    public string escenaMenuPrincipal = "MainMenu";

    [Tooltip("Solo se usa si la escena NO se llama NivelN")]
    public int numeroNivelRespaldo = 1;

    [Header("UI")]
    public GameObject panel;
    public TMP_Text titulo;
    public Button botonContinuar;
    public Button botonAnterior;
    public Button botonSiguiente;

    private bool abierto;
    private bool victoria;
    private MovimientoHielo hielo;
    private PlayerMovement lava;

    private int Numero
    {
        get
        {
            string n = SceneManager.GetActiveScene().name;
            if (n.StartsWith(prefijoEscena) &&
                int.TryParse(n.Substring(prefijoEscena.Length), out int num))
                return num;
            return numeroNivelRespaldo;
        }
    }

    private void Start()
    {
       hielo = FindAnyObjectByType<MovimientoHielo>();
       lava = FindAnyObjectByType<PlayerMovement>();
        Cerrar();
    }

    private void Update()
    {
        if (victoria) return;
        var kb = Keyboard.current;
        if (kb != null && kb.escapeKey.wasPressedThisFrame)
        {
            if (abierto) Cerrar(); else Abrir();
        }
    }

    public void Abrir()
    {
        abierto = true;
        panel.SetActive(true);
        Time.timeScale = 0f;
        PermitirMovimiento(false);
        Refrescar();
    }

    public void Cerrar()
    {
        abierto = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
        PermitirMovimiento(true);
    }

    public void MostrarVictoria()
    {
        victoria = true;
        Abrir();
    }

    private void PermitirMovimiento(bool permitir)
    {
        if (hielo != null) hielo.enabled = permitir;
        if (lava != null) lava.enabled = permitir;
    }

    private bool Existe(int nivel) =>
        Application.CanStreamedLevelBeLoaded(prefijoEscena + nivel);

    private void Refrescar()
    {
        int n = Numero;

        if (titulo != null) titulo.text = victoria ? "¡Nivel completado!" : "Pausa";
        if (botonContinuar != null) botonContinuar.gameObject.SetActive(!victoria);
        if (botonAnterior != null) botonAnterior.interactable = n > 1 && Existe(n - 1);
        if (botonSiguiente != null)
            botonSiguiente.interactable = ProgresoJuego.EstaDesbloqueado(n + 1) && Existe(n + 1);
    }

    private void Cargar(string escena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escena);
    }

    // ---- Conectar a On Click () de los botones ----
    public void Continuar() => Cerrar();
    public void Reiniciar() => Cargar(SceneManager.GetActiveScene().name);
    public void Anterior() => Cargar(prefijoEscena + (Numero - 1));
    public void Siguiente() => Cargar(prefijoEscena + (Numero + 1));
    public void SeleccionNivel() => Cargar(escenaSeleccionNivel);
    public void IrAlMenuPrincipal() => Cargar(escenaMenuPrincipal);

    public void Salir()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}