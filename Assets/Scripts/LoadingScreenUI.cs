using ScriptableObjectArchitecture;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(Animator))]
public class LoadingScreenUI : MonoBehaviour
{
    public BoolGameEvent loadingScreenToggled;
    private Animator _animator;
    [SerializeField] GameObject loadingScreeen;
    [SerializeField] VideoPlayer video;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void ToggleScreen(bool enable)
    {
        //Debug.Log("Toggle Loading Screen: " + enable);
        if (enable)
        {
            
            Debug.Log("Mostrar pantalla de carga");
            _animator.SetTrigger("Show");
            loadingScreeen.SetActive(true);
            video.time = 1;
            video.Play();
        }
        else
        {
            
            StartCoroutine(ProcessLevelLoading());
        }
    }
    public void SendLoadingScreenShowEvent()
    {
        loadingScreenToggled.Raise(true);
    }
    public void SendLoadingScreenHideEvent()
    {
        loadingScreenToggled.Raise(false);
    }
    IEnumerator ProcessLevelLoading()
    {
        _animator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.1f);
        loadingScreeen.SetActive(false);
        video.Stop();
        video.time = 0;
    }
}
