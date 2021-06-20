using UnityEngine;

public class Platform : MonoBehaviour
{
    private GameObject platform;
    private Vector3 OriginPlatform;
    private bool PlatformGoingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        platform = GameObject.Find("Platform");
        OriginPlatform = platform.transform.position;
    }

    //Permet de faire bouger une plateforme sur l'axe des Y
    public void PlatformMove()
    {
        if (platform.transform.position.y < OriginPlatform.y + 1 && PlatformGoingUp)
        {
            platform.transform.position = platform.transform.position + (Time.deltaTime * new Vector3(0, 1, 0));
        }
        else if (platform.transform.position.y > OriginPlatform.y - 2)
        {
            PlatformGoingUp = false;
            platform.transform.position = platform.transform.position + (Time.deltaTime * new Vector3(0, -1, 0));
        }
        else
        {
            PlatformGoingUp = true;
        }
    }

    public GameObject getPlatform()
    {
        return platform;
    }
}