
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
    [SerializeField] float planeVisibilityCheckInterval = 0.25f; // cada cuanto se revisan visibilidades
    [SerializeField] float maxPlaneViewDistance = 5f; // distancia máxima para considerar visible
    [SerializeField] float overlapThreshold = 0.25f; // si dos planos están a menos de esto (m) se consideran solapados

    GameObject spawnedObject;
    [SerializeField] InputAction touchInput;

    ARRaycastManager aRRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Último plano objetivo del raycast (si existe)
    ARPlane lastHitPlane;

    float visibilityTimer = 0f;

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

    private void Start()
    {
        // Inicializar visibilidad de planos
        UpdateAllPlanesVisibility(true);
    }

    private void Update()
    {
        if (aRRaycastManager == null || placementIndicator == null)
            return;

        // Raycast al centro (puedes volver a Screen.width/2 si quieres)
        Vector2 screenPoint = new Vector2(Screen.width / 3f, Screen.height / 3f);

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

        // Actualizar visibilidad de planos periódicamente (no cada frame)
        visibilityTimer += Time.deltaTime;
        if (visibilityTimer >= planeVisibilityCheckInterval)
        {
            visibilityTimer = 0f;
            UpdatePlaneVisibilityBasedOnCamera();
        }
    }

    // Lógica de colocación evitando solapamientos con planos cercanos
    public void PlaceObject()
    {
        if (placementIndicator == null || !placementIndicator.activeInHierarchy)
            return;

        if (placedPrefab == null)
        {
            Debug.LogWarning("PlaceOnIndicator: placedPrefab no está asignado.", this);
            return;
        }

        // Si no hay ARPlaneManager o lastHitPlane, hacemos una comprobación simple con objetos ya colocados
        if (arPlaneManager == null || lastHitPlane == null)
        {
            // Evitar colocar encima de otro objeto spawn (si está muy cerca)
            if (spawnedObject != null && Vector3.Distance(spawnedObject.transform.position, placementIndicator.transform.position) < overlapThreshold)
            {
                Debug.Log("PlaceOnIndicator: ya hay un objeto muy cercano; no se coloca.");
                return;
            }

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(placedPrefab, placementIndicator.transform.position, placementIndicator.transform.rotation);
            }
            else
            {
                spawnedObject.transform.SetPositionAndRotation(placementIndicator.transform.position, placementIndicator.transform.rotation);
            }

            return;
        }

        // Si tenemos el plano que fue raycasteado, comprobamos si está demasiado cerca de otros planos
        ARPlane bestPlane = lastHitPlane;
        foreach (var plane in arPlaneManager.trackables)
        {
            if (plane == lastHitPlane) continue;

            float dist = Vector3.Distance(plane.transform.position, lastHitPlane.transform.position);
            if (dist < overlapThreshold)
            {
                // si están muy cerca, elegimos el que tenga mayor tamaño (area estimada por size.x * size.y)
                float sizeA = GetPlaneArea(bestPlane);
                float sizeB = GetPlaneArea(plane);
                if (sizeB > sizeA)
                    bestPlane = plane;
            }
        }

        // Si el mejor plano está demasiado cerca de un objeto ya colocado, no colocamos
        if (spawnedObject != null && Vector3.Distance(spawnedObject.transform.position, bestPlane.transform.position) < overlapThreshold)
        {
            Debug.Log("PlaceOnIndicator: existe un objeto ya colocado muy cercano al plano seleccionado; no se coloca.");
            return;
        }

        // Colocar sobre bestPlane (usar su centro y rotación)
        Vector3 placePos = placementIndicator.transform.position;
        Quaternion placeRot = placementIndicator.transform.rotation;

        // Ajuste opcional: si quieres alinear con la normal del plano:
        // placeRot = Quaternion.LookRotation(bestPlane.transform.up, Camera.main ? Camera.main.transform.forward : Vector3.forward);

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(placedPrefab, placePos, placeRot);
            objectManipulator.getARObject(spawnedObject);
            spawnedObject.SetActive(true);
        }
        else
        {
            spawnedObject.transform.SetPositionAndRotation(placePos, placeRot);
        }
    }

    float GetPlaneArea(ARPlane plane)
    {
        if (plane == null) return 0f;
        // ARPlane.size es un Vector2 (x = width, y = height) en muchas versiones de AR Foundation
        try
        {
            return plane.size.x * plane.size.y;
        }
        catch
        {
            // fallback aproximado: usar extents
            return plane.extents.x * plane.extents.y;
        }
    }

    // Comprueba visibilidad de todos los planos según la cámara
    void UpdatePlaneVisibilityBasedOnCamera()
    {
        if (arPlaneManager == null) return;

        Camera cam = Camera.main;
        if (cam == null)
            return;

        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (var plane in arPlaneManager.trackables)
        {
            bool visible = IsPlaneInView(plane, cam, frustumPlanes);
            SetPlaneVisualsActive(plane, visible);
        }
    }

    // Habilita o deshabilita renderers del plano sin desactivar el trackable
    void SetPlaneVisualsActive(ARPlane plane, bool active)
    {
        if (plane == null) return;

        // Habilitar/deshabilitar todos los Renderers hijos (mesh de plano, boundary, etc.)
        var renderers = plane.GetComponentsInChildren<Renderer>(true);
        foreach (var r in renderers)
            r.enabled = active;

        // Si usas componentes específicos como ARPlaneMeshVisualizer u otros visualizadores,
        // puedes habilitarlos/deshabilitarlos aquí en lugar de destruir el GameObject.
        var meshVisualizers = plane.GetComponentsInChildren<ARPlaneMeshVisualizer>(true);
        foreach (var v in meshVisualizers)
            v.enabled = active;
    }

    // Determina si el plano está dentro del frustum y a una distancia razonable y con ángulo aceptable
    bool IsPlaneInView(ARPlane plane, Camera cam, Plane[] frustumPlanes)
    {
        if (plane == null || cam == null) return false;

        Vector3 center = plane.transform.position;
        Vector3 toCenter = center - cam.transform.position;
        float dist = toCenter.magnitude;
        if (dist > maxPlaneViewDistance) return false;

        // test frustum
        Bounds b = new Bounds(center, new Vector3(Mathf.Max(0.1f, plane.size.x), 0.01f, Mathf.Max(0.1f, plane.size.y)));
        if (!GeometryUtility.TestPlanesAABB(frustumPlanes, b))
            return false;

        // test ángulo frontal (evitar planos que están "detrás" por rotación)
        float dot = Vector3.Dot(cam.transform.forward.normalized, toCenter.normalized);
        if (dot < 0.25f) // ajustar umbral según necesidad
            return false;

        return true;
    }

    // Util: activar todos los planos (por ejemplo al iniciar)
    void UpdateAllPlanesVisibility(bool active)
    {
        if (arPlaneManager == null) return;
        foreach (var plane in arPlaneManager.trackables)
            SetPlaneVisualsActive(plane, active);
    }
}