using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    
    
    
    // Update is called once per frame
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
    public void SetIndiceMenu(int indice)
    {
        indiceMenu = indice;
    }
    public void AbrirEnlace(string hola) 
    {
        Application.OpenURL("wa.me/59173336823/?text+hola+chau");
    }

    public void MostrarDescripcion(PlatoSO plato)
    {
        imagen.sprite = plato.ImagenPlato;
        if(LocalizationSettings.SelectedLocale.Identifier.Code == "en")
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
    
}
