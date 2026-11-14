using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZonaMusicaRTPC : MonoBehaviour
{
    [Header("Cuántas capas suenan en esta habitación")]
    [Range(1, 4)]
    public int cantidadDeCapas = 1;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica que sea el Player (y evita dispararse con triggers secundarios)
        if (!other.isTrigger && other.CompareTag("Player"))
        {
            if (ControladorMusicaRTPC.Instancia != null)
            {
                ControladorMusicaRTPC.Instancia.ActivarHastaCapa(cantidadDeCapas);
            }
        }
    }
}