using UnityEngine;

public class ControladorPasos : MonoBehaviour
{
    [Header("Wwise Footsteps")]
    public AK.Wwise.Event eventoPasos;
    public AK.Wwise.Switch switchCeramica;
    public AK.Wwise.Switch switchLadrillo;

    [Header("Configuración de Movimiento")]
    public float intervaloPasos = 0.45f;
    private float temporizadorPasos = 0f;
    private Vector3 ultimaPosicion;

    void Start()
    {
        ultimaPosicion = transform.position;
        temporizadorPasos = intervaloPasos;
    }

    void Update()
    {
        // Medimos el desplazamiento real en el plano horizontal para evitar falsos positivos
        Vector3 posicionActualPlana = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 ultimaPosicionPlana = new Vector3(ultimaPosicion.x, 0f, ultimaPosicion.z);
        float velocidadReal = Vector3.Distance(posicionActualPlana, ultimaPosicionPlana) / Time.deltaTime;
        ultimaPosicion = transform.position;

        bool enMovimiento = velocidadReal > 0.1f;

        if (enMovimiento)
        {
            temporizadorPasos -= Time.deltaTime;

            if (temporizadorPasos <= 0f)
            {
                DetectarSuperficieYDispararPasos();
                temporizadorPasos = intervaloPasos;
            }
        }
        else
        {
            // Al detenernos, reseteamos el temporizador al intervalo completo de forma limpia
            temporizadorPasos = intervaloPasos;
        }
    }

    void DetectarSuperficieYDispararPasos()
    {
        RaycastHit hit;
        Vector3 origenRayo = transform.position + Vector3.up * 0.3f;

        // Aumentamos la distancia del rayo a 3.5f para que no falle en umbrales, uniones o desniveles
        if (Physics.Raycast(origenRayo, Vector3.down, out hit, 3.5f))
        {
            string tagSuelo = hit.collider.tag;

            // Actualizamos el Switch de Wwise según el piso detectado
            if (tagSuelo == "Ceramica")
            {
                if (switchCeramica != null && switchCeramica.IsValid())
                    switchCeramica.SetValue(gameObject);
            }
            else if (tagSuelo == "Ladrillo")
            {
                if (switchLadrillo != null && switchLadrillo.IsValid())
                    switchLadrillo.SetValue(gameObject);
            }

            // Disparamos el evento de Wwise de manera segura
            if (eventoPasos != null && eventoPasos.IsValid())
            {
                eventoPasos.Post(gameObject);
            }
        }
    }
}