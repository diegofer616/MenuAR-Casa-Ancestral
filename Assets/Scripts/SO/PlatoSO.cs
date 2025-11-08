using UnityEngine;

[CreateAssetMenu(fileName = "PlatoSO", menuName = "Scriptable Objects/PlatoSO")]
public class PlatoSO : ScriptableObject
{
    [TextArea(3, 8)]
    public string DescripcionPlato;
    [TextArea(3, 8)]
    public string DescripcionIngles;
    public Sprite ImagenPlato;
    public string NombrePlato;
    public GameObject ModeloPlato;
    public string precioPlato;

}
