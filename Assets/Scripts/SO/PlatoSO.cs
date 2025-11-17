using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scriptable Objects/PlatoSO", fileName = "PlatoSO")]
public class PlatoSO : ScriptableObject
{
    public string NombrePlato;
    [TextArea] public string DescripcionPlato;
    public string DescripcionIngles;
    public Sprite ImagenPlato;
    public string precioPlato;

    // Prefab directo (fallback)
    public GameObject ModeloPlato;

    // AssetReference opcional: arrastra aquí el prefab addressable del modelo
    public AssetReferenceGameObject ModeloPlatoAddressable;
}