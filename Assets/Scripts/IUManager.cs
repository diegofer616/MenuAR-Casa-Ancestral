using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
public class IUManager : MonoBehaviour
{
    [SerializeField] ModelManager modelmanager;
    [SerializeField] TextMeshProUGUI descripcion;
    [SerializeField] TextMeshProUGUI titulo;
    [SerializeField] TextMeshProUGUI precio;
    [SerializeField] GameObject imagenObject;
    [SerializeField] private UnityEngine.UI.Image imagen;
    [SerializeField] List <GameObject> menues;
    [SerializeField] int indiceMenu =0;
    [SerializeField] GameObject prefab;
    [SerializeField] List <GameObject> categorias;
    public List<PlatoSO> platosCargados = new List<PlatoSO>();
    private AsyncOperationHandle<Sprite> imagenHandle;
    private bool imagenCargada = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstanciarPlatos();
        imagen =imagenObject.GetComponent<UnityEngine.UI.Image>();
        if (VerificarPrimeraVez.instance.EsPrimeraVez())
        {
            menues[0].SetActive(true);
            menues[1].SetActive(false);
        }
        else
        {
            menues[0].SetActive(false);
            indiceMenu = 1;
        }
        VerificarPrimeraVez.instance.SetPrimeraVez();
    }
    private void OnEnable()
    {
        ControladorPlato.OnPlatoSeleccionado += PlatoSeleccionado;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(indiceMenu)
            {
                case 0:
                    Application.Quit();
                    break;
                case 1:
                    menues[1].SetActive(false);
                    menues[0].SetActive(true);
                    indiceMenu = 0;
                    break;
                case 2:
                    menues[2].SetActive(false);
                    menues[1].SetActive(true);
                    indiceMenu = 1;
                    break;
            }
        }
    }
    void PlatoSeleccionado(PlatoSO plato) 
    {
        
        SetIndiceMenu(2);
        MostrarDescripcion(plato);
        menues[2].SetActive(true);
        menues[1].SetActive(false);
    }
    public void SetIndiceMenu(int indice)
    {
        indiceMenu = indice;
    }
    public void InstanciarPlatos()
    {
        Addressables.LoadAssetsAsync<PlatoSO>("plate", plate =>
        {
           platosCargados.Add(plate);
        }
        ).Completed += handle =>
        {
           if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
           {
               Debug.Log("Platos cargados: " + platosCargados.Count);
                MostrarPlatosCanvas();
            }
        };
        
    }
    public void MostrarPlatosCanvas()
    {
        foreach (var plato in platosCargados)
        {
            GameObject newPlato = Instantiate(prefab, this.transform.position, Quaternion.identity);

            newPlato.transform.SetParent(categorias[plato.categoriaPlato].transform, false);
            newPlato.GetComponent<ControladorPlato>().SetPlato(plato);
           // newPlato.GetComponent<ControladorPlato>().Init(plato);
        }
    }

    public void MostrarDescripcion(PlatoSO plato)
    {
        if (!plato.ImagenPlato.OperationHandle.IsValid())
        {
            plato.ImagenPlato.LoadAssetAsync<Sprite>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    imagen.sprite = handle.Result;
                }
                else
                {
                    Debug.LogWarning("No se pudo cargar la imagen del plato");
                }
            };
        }
        else
        {
            // Ya estaba cargado, solo usamos el resultado
            imagen.sprite = plato.ImagenPlato.OperationHandle.Result as Sprite;
        }
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
        {
            descripcion.text = plato.DescripcionIngles;
        }
        else
        {
            descripcion.text = plato.DescripcionPlato;
        }
            
        titulo.text = plato.NombrePlato;
        modelmanager.SetModel(plato);
        precio.text = plato.precioPlato;

    }
    public void LiberarImagenPlato(PlatoSO plato)
    {
        if (plato.ImagenPlato.OperationHandle.IsValid())
        {
            plato.ImagenPlato.ReleaseAsset();
        }
    }

}
