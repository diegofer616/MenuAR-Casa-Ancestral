using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class CambiarIdioma : MonoBehaviour
{
    
    int idiomaSeleccionado;
    public static CambiarIdioma instancia;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;

        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        CargarDatos();
        Invoke("CargarLocal", 0.1f);
    }
    private void CargarLocal()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[idiomaSeleccionado];
    }
    public void Cambiar(int indice)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[indice];
        idiomaSeleccionado = indice;
        GuardarDatos();
    }
    void GuardarDatos()
    {
               PlayerPrefs.SetInt("idioma", idiomaSeleccionado);
    }
    void CargarDatos()
    {
        if (PlayerPrefs.HasKey("idioma"))
        {
            idiomaSeleccionado = PlayerPrefs.GetInt("idioma");
            
        }
    }
    public bool EsEspanol()
    {
        return idiomaSeleccionado == 1 ;
    }
}
