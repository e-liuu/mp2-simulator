using UnityEngine;

public class SunFloat : MonoBehaviour
{
    public float fallSpeed = 0.4f;
    public float rotateSpeed = 60f;
    public float groundCheckDistance = 1f;

    private bool hasLanded = false;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (hasLanded)
            return;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            // Snap exactly to ground contact point (no invisible gap)
            transform.position = hit.point;

            hasLanded = true;
            return;
        }

        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }
}