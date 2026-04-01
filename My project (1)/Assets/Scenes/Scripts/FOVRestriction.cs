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

        lastPosition = xrCamera.transform.position; // track camera not rig
    }

    void Update()
    {
        float speed = (xrCamera.transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = xrCamera.transform.position;

        if (speed > motionThreshold)
            timeSinceMoving = 0f;
        else
            timeSinceMoving += Time.deltaTime;

        float targetFOV = timeSinceMoving < revertDelay ? restrictedFOV : normalFOV;
        xrCamera.fieldOfView = Mathf.Lerp(xrCamera.fieldOfView, targetFOV, Time.deltaTime * smoothSpeed);
    }
}