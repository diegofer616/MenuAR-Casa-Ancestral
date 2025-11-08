using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject;
    [SerializeField] private Camera aRCamera;
    private float touchDis;
    private Vector2 touchPositionDiff;

    private float rotationTolerance = 1.5f;
    [SerializeField] float speedRotation = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void getARObject(GameObject newAr)
    {
        ARObject = newAr;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {



            if (Input.touchCount == 2)
            {
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    touchPositionDiff = touchTwo.position - touchOne.position;
                    touchDis = Vector2.Distance(touchTwo.position, touchOne.position);
                }
                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchDiff = touchTwo.position - touchOne.position;
                    float currentTouchDis = Vector2.Distance(touchTwo.position, touchOne.position);

                    float diffDis = currentTouchDis - touchDis;

                    float angle = Vector2.SignedAngle(touchPositionDiff, currentTouchDiff);
                    if (Mathf.Abs(angle) > rotationTolerance)
                    {
                        ARObject.transform.rotation = Quaternion.Euler(0, ARObject.transform.rotation.eulerAngles.y - Mathf.Sign(angle) * speedRotation, 0);
                    }

                    touchPositionDiff = currentTouchDiff;
                    touchDis = currentTouchDis;
                }

            }
        }
    }
  
}
