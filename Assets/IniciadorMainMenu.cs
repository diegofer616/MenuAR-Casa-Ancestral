using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.AddressableAssets;
using System.Runtime.CompilerServices;
using System;
using System.Reflection;
using TMPro;
public class IniciadorMainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI statusText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CheckDownloadStatus());
    }
    private IEnumerator CheckDownloadStatus()
    {
        AsyncOperationHandle<IResourceLocator> handle = default;
        AsyncOperationHandle<long> downloadSizeHandle = default;
        try
        {
            downloadSizeHandle = Addressables.GetDownloadSizeAsync("MenuPrincipal");
        }
        catch (System.Exception e)
        {
            statusText.text = "Error checking download size.";
            Debug.Log("Algo malo paso");
        }

        yield return downloadSizeHandle;

        if (downloadSizeHandle.Status == AsyncOperationStatus.Succeeded)
        {
            if (downloadSizeHandle.Result > 0)
            {
                Addressables.Release(downloadSizeHandle);
                InitialDowload();
                statusText.text = "Downloading game data...";
                Debug.Log("se inicia la descargar");
            }
            else
            {
                Addressables.Release(downloadSizeHandle);
                statusText.text = "No download needed.";
                Debug.Log("No download needed for 'mainmenu' label.");
                StartCoroutine(InstantiateScene());
            }
        }
        else
        {
            statusText.text = "Error checking download size.";
            Debug.Log("algo fallo viendo descargas");
        }
    }
        private void InitialDowload()
        {
            StartCoroutine(DowloadGameData());
    }
        
        private IEnumerator DowloadGameData()
        {
            AsyncOperationHandle downloadHandle = default;
        try
        {
            downloadHandle = Addressables.DownloadDependenciesAsync("MenuPrincipal");
           downloadHandle.Completed += OndownloadComplete;
        }
        catch (Exception e)
        {
            statusText.text = "Error starting download." + e;
            Debug.Log("ahlgo malo paso");
        }
        while (!downloadHandle.IsDone)
        {
            Debug.Log($"Download progress: {downloadHandle.PercentComplete * 100f}%");
            yield return null;
        }
        }

    private void OndownloadComplete(AsyncOperationHandle handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            statusText.text = "Download complete.";
            Debug.Log("Descarga completa");
            Addressables.Release(handle);
            StartCoroutine(InstantiateScene(1f));
        }
        else
        {
            statusText.text = "Download failed.";
            Debug.Log("La descarga fallo");
            Addressables.Release(handle);
        }
    }
    private IEnumerator InstantiateScene(float delay = 0f)
    {
        if (Mathf.Approximately(delay, 0f))
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        AsyncOperationHandle<SceneInstance> loadHandle = default;
        try
        {
            statusText.text = "Loading Main Menu...";
            loadHandle = Addressables.LoadSceneAsync("MenuPrincipal");
        }
        catch (Exception e)
        {
            statusText.text = "Error loading Main Menu." + e;
            Debug.Log("Algo malo paso");
            yield break;
        }
        yield return loadHandle;
        if(loadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            statusText.text = "Main Menu loaded.";
            Debug.Log("MainMenu scene loaded successfully.");
        }

       
    }
}