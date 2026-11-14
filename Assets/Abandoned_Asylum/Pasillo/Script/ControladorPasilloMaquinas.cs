using UnityEngine;

public class ControladorPasilloMaquinas : MonoBehaviour
{
    public static ControladorPasilloMaquinas Instancia;

    [Header("Eventos de Wwise")]
    public AK.Wwise.Event eventoPasilloMaquinas;
    public AK.Wwise.Event eventoMorgue;

    [Header("States Horizontales (3 Zonas)")]
    public AK.Wwise.State estadoPasillo;
    public AK.Wwise.State estadoSalaMaquinas;
    public AK.Wwise.State estadoPasilloSuspenso;

    private bool sonandoPasilloMaquinas = false;
    private bool sonandoMorgue = false;
    private string estadoActual = "";
    private float tiempoUltimoCambio = -10f;

    void Awake()
    {
        Instancia = this;
    }

    void Start()
    {
        EntrarPasillo();
    }

    public void EntrarPasillo()
    {
        ActivarSistemaPasilloMaquinas(estadoPasillo);
    }

    public void EntrarSalaMaquinas()
    {
        ActivarSistemaPasilloMaquinas(estadoSalaMaquinas);
    }

    public void EntrarPasilloSuspenso()
    {
        ActivarSistemaPasilloMaquinas(estadoPasilloSuspenso);
    }

    private void ActivarSistemaPasilloMaquinas(AK.Wwise.State nuevoEstado)
    {
        if (nuevoEstado == null || !nuevoEstado.IsValid()) return;
        if (Time.time - tiempoUltimoCambio < 0.5f) return;

        if (sonandoMorgue && eventoMorgue != null)
        {
            eventoMorgue.Stop(gameObject, 1000);
            sonandoMorgue = false;
        }

        if (estadoActual != nuevoEstado.Name)
        {
            nuevoEstado.SetValue();
            estadoActual = nuevoEstado.Name;
            tiempoUltimoCambio = Time.time;
        }

        if (!sonandoPasilloMaquinas && eventoPasilloMaquinas != null)
        {
            eventoPasilloMaquinas.Post(gameObject);
            sonandoPasilloMaquinas = true;
        }
    }

    public void VolverAMorgue()
    {
        if (Time.time - tiempoUltimoCambio < 0.5f) return;

        if (sonandoPasilloMaquinas && eventoPasilloMaquinas != null)
        {
            eventoPasilloMaquinas.Stop(gameObject, 1000);
            sonandoPasilloMaquinas = false;
            estadoActual = "";
        }

        if (!sonandoMorgue && eventoMorgue != null)
        {
            eventoMorgue.Post(gameObject);
            sonandoMorgue = true;
            tiempoUltimoCambio = Time.time;
        }
    }
}