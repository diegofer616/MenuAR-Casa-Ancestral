[System.Serializable]
public class SceneLoadRequest
{
    public SceneSO escena;
    public bool pantallaDeCarga;
    

    public SceneLoadRequest(SceneSO scene, bool pantallaDeCarga)
    {
        this.escena = scene;
        this.pantallaDeCarga = pantallaDeCarga;
    }
}