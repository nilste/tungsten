using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make the background parallax and self-repeating
/// Concept by Waldo on Press Start, with modifications
/// https://www.youtube.com/watch?v=3UO-1suMbNc
/// https://www.youtube.com/watch?v=Mp6BWCMJZH4
/// </summary>
public class Background : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float scrollSpeed = .1f;
    [SerializeField] float choke = 0f;

    // References
    private List<GameObject> levels = null;
    private Camera mainCamera = null;
    private GameObject mainCameraObject = null;
    private Vector2 screenBounds;
    private Vector3 lastScreenPosition;

    void Awake()
    {
        // Fetch the camera object, to not have the script on the prefab
        mainCameraObject = GameObject.FindWithTag("MainCamera");
        mainCamera = mainCameraObject.GetComponent<Camera>();
        
        // Calculate screen estate and bounds
        Vector3 cameraDimensions = new Vector3(Screen.width * 2, Screen.height, mainCamera.transform.position.z);
        screenBounds = mainCamera.ScreenToWorldPoint(cameraDimensions);
        
        // Automatically import child objects to avoid doing it manually
        levels = new List<GameObject>();
        foreach (Transform child in gameObject.transform)
        {
            levels.Add(child.gameObject);
            LoadChildObjects(child.gameObject);
        }

        lastScreenPosition = mainCamera.transform.position;
    }

    void Start()
    {
        
    }

    void LoadChildObjects(GameObject obj)
    {
        // Get width and narrow it a bit to avoid space in between
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        objectWidth -= choke;

        int childsNeeded = (int)Mathf.Ceil(screenBounds.x * 2 / objectWidth);

        GameObject clone = Instantiate(obj) as GameObject;

        for (int i = 0; i <= childsNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(obj.transform);

            c.transform.position = new Vector3(objectWidth * i,
                obj.transform.position.y,
                obj.transform.position.z);
            
            c.name = obj.name + 1;
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = mainCamera.transform.position + new Vector3(scrollSpeed, 0, 0);
        Vector3 smothPosition = Vector3.SmoothDamp(mainCamera.transform.position, desiredPosition, ref velocity, 0.3f);
        mainCamera.transform.position = smothPosition;
    }

    void FixedUpdate()
    {
        foreach (GameObject obj in levels)
        {
            repositionChildOjbects(obj);

            // Adjust speed based on distance from camera
            float parallaxSpeed = 1 - Mathf.Clamp01(Mathf.Abs(mainCamera.transform.position.z / obj.transform.position.z));
            float difference = mainCamera.transform.position.x - lastScreenPosition.x;
            obj.transform.Translate(Vector3.right * difference * parallaxSpeed);
        }

        lastScreenPosition = mainCamera.transform.position;
    }

    void repositionChildOjbects(GameObject obj)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        
        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;
            
            // Get width and narrow it a bit to avoid space in between
            float halfObjectWidth = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;
            halfObjectWidth -= choke;

            if ((mainCameraObject.transform.position.x + screenBounds.x) > (lastChild.transform.position.x + halfObjectWidth))
            {
                firstChild.transform.SetAsLastSibling();
                firstChild.transform.position = new Vector3(
                    lastChild.transform.position.x + halfObjectWidth * 2,
                    lastChild.transform.position.y,
                    lastChild.transform.position.z);
            }
            else if ((mainCameraObject.transform.position.x - screenBounds.x) < (firstChild.transform.position.x - halfObjectWidth))
            {
                lastChild.transform.SetAsFirstSibling();
                lastChild.transform.position = new Vector3(
                    firstChild.transform.position.x - halfObjectWidth * 2,
                    firstChild.transform.position.y,
                    firstChild.transform.position.z);
            }
        }
    }
}
