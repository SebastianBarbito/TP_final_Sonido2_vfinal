using UnityEngine;

public class PuertaInteractiva : MonoBehaviour
{
    [Header("Configuración de Apertura")]
    [Tooltip("Ángulo de apertura (ej. 90 o -90 según hacia dónde abra)")]
    public float anguloApertura = 90f;
    public float velocidadApertura = 4f;

    [Header("Puerta Doble (Opcional - para la Morgue)")]
    [Tooltip("Si es puerta doble, arrastra aquí la otra hoja para que se abran juntas")]
    public PuertaInteractiva puertaHermana;

    [Header("Audio Wwise (Opcional - SFX interactivo)")]
    public AK.Wwise.Event sonidoAbrir;
    public AK.Wwise.Event sonidoCerrar;

    private bool estaAbierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = rotacionCerrada * Quaternion.Euler(0f, anguloApertura, 0f);
    }

    void Update()
    {
        Quaternion objetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, objetivo, Time.deltaTime * velocidadApertura);
    }

    public void AlternarPuerta(bool propagarAHermana = true)
    {
        estaAbierta = !estaAbierta;

        if (estaAbierta)
        {
            if (sonidoAbrir != null && sonidoAbrir.IsValid())
                sonidoAbrir.Post(gameObject);
        }
        else
        {
            if (sonidoCerrar != null && sonidoCerrar.IsValid())
                sonidoCerrar.Post(gameObject);
        }

        // Si tiene puerta hermana (puerta doble de la morgue), la abre/cierra al mismo tiempo
        if (propagarAHermana && puertaHermana != null)
        {
            puertaHermana.SincronizarConHermana(estaAbierta);
        }
    }

    public void SincronizarConHermana(bool estado)
    {
        estaAbierta = estado;
    }
}