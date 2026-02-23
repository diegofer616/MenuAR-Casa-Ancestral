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
    [SerializeField] float overlapThreshold = 0.25f; // si dos planos/objetos están a menos de esto (m) se consideran solapados

    GameObject spawnedObject;
    [SerializeField] InputAction touchInput;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Último plano objetivo del raycast (si existe)
    ARPlane lastHitPlane;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        if (arPlaneManager == null)
            arPlaneManager = FindObjectOfType<ARPlaneManager>();

        if (placementIndicator != null)
            placementIndicator.SetActive(false);
        else
            Debug.LogWarning("PlaceOnIndicator: placementIndicator no está asignado.", this);

        // Asegurarnos de que los planos sean visibles al inicio
        EnsureAllPlaneVisuals(true);
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

            // Guardar el plano objetivo del hit
            if (arPlaneManager != null)
            {
                var plane = arPlaneManager.GetPlane(hit.trackableId);
                lastHitPlane = plane;
            }
            else
            {
                lastHitPlane = null;
            }
        }
        else
        {
            placementIndicator.SetActive(false);
            lastHitPlane = null;
        }
    }

    // Lógica de colocación evitando solapamientos con objetos/planos cercanos
    public void PlaceObject()
    {
        if (placementIndicator == null || !placementIndicator.activeInHierarchy)
            return;

        if (placedPrefab == null)
        {
            Debug.LogWarning("PlaceOnIndicator: placedPrefab no está asignado.", this);
            return;
        }

        Vector3 placePos = placementIndicator.transform.position;
        Quaternion placeRot = placementIndicator.transform.rotation;

        // Si no hay ARPlaneManager o lastHitPlane, comprobación simple contra objeto ya colocado
        if (arPlaneManager == null || lastHitPlane == null)
        {
            if (spawnedObject != null && Vector3.Distance(spawnedObject.transform.position, placePos) < overlapThreshold)
            {
                Debug.Log("PlaceOnIndicator: ya hay un objeto muy cercano; no se coloca.");
                return;
            }

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(placedPrefab, placePos, placeRot);
                FinishPlacement();
            }
            else
            {
                spawnedObject.transform.SetPositionAndRotation(placePos, placeRot);
            }

            return;
        }

        // Si hay plano objetivo, comprobamos planos cercanos para evitar solapamientos
        ARPlane bestPlane = lastHitPlane;
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane == lastHitPlane) continue;

            float dist = Vector3.Distance(plane.transform.position, lastHitPlane.transform.position);
            if (dist < overlapThreshold)
            {
                float sizeA = GetPlaneArea(bestPlane);
                float sizeB = GetPlaneArea(plane);
                if (sizeB > sizeA)
                    bestPlane = plane;
            }
        }

        // Colocar sobre bestPlane (usa la posición del indicador)
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(placedPrefab, placePos, placeRot);
            FinishPlacement();
        }
        else
        {
            spawnedObject.transform.SetPositionAndRotation(placePos, placeRot);
        }
    }

    void FinishPlacement()
    {
        if (objectManipulator != null)
            objectManipulator.GetARObject(spawnedObject);
        spawnedObject.SetActive(true);
    }

    float GetPlaneArea(ARPlane plane)
    {
        if (plane == null) return 0f;
        try
        {
            return plane.size.x * plane.size.y;
        }
        catch
        {
            return plane.extents.x * plane.extents.y;
        }
    }

    // Fuerza que todos los visualizadores y renderers de planos estén activos/inactivos
    void EnsureAllPlaneVisuals(bool active)
    {
        if (arPlaneManager == null) return;
        foreach (var plane in arPlaneManager.trackables)
        {
            var renderers = plane.GetComponentsInChildren<Renderer>(true);
            foreach (var r in renderers) r.enabled = active;

            var meshVisualizers = plane.GetComponentsInChildren<ARPlaneMeshVisualizer>(true);
            foreach (var v in meshVisualizers) v.enabled = active;
        }
    }
}