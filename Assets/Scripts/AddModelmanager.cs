
using UnityEngine;

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
            Destroy(currentInstance);

        currentInstance = Instantiate(
            modelManager.currentModel.ModeloPlato,
            modelHolder
        );
        //currentInstance.SetActive(false);
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
