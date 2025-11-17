using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SceneSO", menuName = "Scriptable Objects/SceneSO")]
public class SceneSO : ScriptableObject
{
    // Si quieres mantener el nombre legible/legacy:
    public string nombreEscena;

    // Asigna aquí la escena addressable desde el inspector (drag & drop)
    public AssetReference escenaAddressable;
}