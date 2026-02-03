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
    [SerializeField] GameObject botonIniciar;
    [SerializeField] ModelManager modelmanager;
    [SerializeField] TextMeshProUGUI descripcion;
    [SerializeField] TextMeshProUGUI titulo;
    [SerializeField] TextMeshProUGUI precio;
    [SerializeField] GameObject imagenIU;
    [SerializeField] private UnityEngine.UI.Image imagen;
    [SerializeField] List <GameObject> menues;
    [SerializeField] int indiceMenu =0;
    [SerializeField] GameObject prefab;
    [SerializeField] List <GameObject> categorias;
    public List<PlatoSO> platosCargados = new List<PlatoSO>();
    //private AsyncOperationHandle<Sprite> imagenHandle;
 
    void Start()
    {
        imagen = imagenIU.GetComponent<UnityEngine.UI.Image>();
        InstanciarPlatos();
        if (VerificarPrimeraVez.instance.EsPrimeraVez())
        {
            menues[0].SetActive(true);
            menues[1].SetActive(false);
            VerificarPrimeraVez.instance.SetPrimeraVez();
        }
        else
        {
            menues[0].SetActive(false);
            indiceMenu = 1;
        }
    }
    private void OnEnable()
    {
        ControladorPlato.OnPlatoSeleccionado += PlatoSeleccionado;
    }
    private void OnDisable()
    {
        ControladorPlato.OnPlatoSeleccionado -= PlatoSeleccionado;
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
        }
        IniciarBotonMenu();
    }
    public void IniciarBotonMenu() 
    {
        botonIniciar.GetComponent<Button>().interactable = true;
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
        descripcion.text = CambiarIdioma.instancia.EsEspanol() ? plato.DescripcionPlato : plato.DescripcionIngles;
        titulo.text = plato.NombrePlato;
        modelmanager.SetModel(plato);
        precio.text = plato.precioPlato;

    }
    

}
