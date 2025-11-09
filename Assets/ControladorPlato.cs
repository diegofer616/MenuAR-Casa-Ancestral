using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ControladorPlato : MonoBehaviour
{
    [SerializeField] PlatoSO plato;
    [SerializeField] TextMeshProUGUI descripcion;
    [SerializeField] TextMeshProUGUI titulo;
    [SerializeField] UnityEngine.UI.Image imagenPlato;
    void Start()
    {
        // Aseguramos título y texto al arrancar
        if (titulo != null && plato != null)
            titulo.text = plato.NombrePlato;
            imagenPlato.sprite = plato.ImagenPlato;

        UpdateTextForLocale();
    }

    private void OnEnable()
    {
        // Suscribirse para reaccionar si el usuario cambia el idioma en tiempo de ejecución
        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;

        // Actualizar al activarse (por si SelectedLocale ya está disponible)
        UpdateTextForLocale();
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
    }

    private void OnSelectedLocaleChanged(Locale locale)
    {
        // Cuando cambia el locale, actualizamos la UI
        UpdateTextForLocale();
    }

    private void UpdateTextForLocale()
    {
        if (plato == null || descripcion == null)
            return;

        var selected = LocalizationSettings.SelectedLocale;
        var locales = LocalizationSettings.AvailableLocales?.Locales;

        // Protección si aún no se ha inicializado la lista de locales
        if (selected != null && locales != null && locales.Count > 0)
        {
            // Tu tabla de localización tiene ingles en locales[0]
            if (selected == locales[0])
                descripcion.text = plato.DescripcionIngles;
            else
                descripcion.text = plato.DescripcionPlato;
        }
        else
        {
            // Fallback si no hay info de localización: usar español por defecto
            descripcion.text = plato.DescripcionPlato;
        }

        if (titulo != null && plato != null)
            titulo.text = plato.NombrePlato;
    }

    public void ActualizarIdioma(bool spanish)
    {
        // Método manual si lo necesitas: fuerza idioma según el bool
        if (descripcion == null || plato == null) return;

        descripcion.text = spanish ? plato.DescripcionPlato : plato.DescripcionIngles;
    }
}
