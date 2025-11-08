using UnityEngine;

[CreateAssetMenu(fileName = "ModelManager", menuName = "Scriptable Objects/ModelManager")]
public class ModelManager : ScriptableObject
{
    public PlatoSO currentModel;

    public void SetModel(PlatoSO newModel)
    {
        currentModel = newModel;
    }
}
