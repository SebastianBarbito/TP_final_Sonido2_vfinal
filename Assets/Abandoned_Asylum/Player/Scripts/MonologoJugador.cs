using UnityEngine;
using System.Collections;

public class MonologoJugador : MonoBehaviour
{
    [Header("Wwise Voice")]
    [Tooltip("Arrastra aquí el único evento Play_Monologo")]
    public AK.Wwise.Event eventoMonologo;

    [Header("Intervalo de Tiempo")]
    public float tiempoMinimo = 5f;
    public float tiempoMaximo = 6f;

    // Bandera estática global para bloquear los monólogos desde el ascensor u otras interacciones
    public static bool bloqueado = false;

    void Start()
    {
        bloqueado = false;
        StartCoroutine(RutinaMonologos());
    }

    IEnumerator RutinaMonologos()
    {
        while (true)
        {
            // Espera un tiempo aleatorio entre 5 y 6 segundos
            float espera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(espera);

            // Si no está bloqueado, dispara el evento (Wwise elegirá una voz al azar sin repetir la última)
            if (!bloqueado && eventoMonologo != null && eventoMonologo.IsValid())
            {
                eventoMonologo.Post(gameObject);
            }
        }
    }
}