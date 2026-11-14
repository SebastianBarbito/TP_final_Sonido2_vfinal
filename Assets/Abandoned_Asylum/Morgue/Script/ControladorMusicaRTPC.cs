using UnityEngine;

public class ControladorMusicaRTPC : MonoBehaviour
{
    public static ControladorMusicaRTPC Instancia;

    [Header("Configuración de Wwise")]
    public AK.Wwise.RTPC rtpcMusica;

    [Header("Valores exactos por Capa (0 a 100)")]
    [Tooltip("Capa 1 (Base), Capa 2, Capa 3, Capa 4")]
    public float[] valoresPorCapa = new float[] { 0f, 35f, 70f, 100f };

    [Header("Estado Actual (Solo Lectura)")]
    [SerializeField] private int capaActual = 1;
    [SerializeField] private float valorActualEnviado = 0f;

    private float tiempoUltimoCambio = -10f;
    private const float COOLDOWN_CAMBIO = 0.5f; // Evita rebotes en bordes de colliders

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        // Arranca siempre en Capa 1 (valor 0)
        ActivarHastaCapa(1, forzar: true);
    }

    /// <summary>
    /// Llama a este método pasando 1, 2, 3 o 4 según cuántas capas quieres que suenen.
    /// </summary>
    public void ActivarHastaCapa(int numeroDeCapas, bool forzar = false)
    {
        if (rtpcMusica == null) return;

        int indice = Mathf.Clamp(numeroDeCapas - 1, 0, valoresPorCapa.Length - 1);

        // Si ya estamos en esa capa o acaba de ocurrir un cambio hace menos de 0.5s, ignorar
        if (!forzar)
        {
            if (numeroDeCapas == capaActual) return;
            if (Time.time - tiempoUltimoCambio < COOLDOWN_CAMBIO) return;
        }

        capaActual = numeroDeCapas;
        valorActualEnviado = valoresPorCapa[indice];
        tiempoUltimoCambio = Time.time;

        // Enviamos el valor UNA SOLA VEZ; Wwise hace el fade suave con Slew Rate
        rtpcMusica.SetGlobalValue(valorActualEnviado);
    }
}