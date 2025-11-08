using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ARManager : MonoBehaviour
{
   [SerializeField] private ARRaycastManager raycastManager;
   [SerializeField] private ARSession ARSession;
   [SerializeField] private GameObject canvas;
    GameObject platoColocado;
    bool isPlacing = false;




    private IEnumerator Start()
    {
        // Espera a que la escena esté completamente cargada
        yield return null;

       

       
        yield return new WaitForSeconds(0.5f);
        if (ARSession != null)
        {
            ARSession.enabled = true;
        }

        canvas.SetActive(true);
    }
    
    void Update()
    {
        if(!raycastManager) return;
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.E)) && !isPlacing) 
        {

            isPlacing = true;

            if (Input.touchCount > 0)
            {
                PlaceObject(Input.GetTouch(0).position);
            }
            else 
            {
                Debug.Log("estas introduciendo una tecla");
                PlaceObject(Input.mousePosition);
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && platoColocado != null)
        {
            Destroy(platoColocado);
            
        }
    }

    void PlaceObject(Vector2 touchPosition)
    {
        
        var rayHits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, rayHits, TrackableType.AllTypes);

        if(rayHits.Count > 0)
        {
            Vector3 hitPosePosition = rayHits[0].pose.position;
            Quaternion hitPoserotation = rayHits[0].pose.rotation;
            platoColocado= Instantiate(raycastManager.raycastPrefab, hitPosePosition, hitPoserotation);
        }
        //StartCoroutine(SetIsPlacingToFalseWithDelay());
       

    }

    
    public void EliminarPlato()
        {  
        if(platoColocado != null){
                Destroy(platoColocado);
                isPlacing = false;
            }
        }
    
}
