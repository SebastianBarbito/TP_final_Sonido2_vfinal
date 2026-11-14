using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZonaHorizontalTrigger : MonoBehaviour
{
    public enum TipoZona { Pasillo, SalaMaquinas, PasilloSuspenso, Morgue }

    [Header("Selecciona qué zona es este Collider")]
    public TipoZona zona;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || !other.CompareTag("Player")) return;
        if (ControladorPasilloMaquinas.Instancia == null) return;

        switch (zona)
        {
            case TipoZona.Pasillo:
                ControladorPasilloMaquinas.Instancia.EntrarPasillo();
                break;
            case TipoZona.SalaMaquinas:
                ControladorPasilloMaquinas.Instancia.EntrarSalaMaquinas();
                break;
            case TipoZona.PasilloSuspenso:
                ControladorPasilloMaquinas.Instancia.EntrarPasilloSuspenso();
                break;
            case TipoZona.Morgue:
                ControladorPasilloMaquinas.Instancia.VolverAMorgue();
                break;
        }
    }
}