using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        for (int i = 0; i <= escenas.Length - 1; i++)
        {
            SceneSO escenaACargar = escenas[i];

            if (SceneManager.GetSceneByName(escenaACargar.name).isLoaded == false)
            {
                var operacionCargado = SceneManager.LoadSceneAsync(escenaACargar.nombreEscena, LoadSceneMode.Additive);
                while (!operacionCargado.isDone)
                {
                    yield return null;
                }
            }
            if(cargandoEscena != null)
            cargandoEscena.Invoke();

        }
    }
}
