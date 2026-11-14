using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorCarga : MonoBehaviour
{
    void Start()
    {
        // Forzar la carga explícita de los bancos de Wwise al arrancar el ejecutable
        AkBankManager.LoadBank("Init", false, false);
        AkBankManager.LoadBank("Main_Soundbanks", false, false);

        StartCoroutine(CargarEscenaAsincrona());
    }

    IEnumerator CargarEscenaAsincrona()
    {
        yield return new WaitForSeconds(0.5f); // Breve respiro para que Wwise procese los bancos
        AsyncOperation operacion = SceneManager.LoadSceneAsync("Abandoned_Asylum/Show");

        while (!operacion.isDone)
        {
            yield return null;
        }
    }
}