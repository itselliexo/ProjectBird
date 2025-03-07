using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCam : MonoBehaviour
{
    [SerializeField] Camera birdCam;
    [SerializeField] Camera mainCamera;
    [SerializeField] RenderTexture renderTexture;
    public GameObject targetBird;
    [SerializeField] KeyCode captureKey = KeyCode.Mouse0;
    [SerializeField] private bool isInCamera = false;

    [Header("FPV settings")]
    [SerializeField] float sensitivity;
    float pitch;
    float yaw;

    private void Start()
    {
        birdCam.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isInCamera = !isInCamera;
            birdCam.enabled = isInCamera;
            mainCamera.enabled = !isInCamera;
        }

        if (isInCamera)
        {
            HandleFPV();

            if (Input.GetKeyDown(captureKey))
            {
                StartCoroutine(CapturePhoto());
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        birdCam.targetTexture = renderTexture;

        birdCam.Render();

       if (IsObjectInView(targetBird))
       {
            Debug.Log("Bird Found");
       }
       else
       {
            Debug.Log("Bird Not Found");
       }

        birdCam.targetTexture = null;
    }

    bool IsObjectInView(GameObject obj)
    {
        if (obj.CompareTag("Robin"))
        {
            Vector3 viewPos = birdCam.WorldToViewportPoint(obj.transform.position);

            return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
        }
        else
        {
            return false;
        }
    }

    private void HandleFPV()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -70f, 70f);

        birdCam.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
