using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //Variables
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] private Transform newCameraPosition;
    [SerializeField] Vector3 boom;
    [SerializeField] private bool isFirstPerson = false;
    [SerializeField] RenderTexture renderTexture;
    public BirdDatabase birdDatabase;
    [SerializeField] private Bird[] allBirds;
    public GameObject targetBird;
    [SerializeField] public bool canLook;

    [Header("Camera Settings")]
    [SerializeField] float baseFOV;
    [SerializeField] float zoomedFOV;
    [SerializeField] float minZoomFOV;
    [SerializeField] float maxZoomFOV;
    [SerializeField] float lowerRotLimit;
    [SerializeField] float upperRotLimit;

    [Header("Mouse Settings")]
    [SerializeField] float sensitivity;
    [SerializeField] float rotationSmoothTime;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private void LateUpdate()
    {
        if (!player) return; //exits update method if player is null

        if (Input.GetKeyDown(KeyCode.R))
        {
            birdDatabase.ResetProgress();
        } //resets bird data scriptable objects

        if (Input.GetMouseButtonDown(1) && canLook)
        {
            isFirstPerson = !isFirstPerson; //changes from first person to trird person and back
            if (isFirstPerson)
            {
                mainCamera.fieldOfView = zoomedFOV;
            }

            if (!isFirstPerson)
            {
                mainCamera.fieldOfView = baseFOV;
            }//sets camera fov based on fp or tp
        }

        if (isFirstPerson && canLook)
        {
            HandleFirstRotation();

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput > 0)
            {
                mainCamera.fieldOfView--;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoomFOV, maxZoomFOV);
            }
            else if (scrollInput < 0)
            {
                mainCamera.fieldOfView++;
                mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, minZoomFOV, maxZoomFOV);
            }
        }//handles first person camera and allows zoom because its the photograph camera
        else if (!isFirstPerson && canLook)
        {
            HandleThirdRotation();

            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput > 0)
            {
                boom.z++;
            }
            else if (scrollInput < 0)
            {
                boom.z--;
            }
            boom.z = Mathf.Clamp(boom.z, -8, -3);
        }//if its not first person, its third person

        if (isFirstPerson)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AssignTargetBird(); //needed for the coroutine to work
                StartCoroutine(CapturePhoto());
            }
        }//only allows taking photos if in first person
    }

    private void UpdateBirdArray()
    {
        allBirds = FindObjectsOfType<Bird>(); //finds all bird objects in scene and assigns them to an array
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateBirdArray(); //gets the array on start
    }

    private void HandleThirdRotation()
    {
        //mouse inputs
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, lowerRotLimit, upperRotLimit);

        Vector3 targetRotation = new Vector3(pitch, yaw, 0); //a v3 that holds the inputs from pitch and yaw variables 
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime); //setting the v3 current rotation to a smoothed version of the movement (could just use targetRotation as raw movement but will be jerky)
        Quaternion rotation = Quaternion.Euler(currentRotation); //converts euler angles to a quaternion, its essentially the same as the v3 currentRotation but better for unity to understand. magic lmao

        Vector3 desiredPosition = player.transform.position + rotation * boom; //adds the offet to the player in relation to the players position so when she moves, it adds the ofset (boom) as well as the change in location the player moved to.

        mainCamera.transform.position = desiredPosition; //applies the calculated position to the cameras transform

        /*RaycastHit hit; //variable

        Vector3 directionToDesiredPos = (desiredPosition - player.transform.position).normalized;
        float rayDistance = boom.magnitude;

         if (Physics.Raycast(player.transform.position, directionToDesiredPos, out hit, rayDistance))
         {
             mainCamera.transform.position = hit.point + hit.normal * 0.3f;
         }
         else
         {
             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 10);
         }*/

        player.transform.rotation = Quaternion.Euler(0, yaw, 0);

        mainCamera.transform.LookAt(player.transform.position + Vector3.up * boom.y);
    }

    private void HandleFirstRotation()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -90, 90);

        player.transform.rotation = Quaternion.Euler(0, yaw, 0);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition.position, Time.deltaTime * 10000);
        mainCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    IEnumerator CapturePhoto()
    {
        if (targetBird != null)
        {
            yield return new WaitForEndOfFrame();

            mainCamera.targetTexture = renderTexture;

            mainCamera.Render();

            Bird bird = targetBird.GetComponent<Bird>();

            if (bird != null && IsObjectInView(targetBird))
            {
                if (!bird.birdData.isUnlocked)
                {
                    birdDatabase.UnlockBird(bird.birdData.birdName);
                }
            }

            mainCamera.targetTexture = null;
        }
    }

    bool IsObjectInView(GameObject obj)
    {
        Bird bird = obj.GetComponent<Bird>();

        if (bird == null || bird.birdData == null)
        {
            return false;
        }

        Vector3 viewPos = mainCamera.WorldToViewportPoint(obj.transform.position);

        bool inViewport = viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;

        if (!inViewport)
        {
            return false;
        }

        Vector3 direction = obj.transform.position - mainCamera.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(mainCamera.transform.position, direction, out hit))
        {
            return hit.collider.gameObject == obj;
        }

        return false;
    }

    private void AssignTargetBird()
    {
        Bird closestBird = null;
        float closestDistance = Mathf.Infinity;

        foreach (Bird bird in allBirds)
        {
            if (bird == null || bird.birdData == null)
                continue;

            if (bird.birdData.isUnlocked)
                continue;

            Vector3 viewPos = mainCamera.WorldToViewportPoint(bird.transform.position);

            if (viewPos.z > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
                float distanceToBird = Vector3.Distance(mainCamera.transform.position, bird.transform.position);

                if (distanceToBird < closestDistance)
                {
                    closestDistance = distanceToBird;
                    closestBird = bird;
                }
            }
        }

        closestDistance = Mathf.Infinity;

        if (closestBird != null)
        {
            targetBird = closestBird.gameObject;
        }
        else
        {
            Debug.Log("No bird in the viewport.");
        }
    }
}
