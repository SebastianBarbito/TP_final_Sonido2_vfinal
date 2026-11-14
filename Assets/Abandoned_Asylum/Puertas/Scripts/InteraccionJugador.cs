using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InteraccionJugador : MonoBehaviour
{
    [Header("Configuración de Raycast")]
    public Transform camaraJugador;
    public float distanciaInteraccion = 3f;

    void Update()
    {
        bool presionoE = false;
        bool presionoQ = false;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame) presionoE = true;
            if (Keyboard.current.qKey.wasPressedThisFrame) presionoQ = true;
        }
#else
        if (Input.GetKeyDown(KeyCode.E)) presionoE = true;
        if (Input.GetKeyDown(KeyCode.Q)) presionoQ = true;
#endif

        // Si se presiona alguna de las dos teclas y tenemos la cámara asignada
        if ((presionoE || presionoQ) && camaraJugador != null)
        {
            Ray rayo = new Ray(camaraJugador.position, camaraJugador.forward);
            if (Physics.Raycast(rayo, out RaycastHit impacto, distanciaInteraccion))
            {
                // Si presionamos Q -> EXCLUSIVO para el lavamanos
                if (presionoQ)
                {
                    LavamanosInteractivo lavamanos = impacto.collider.GetComponentInParent<LavamanosInteractivo>();
                    if (lavamanos != null)
                    {
                        lavamanos.Interactuar();
                        return;
                    }
                }

                // Si presionamos E -> Para puertas y ascensor
                if (presionoE)
                {
                    // 1. ¿Es una puerta?
                    PuertaInteractiva puerta = impacto.collider.GetComponentInParent<PuertaInteractiva>();
                    if (puerta != null)
                    {
                        puerta.AlternarPuerta();
                        return;
                    }

                    // 2. ¿Es el ascensor?
                    FallaAscensor ascensor = impacto.collider.GetComponentInParent<FallaAscensor>();
                    if (ascensor != null)
                    {
                        ascensor.Interactuar();
                        return;
                    }
                }
            }
        }
    }
}