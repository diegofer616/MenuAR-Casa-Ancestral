
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] public LoadingScreenUI loadingScreenUI;
    [SerializeField] public SceneLoadRequest _pendentRequest;

    // Mapa de escenas cargadas (nombreEscena -> handle)
    private Dictionary<string, AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>> _loadedSceneHandles = new Dictionary<string, AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>();

    // Start is called before the first frame update


    // Update is called once per frame

    public void OnLoadMenuRequest(SceneLoadRequest request)
    {
        if (IsSceneAlredyLoaded(request.escena) == false)
        {
            StartCoroutine(ProcessMenuLoading(request));
        }
    }

    public void OnLoadLevelRequest(SceneLoadRequest request)
    {
        if (IsSceneAlredyLoaded(request.escena))
        {
            ActiveLevel(request);
        }
        else
        {
            if (request.pantallaDeCarga)
            {
                _pendentRequest = request;
                loadingScreenUI.ToggleScreen(true);
            }
            else
            {
                StartCoroutine(ProcessLevelLoading(request));
            }
        }
    }

    public void OnloadingScreenToggled(bool enabled)
    {
        if (_pendentRequest != null && enabled == true)
        {
            Debug.Log("comunicacion hecha");
            StartCoroutine(ProcessLevelLoading(_pendentRequest));
        }
    }

    private bool IsSceneAlredyLoaded(SceneSO escena)
    {
        // Usar nombreEscena (propiedad del ScriptableObject)
        Scene escenaCargada = SceneManager.GetSceneByName(escena.nombreEscena);
        return escenaCargada.IsValid() && escenaCargada.isLoaded;
    }

    private IEnumerator ProcessMenuLoading(SceneLoadRequest request)
    {
        if (request.escena == null) yield break;

        // Cargar la escena principal usando Addressables en modo Single
        var loadHandle = Addressables.LoadSceneAsync(request.escena.nombreEscena, LoadSceneMode.Single);
        yield return loadHandle;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Guardamos handle para futuras descargas si fuera necesario
            _loadedSceneHandles[request.escena.nombreEscena] = loadHandle;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(request.escena.nombreEscena));
            Debug.Log("Main menu loaded via Addressables.");
        }
        else
        {
            Debug.LogError($"Failed loading main menu: {request.escena.nombreEscena}");
        }
    }

    private IEnumerator ProcessLevelLoading(SceneLoadRequest request)
    {
        if (request.escena == null) yield break;

        var currentLoadedLevel = SceneManager.GetActiveScene();

        // (Opcional) pequeña espera UX
        yield return new WaitForSeconds(0.5f);

        // Cargar la nueva escena de forma aditiva vía Addressables
        var loadHandle = Addressables.LoadSceneAsync(request.escena.nombreEscena, LoadSceneMode.Additive);
        yield return loadHandle;

        if (loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            // Guardamos handle
            _loadedSceneHandles[request.escena.nombreEscena] = loadHandle;

            // Descargamos la escena previa:
            string prevName = currentLoadedLevel.name;
            if (!string.IsNullOrEmpty(prevName) && prevName != request.escena.nombreEscena)
            {
                // Si la previa fue cargada con Addressables y tenemos su handle, usamos Addressables.UnloadSceneAsync
                if (_loadedSceneHandles.TryGetValue(prevName, out var prevHandle))
                {
                    var unloadOp = Addressables.UnloadSceneAsync(prevHandle);
                    unloadOp.Completed += (op) =>
                    {
                        // Liberar la referencia al handle después de unload
                        Addressables.Release(prevHandle);
                    };
                    _loadedSceneHandles.Remove(prevName);
                }
                else
                {
                    // Fallback a SceneManager.UnloadSceneAsync si la escena previa no fue cargada con Addressables
                    SceneManager.UnloadSceneAsync(prevName);
                }
            }

            ActiveLevel(request);
        }
        else
        {
            Debug.LogError($"Error cargando escena addressable: {request.escena.nombreEscena}");
        }
    }

    private void ActiveLevel(SceneLoadRequest request)
    {
        Debug.Log("Activando escena: " + request);
        var escenaCargada = SceneManager.GetSceneByName(request.escena.nombreEscena);

        if (request.pantallaDeCarga)
        {
            loadingScreenUI.ToggleScreen(false);
        }

        if (escenaCargada.IsValid())
        {
            SceneManager.SetActiveScene(escenaCargada);
        }

        _pendentRequest = null;
    }
}