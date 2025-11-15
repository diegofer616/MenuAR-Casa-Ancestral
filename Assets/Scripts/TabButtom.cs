using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButtom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup tabGroup;
    public Image background;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }
}
