using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class CambiarIdioma : MonoBehaviour
{
    public BoolGameEvent LanguagueSpanish;
    int idiomaSeleccionado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    public void ChangeLanguage(bool spanish) 
    {

        LanguagueSpanish.Raise(spanish);
    }
}
