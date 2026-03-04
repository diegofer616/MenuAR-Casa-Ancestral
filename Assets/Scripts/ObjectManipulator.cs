using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    [SerializeField] private Camera aRCamera;
    private float touchDis;
    private Vector2 touchPositionDiff;

    private float rotationTolerance = 2.5f;
    private float scaleTolerance = 25f;
    [SerializeField] float speedRotation = 1f; 
    [SerializeField] float scaleFactor = 0.1f;
    public void GetARObject(GameObject newAr)
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
                float disDiff = currentTouchDis - touchDis;

                if (Mathf.Abs(disDiff) > scaleTolerance)
                {
                    Vector3 newScale = ARObject.transform.localScale + Mathf.Sign(disDiff) * Vector3.one * scaleFactor;
                    ARObject.transform.localScale = Vector3.Lerp(ARObject.transform.localScale, newScale, 0.05f);
                }
                // Ángulo entre la diferencia previa y la actual (en grados)
                float angle = Vector2.SignedAngle(touchPositionDiff, currentTouchDiff);

                if (Mathf.Abs(angle) > rotationTolerance && ARObject != null)
                {
                    ARObject.transform.rotation = Quaternion.Euler(0, ARObject.transform.rotation.eulerAngles.y - Mathf.Sign(angle) * speedRotation, 0);
                }

                touchPositionDiff = currentTouchDiff;
                touchDis = currentTouchDis;
            }
        }
    }
}