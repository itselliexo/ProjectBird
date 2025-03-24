using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject player;
    [SerializeField] private Transform newCameraPosition;
    [SerializeField] Vector3 boom;
    [SerializeField] private bool isFirstPerson = false;
    [SerializeField] RenderTexture renderTexture;
    public BirdDatabase birdDatabase;
    [SerializeField] private Bird[] allBirds;
    public GameObject targetBird;

    [Header("Mouse Settings")]
    [SerializeField] float sensitivity;
    [SerializeField] float rotationSmoothTime;
    [SerializeField] float lowerRotLimit;
    [SerializeField] float upperRotLimit;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;

    private void LateUpdate()
    {
        if (!player) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            birdDatabase.ResetProgress();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isFirstPerson = !isFirstPerson;
        }

        if (isFirstPerson)
        {
            HandleFirstRotation();
        }
        else
        {
            HandleThirdRotation();
        }

        if (isFirstPerson)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AssignTargetBird();
                StartCoroutine(CapturePhoto());
            }
        }
    }

    private void UpdateBirdArray()
    {
        allBirds = FindObjectsOfType<Bird>();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UpdateBirdArray();
    }

    private void HandleThirdRotation()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, lowerRotLimit, upperRotLimit);

        Vector3 targetRotation = new Vector3(pitch, yaw, 0);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        Quaternion rotation = Quaternion.Euler(currentRotation);

        Vector3 desiredPosition = player.transform.position + rotation * boom;

        mainCamera.transform.position = desiredPosition;

        /*RaycastHit hit;
         if (Physics.Raycast(player.transform.position, (desiredPosition - player.transform.position).normalized, out hit, boom.magnitude))
         {
             mainCamera.transform.position = hit.point + hit.normal * 0.3f;
         }
         else
         {
             mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, Time.deltaTime * 10);
         }*/

        mainCamera.transform.LookAt(player.transform.position + Vector3.up * boom.y);
    }

    private void HandleFirstRotation()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, lowerRotLimit, upperRotLimit);

        player.transform.rotation = Quaternion.Euler(0, yaw, 0);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition.position, Time.deltaTime * 10000);
        mainCamera.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        mainCamera.targetTexture = renderTexture;

        mainCamera.Render();

        Bird bird = targetBird.GetComponent<Bird>();

        if (bird != null && IsObjectInView(targetBird))
        {

            birdDatabase.UnlockBird(bird.birdData.birdName);

            if (bird.birdData.isUnlocked == true)
            {
            }
        }

        mainCamera.targetTexture = null;
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
            if (bird != null && bird.birdData != null)
            {
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
