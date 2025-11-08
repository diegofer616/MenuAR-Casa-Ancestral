using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField] public LoadingScreenUI loadingScreenUI;
    [SerializeField] public SceneLoadRequest _pendentRequest;
    
    // Start is called before the first frame update


    // Update is called once per frame
    
    public void OnLoadMenuRequest(SceneLoadRequest request)
    {
        if (IsSceneAlredyLoaded(request.escena)== false)
        {
            SceneManager.LoadScene(request.escena.nombreEscena);
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
            if(request.pantallaDeCarga)
            {
                _pendentRequest = request;
                loadingScreenUI.ToggleScreen(true);
                //Debug.Log("Request para cargar escena: " + request.escena.nombreEscena);
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
        Scene escenaCargada = SceneManager.GetSceneByName(escena.nombreEscena);
        if (escenaCargada != null && escenaCargada.isLoaded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator ProcessLevelLoading(SceneLoadRequest request)
    {
        if(request.escena != null)
        {
            var currentLoadedLevel = SceneManager.GetActiveScene();

            
            yield return new WaitForSeconds(2f);
            
            AsyncOperation loadSceneProcess = SceneManager.LoadSceneAsync(request.escena.nombreEscena, LoadSceneMode.Additive);
            while (!loadSceneProcess.isDone)
            {
                
                yield return null;
            }
            SceneManager.UnloadSceneAsync(currentLoadedLevel);
            ActiveLevel(request);
        }
        
    }
    private void ActiveLevel(SceneLoadRequest request)
    {
        Debug.Log("Activando escena: " + request);
        var escenaCargada = SceneManager.GetSceneByName(request.escena.nombreEscena);
        
        if(request.pantallaDeCarga)
        {
            loadingScreenUI.ToggleScreen(false);
           
        }
        SceneManager.SetActiveScene(escenaCargada);
        _pendentRequest = null;
    }
}