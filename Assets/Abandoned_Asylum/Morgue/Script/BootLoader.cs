using UnityEngine;
using UnityEngine.SceneManagement;

public class BootLoader : MonoBehaviour
{
    void Start()
    {
        // Opcional: Asegurar la carga explícita de los bancos principales
        AkBankManager.LoadBank("Init", false, false);
        AkBankManager.LoadBank("Main_Soundbanks", false, false);

        // Cargar la escena de la morgue de manera segura
        SceneManager.LoadScene("Abandoned_Asylum/Show"); // Reemplaza con el nombre exacto de tu escena de la morgue
    }
}