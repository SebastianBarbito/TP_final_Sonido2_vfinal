using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class FallaAscensor : MonoBehaviour
{
    [Header("Wwise Audio - Ascensor")]
    public AK.Wwise.Event eventoBajando;
    public AK.Wwise.Event eventoFalla;
    public AK.Wwise.Event eventoAlertaUI;

    [Header("Wwise Audio - Voces del Personaje")]
    [Tooltip("Voz cuando el personaje activa el ascensor")]
    public AK.Wwise.Event vozLlamarAscensor;
    [Tooltip("Voz de pánico cuando el ascensor falla")]
    public AK.Wwise.Event vozFallaAscensor;

    [Header("Interfaz de Usuario (UI)")]
    public GameObject objetoTextoAdvertencia;
    public float duracionAviso = 4f;

    [Header("Efectos de Cámara")]
    public Transform camaraJugador;
    public float duracionSacudida = 1.5f;
    public float magnitud = 0.15f;

    [Header("Tiempos")]
    public float tiempoDeBajada = 5f;

    private bool yaSeRompio = false;

    void Start()
    {
        if (objetoTextoAdvertencia != null)
        {
            objetoTextoAdvertencia.SetActive(false);
        }
    }

    public void Interactuar()
    {
        if (yaSeRompio) return;
        yaSeRompio = true;

        // Bloqueamos los monólogos aleatorios del jugador durante la secuencia del ascensor
        MonologoJugador.bloqueado = true;

        StartCoroutine(SecuenciaAscensorRoutine());
    }

    private IEnumerator SecuenciaAscensorRoutine()
    {
        // 1. Empieza a bajar y bloqueamos monólogos
        if (eventoBajando != null && eventoBajando.IsValid())
        {
            eventoBajando.Post(gameObject);
        }

        if (vozLlamarAscensor != null && vozLlamarAscensor.IsValid())
        {
            vozLlamarAscensor.Post(gameObject);
        }

        // 2. Esperamos los 5 segundos de trayecto
        yield return new WaitForSeconds(tiempoDeBajada);

        // 3. Falla, alarma y voz de pánico
        if (eventoFalla != null && eventoFalla.IsValid())
        {
            eventoFalla.Post(gameObject);
        }

        if (eventoAlertaUI != null && eventoAlertaUI.IsValid())
        {
            eventoAlertaUI.Post(gameObject);
        }

        if (vozFallaAscensor != null && vozFallaAscensor.IsValid())
        {
            vozFallaAscensor.Post(gameObject);
        }

        if (objetoTextoAdvertencia != null)
        {
            objetoTextoAdvertencia.SetActive(true);
            StartCoroutine(OcultarTextoRutina());
        }

        // 4. Temblor de cámara
        if (camaraJugador == null && Camera.main != null)
        {
            camaraJugador = Camera.main.transform;
        }

        if (camaraJugador != null)
        {
            yield return StartCoroutine(SacudirCamaraRoutine());
        }

        // ---> ¡AQUÍ ESTÁ LA SOLUCIÓN! <---
        // Una vez que termina toda la secuencia del ascensor, desbloqueamos los monólogos para que vuelvan a sonar con normalidad
        MonologoJugador.bloqueado = false;
    }

    // Método auxiliar de validación rápida para Wwise Event
    private bool vozFallaFallaValidar(AK.Wwise.Event evento)
    {
        return evento.IsValid();
    }

    private IEnumerator OcultarTextoRutina()
    {
        yield return new WaitForSeconds(duracionAviso);
        if (objetoTextoAdvertencia != null)
        {
            objetoTextoAdvertencia.SetActive(false);
        }
    }

    private IEnumerator SacudirCamaraRoutine()
    {
        Vector3 posicionOriginal = camaraJugador.localPosition;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionSacudida)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitud;
            float offsetY = Random.Range(-1f, 1f) * magnitud;

            camaraJugador.localPosition = posicionOriginal + new Vector3(offsetX, offsetY, 0f);

            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        camaraJugador.localPosition = posicionOriginal;
    }
}