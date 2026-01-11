
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
