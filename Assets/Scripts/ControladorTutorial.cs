using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ControladorTutorial : MonoBehaviour
{
    [SerializeField] GameObject panelTutorial;
    void Start()
    {
        StartCoroutine(EsperaTutorial());
    }
    public IEnumerator EsperaTutorial()
    {
        yield return new WaitForSeconds(5f);
        panelTutorial.SetActive(false);
    }
    public void ApagarTutorial()
    {
        panelTutorial.SetActive(false);
    }
}
