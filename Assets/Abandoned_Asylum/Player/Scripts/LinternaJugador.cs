using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LinternaJugador : MonoBehaviour
{
    [Header("Componente de Luz")]
    [Tooltip("Arrastra aquí el Spotlight o Point Light de la linterna (puede ser hijo de la cámara)")]
    public Light luzLinterna;

    [Header("Wwise Audio")]
    public AK.Wwise.Event eventoPrender;
    public AK.Wwise.Event eventoApagar;

    private bool linternaEncendida = false;

    void Start()
    {
        // Aseguramos que la linterna arranque apagada
        if (luzLinterna != null)
        {
            luzLinterna.enabled = false;
        }
    }

    void Update()
    {
        bool presionoF = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
            presionoF = true;
#else
        if (Input.GetKeyDown(KeyCode.F))
            presionoF = true;
#endif

        if (presionoF)
        {
            AlternarLinterna();
        }
    }

    void AlternarLinterna()
    {
        linternaEncendida = !linternaEncendida;

        if (luzLinterna != null)
        {
            luzLinterna.enabled = linternaEncendida;
        }

        if (linternaEncendida)
        {
            // Reproducir sonido de encendido
            if (eventoPrender != null && eventoPrender.IsValid())
            {
                eventoPrender.Post(gameObject);
            }
        }
        else
        {
            // Reproducir sonido de apagado
            if (eventoApagar != null && eventoApagar.IsValid())
            {
                eventoApagar.Post(gameObject);
            }
        }
    }
}