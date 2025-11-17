using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ARObjectController : MonoBehaviour
{
    public ModelManager modelManager;
    public Transform modelHolder;

    private GameObject currentInstance;
    public static ARObjectController instance;

    private AsyncOperationHandle<GameObject> _instantiateHandle;
    private bool _usingAddressableInstance = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Depuración: verificar que ModelManager y PlatoSO están correctos
        if (modelManager == null)
        {
            Debug.LogError("ARObjectController: modelManager es null");
        }
        else if (modelManager.currentModel == null)
        {
            Debug.LogWarning("ARObjectController: modelManager.currentModel es null en Start()");
        }
        else
        {
            var plato = modelManager.currentModel;
            Debug.Log($"ARObjectController: currentModel = {plato.NombrePlato}");
            Debug.Log($" - ModeloPlato (fallback) = {(plato.ModeloPlato != null ? plato.ModeloPlato.name : "null")}");
            Debug.Log($" - ModeloPlatoAddressable assigned = {(plato.ModeloPlatoAddressable != null ? "yes" : "no")}");
        }

        LoadModel();
    }

    public void LoadModel()
    {
        // Liberar instancia previa
        if (_usingAddressableInstance)
        {
            if (_instantiateHandle.IsValid()) Addressables.ReleaseInstance(_instantiateHandle);
            _instantiateHandle = default;
            _usingAddressableInstance = false;
            currentInstance = null;
        }
        else
        {
            if (currentInstance != null)
            {
                Destroy(currentInstance);
                currentInstance = null;
            }
        }

        if (modelManager == null || modelManager.currentModel == null) return;

        var plato = modelManager.currentModel;

        // 1) Si el PlatoSO contiene AssetReference, instanciamos via Addressables
        if (plato.ModeloPlatoAddressable != null && plato.ModeloPlatoAddressable.RuntimeKeyIsValid())
        {
            _instantiateHandle = plato.ModeloPlatoAddressable.InstantiateAsync(modelHolder);
            _instantiateHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    currentInstance = handle.Result;
                    _usingAddressableInstance = true;
                }
                else
                {
                    Debug.LogError("ARObjectController: fallo al instanciar addressable del plato.");
                }
            };
            return;
        }

        // 2) Fallback: prefab directo
        if (plato.ModeloPlato == null)
        {
            Debug.LogWarning("ARObjectController: PlatoSO.ModeloPlato es null.");
            return;
        }

        currentInstance = Instantiate(plato.ModeloPlato, modelHolder);
    }

    public void RefreshModel() => LoadModel();

    public GameObject GetObjecto() => currentInstance;
}