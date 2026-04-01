using UnityEngine;

public class FOVRestriction : MonoBehaviour
{
    public Camera xrCamera;
    public float normalFOV = 90f;
    public float restrictedFOV = 60f;
    public float smoothSpeed = 5f;
    public float motionThreshold = 0.02f;
    public float revertDelay = 0.3f;

    private Vector3 lastPosition;
    private float timeSinceMoving = 0f;

    void Start()
    {
        if (xrCamera == null)
            xrCamera = Camera.main;

        lastPosition = xrCamera.transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = xrCamera.transform.position;
        
        // Only check X and Z, ignore Y (vertical)
        Vector2 currentHorizontal = new Vector2(currentPosition.x, currentPosition.z);
        Vector2 lastHorizontal = new Vector2(lastPosition.x, lastPosition.z);
        
        float speed = (currentHorizontal - lastHorizontal).magnitude / Time.deltaTime;
        lastPosition = currentPosition;

        if (speed > motionThreshold)
            timeSinceMoving = 0f;
        else
            timeSinceMoving += Time.deltaTime;

        float targetFOV = timeSinceMoving < revertDelay ? restrictedFOV : normalFOV;
        xrCamera.fieldOfView = Mathf.Lerp(xrCamera.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}