
using UnityEngine;
using UnityEngine.AddressableAssets;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
public class ARObjectController : MonoBehaviour
{
    public ModelManager modelManager;
    public Transform modelHolder;
    
    public TextMeshProUGUI nombrePlatoText;
    public TextMeshProUGUI descripcionPlatoText;
    private GameObject currentInstance;
    public static ARObjectController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        LoadModel();
    }

    public void LoadModel()
    {
        nombrePlatoText.text = modelManager.currentModel.NombrePlato;
        if (CambiarIdioma.instancia.EsEspanol())
        {
            descripcionPlatoText.text = modelManager.currentModel.DescripcionPlato;
        }
        else
        {
            descripcionPlatoText.text = modelManager.currentModel.DescripcionIngles;
        }
            
        Debug.Log(modelManager.currentModel.name);
        if (modelManager.currentModel == null) return;

        if (currentInstance != null)
        {
            
            Addressables.ReleaseInstance(currentInstance);
            currentInstance = null;
        }


        modelManager.currentModel.ModeloPlato
            .InstantiateAsync(modelHolder)
            .Completed += OnModelLoaded;
    }
    private void OnModelLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            currentInstance = obj.Result;
        }
        
    }

    public void RefreshModel()
    {
        LoadModel();
    }

    public GameObject GetObjecto()
    {
        return currentInstance;
    }
    
}