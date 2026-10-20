using UnityEngine;

/// <summary>
/// BELLINGHAM-21: lleva el registro de cuál es el nivel más alto
/// desbloqueado, usando PlayerPrefs para que el progreso persista
/// entre partidas. Por defecto, solo el Nivel 1 está desbloqueado.
///
/// Clase estática: no se agrega a ningún GameObject, se usa directamente
/// como ProgresoJuego.NivelDesbloqueado, ProgresoJuego.EstaDesbloqueado(n), etc.
/// </summary>
public static class ProgresoJuego
{
    private const string ClavePref = "NivelDesbloqueado";

    public static int NivelDesbloqueado => PlayerPrefs.GetInt(ClavePref, 1);

    public static bool EstaDesbloqueado(int nivel) => nivel <= NivelDesbloqueado;

    /// <summary>Desbloquea hasta el nivel indicado (si es mayor al progreso actual).</summary>
    public static void DesbloquearHasta(int nivel)
    {
        if (nivel > NivelDesbloqueado)
        {
            PlayerPrefs.SetInt(ClavePref, nivel);
            PlayerPrefs.Save();
        }
    }

    /// <summary>Vuelve a dejar solo el Nivel 1 desbloqueado. Útil para pruebas y demos.</summary>
    public static void Reiniciar()
    {
        PlayerPrefs.DeleteKey(ClavePref);
        PlayerPrefs.Save();
    }
}
