using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    [SerializeField] private Camera aRCamera;
    private float touchDis;
    private Vector2 touchPositionDiff;

    private float rotationTolerance = 1.5f;
    [SerializeField] float rotationSensitivity = 1f; // factor de sensibilidad (ajustable)

    public void getARObject(GameObject newAr)
    {
        ARObject = newAr;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
            {
                touchPositionDiff = touchTwo.position - touchOne.position;
                touchDis = Vector2.Distance(touchTwo.position, touchOne.position);
                return;
            }

            if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchDiff = touchTwo.position - touchOne.position;
                float currentTouchDis = Vector2.Distance(touchTwo.position, touchOne.position);

                // Ángulo entre la diferencia previa y la actual (en grados)
                float angle = Vector2.SignedAngle(touchPositionDiff, currentTouchDiff);

                if (Mathf.Abs(angle) > rotationTolerance && ARObject != null)
                {
                    // preservar posición world antes de rotar (evita pequeños desvíos por pivote)
                    Vector3 worldPos = ARObject.transform.position;

                    // Rotación incremental: eje up local
                    float deltaAngle = -angle * rotationSensitivity;
                    ARObject.transform.Rotate(Vector3.up, deltaAngle, Space.Self);

                    // Reaplicar la posición world para evitar desplazamientos accidentales
                    ARObject.transform.position = worldPos;
                }

                touchPositionDiff = currentTouchDiff;
                touchDis = currentTouchDis;
            }
        }
    }
}