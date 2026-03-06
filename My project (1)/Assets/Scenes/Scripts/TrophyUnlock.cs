using UnityEngine;

public class TrophyUnlock : MonoBehaviour
{
    public GameObject trophy;
    public float sunReq;
    public float waterReq;

    private bool unlocked = false;

    void Update()
    {
        if (unlocked || trophy == null)
            return;

        ResourceManager rm = ResourceManager.Instance;

        if (rm == null)
            return;

        if (rm.sunlight >= sunReq && rm.water >= waterReq)
        {
            trophy.SetActive(true);
            unlocked = true;

            Debug.Log("Trophy unlocked!");
        }
    }
}