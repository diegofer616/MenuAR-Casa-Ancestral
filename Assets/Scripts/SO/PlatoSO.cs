using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(fileName = "PlatoSO", menuName = "Scriptable Objects/PlatoSO")]
public class PlatoSO : ScriptableObject
{
    [TextArea(3, 8)]
    public string DescripcionPlato;
    [TextArea(3, 8)]
    public string DescripcionIngles;
    public AssetReferenceSprite ImagenPlato;
    public string NombrePlato;
    public AssetReferenceGameObject ModeloPlato;
    public string precioPlato;
    public int categoriaPlato;
}
