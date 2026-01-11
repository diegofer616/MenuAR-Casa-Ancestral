using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ControladorTutorial : MonoBehaviour
{
    [SerializeField] GameObject panelTutorial;
    [SerializeField] VideoPlayer video;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        video.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
