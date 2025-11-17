
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnIndicator : MonoBehaviour
{
    [SerializeField] ObjectManipulator objectManipulator;
    [SerializeField] GameObject placementIndicator;
    [SerializeField] GameObject placedPrefab;
    [SerializeField] ARPlaneManager arPlaneManager;
    [SerializeField] InputAction touchInput;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    GameObject spawnedObject;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        if (arPlaneManager == null)
            arPlaneManager = FindObjectOfType<ARPlaneManager>();

        if (placementIndicator != null)
            placementIndicator.SetActive(false);
        else
            Debug.LogWarning("PlaceOnIndicator: placementIndicator no está asignado.", this);
    }

    private void OnEnable()
    {
        if (aRRaycastManager == null)
            aRRaycastManager = FindObjectOfType<ARRaycastManager>();

        if (arPlaneManager == null)
            arPlaneManager = FindObjectOfType<ARPlaneManager>();

        if (touchInput != null)
            touchInput.Enable();
    }

    private void OnDisable()
    {
        if (touchInput != null)
            touchInput.Disable();
    }

    private void Update()
    {
        if (aRRaycastManager == null || placementIndicator == null)
            return;

        // Raycast al centro de la pantalla
        Vector2 screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        if (aRRaycastManager.Raycast(screenPoint, hits, TrackableType.PlaneWithinPolygon))
        {
            var hit = hits[0];
            var hitPose = hit.pose;
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

            if (!placementIndicator.activeInHierarchy)
                placementIndicator.SetActive(true);
        }
        else
        {
            if (placementIndicator.activeInHierarchy)
                placementIndicator.SetActive(false);
        }
    }

    // Método público para colocar el objeto (conectarlo a un botón)
    public void PlaceObject()
    {
        if (placementIndicator == null || !placementIndicator.activeInHierarchy)
            return;

        if (placedPrefab == null)
        {
            Debug.LogWarning("PlaceOnIndicator: placedPrefab no está asignado.", this);
            return;
        }

        Vector3 pos = placementIndicator.transform.position;
        Quaternion rot = placementIndicator.transform.rotation;

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(placedPrefab, pos, rot);
            spawnedObject.SetActive(true);
            if (objectManipulator != null)
                objectManipulator.getARObject(spawnedObject);
        }
        else
        {
            spawnedObject.transform.SetPositionAndRotation(pos, rot);
        }
    }
}