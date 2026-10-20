using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BELLINGHAM-21 (HU-03): maneja los botones de la escena MainMenu.
///
/// Prototipo: como todavía no existen las escenas definitivas de nivel,
/// "Jugar" y el botón "Nivel 1" de Selección de nivel cargan la escena
/// jugable actual del equipo (por defecto, SampleScene). Cuando existan
/// Level1/Level2 reales, basta con cambiar el campo "Escena Nivel 1" en
/// el Inspector; no hace falta tocar este script.
///
/// Se agrega a un GameObject vacío (p. ej. "GestorMenu") en la escena
/// MainMenu, y cada botón se conecta a sus métodos desde On Click ().
/// </summary>
public class MenuPrincipal : MonoBehaviour
{
    [Tooltip("Escena que se carga como Nivel 1 mientras el prototipo no tenga niveles definitivos")]
    public string escenaNivel1 = "SampleScene";

    [Tooltip("Nombre de la escena de Seleccion de nivel")]
    public string escenaSeleccionNivel = "LevelSelect";

    // Boton "Jugar": entra directo al nivel mas alto desbloqueado (por ahora, el unico nivel del prototipo)
    public void Jugar()
    {
        SceneManager.LoadScene(escenaNivel1);
    }

    // Boton "Seleccion de nivel"
    public void AbrirSeleccionNivel()
    {
        SceneManager.LoadScene(escenaSeleccionNivel);
    }

    // Boton "Salir"
    public void Salir()
    {
        Debug.Log("Salir presionado (en el editor no cierra la aplicacion; si en una build).");
        Application.Quit();
    }
}
