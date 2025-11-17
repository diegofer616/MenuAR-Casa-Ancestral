using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class IniciadorEscena : MonoBehaviour
{
    [SerializeField] SceneSO[] escenas;
    [SerializeField] public UnityEvent cargandoEscena;

    void Start()
    {
        StartCoroutine(CargandoEscena());
    }

    IEnumerator CargandoEscena()
    {
        for (int i = 0; i < escenas.Length; i++)
        {
            SceneSO escenaACargar = escenas[i];

            // Corregido: usar nombreEscena (propiedad) en lugar de .name (nombre del ScriptableObject)
            var sceneByName = SceneManager.GetSceneByName(escenaACargar.nombreEscena);
            if (!sceneByName.IsValid() || sceneByName.isLoaded == false)
            {
                var operacionCargado = Addressables.LoadSceneAsync(escenaACargar.nombreEscena, LoadSceneMode.Additive);
                yield return operacionCargado;

                if (operacionCargado.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Falló carga addressable de escena: {escenaACargar.nombreEscena}");
                }
            }

            cargandoEscena?.Invoke();
        }
    }
}