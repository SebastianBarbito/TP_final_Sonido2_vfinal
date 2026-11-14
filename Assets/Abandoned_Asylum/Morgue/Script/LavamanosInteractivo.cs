using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LavamanosInteractivo : MonoBehaviour
{
    [Header("Wwise Audio - Lavamanos")]
    [Tooltip("El evento que inicia el bucle del agua")]
    public AK.Wwise.Event eventoPlay;
    [Tooltip("El evento que detiene el bucle del agua")]
    public AK.Wwise.Event eventoStop;

    private bool estaActivo = false;

    public void Interactuar()
    {
        // Alternamos el estado lógico
        estaActivo = !estaActivo;

        if (estaActivo)
        {
            // Por seguridad, detenemos cualquier rastro anterior en este objeto antes de abrirlo
            if (eventoStop != null && eventoStop.IsValid())
            {
                eventoStop.Post(gameObject);
            }

            // Encendemos el agua en bucle limpio
            if (eventoPlay != null && eventoPlay.IsValid())
            {
                eventoPlay.Post(gameObject);
            }
        }
        else
        {
            // Apagamos el agua por completo
            if (eventoStop != null && eventoStop.IsValid())
            {
                eventoStop.Post(gameObject);
            }
        }
    }
}