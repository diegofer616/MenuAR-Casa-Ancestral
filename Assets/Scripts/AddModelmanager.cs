
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public class ARObjectController : MonoBehaviour
{
    public ModelManager modelManager;
    public Transform modelHolder;

    private GameObject currentInstance;
    public static ARObjectController instance;

    private void Awake()
    {
        // Evitar persistir objetos que dependan de la escena AR para no dejar referencias a cámaras destruidas.
        if (instance == null)
        {
            instance = this;
            // Quitado DontDestroyOnLoad(gameObject);
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
        if (modelManager.currentModel == null) return;

        if (currentInstance != null)
        {
            Addressables.ReleaseInstance(currentInstance);
            currentInstance = null;
        }


        modelManager.currentModel.ModeloPlato
            .InstantiateAsync(modelHolder)
            .Completed += OnModelLoaded;
        
        //currentInstance.SetActive(false);
    }
    private void OnModelLoaded(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            currentInstance = obj.Result;
            //currentInstance.transform.localPosition = Vector3.zero;
            //currentInstance.transform.localRotation = Quaternion.identity;
            //currentInstance.transform.localScale = Vector3.one;
            //currentInstance.SetActive(true);
        }
        else
        {
            Debug.LogError("Failed to load model.");
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
