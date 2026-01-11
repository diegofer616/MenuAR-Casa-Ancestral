using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
public class ControladorPlato : MonoBehaviour
{
    [SerializeField] PlatoSO plato;
    [SerializeField] TextMeshProUGUI descripcion;
    [SerializeField] TextMeshProUGUI titulo;
    [SerializeField] UnityEngine.UI.Image imagenPlato;
    [SerializeField] Button boton;
    public static event Action<PlatoSO> OnPlatoSeleccionado;

    void NotificarClick()
    {
        OnPlatoSeleccionado?.Invoke(plato);
    }
    public void SetPlato(PlatoSO nuevoPlato)
    {
        plato = nuevoPlato;
        titulo.text = plato.NombrePlato;
        if (plato.ImagenPlato != null)
        {
            if (!plato.ImagenPlato.OperationHandle.IsValid())
            {
                plato.ImagenPlato.LoadAssetAsync<Sprite>().Completed += handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        imagenPlato.sprite = handle.Result;
                    }
                    else
                    {
                        Debug.LogError("No se pudo cargar el sprite del plato: " + plato.NombrePlato);
                    }
                };
            }
            else
            {
                // Ya estaba cargado solo usamos el resultado
                imagenPlato.sprite = plato.ImagenPlato.OperationHandle.Result as Sprite;
            }
        }
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(NotificarClick);
        ActualizarIdioma();
    }
    

    private void OnEnable()
    {
        ActualizarIdioma();
    }
    public void ActualizarIdioma()
    {
        descripcion.text = CambiarIdioma.instancia.EsEspanol() ? plato.DescripcionPlato : plato.DescripcionIngles;
    }

}
