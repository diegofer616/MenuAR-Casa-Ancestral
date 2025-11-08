using ScriptableObjectArchitecture;
using UnityEngine;

public class IniciarEscena : MonoBehaviour
{
    [SerializeField] public SceneLoadRequestGameEvent cargarEscenaEvent;
    public SceneLoadRequest request;
    string nombreEscena;
    //[SerializeField] private event
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IniciarEscenaMenu(SceneSO escena)
    {
        request.escena = escena;
        request.pantallaDeCarga = true;
        cargarEscenaEvent.Raise(request);
    }
    public void IniciarEscenaAR(SceneSO escena)
    {
        request.escena = escena;
        request.pantallaDeCarga = true;
        cargarEscenaEvent.Raise(request);
    }
}
