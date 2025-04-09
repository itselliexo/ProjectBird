using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkTrigger : MonoBehaviour
{
    [SerializeField] private WorkMinigame workMinigame;
    public GameObject minigameUI;
    public GameObject prompText;
    private bool playerInRange = false;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    private void Start()
    {
        if (prompText != null)
        {
            prompText.gameObject.SetActive(false);
        }

        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (prompText != null)
            {
                prompText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (prompText != null)
            {
                prompText.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (workMinigame != null)
            {
                if (!workMinigame.isPlaying)
                {
                    workMinigame.StartWorkGame();
                    Cursor.lockState = CursorLockMode.None;
                    if (minigameUI != null)
                    {
                        minigameUI.SetActive(true);
                    }
                }
                if (workMinigame.isPlaying)
                {
                    workMinigame.EndWorkGame();
                    Cursor.lockState = CursorLockMode.Locked;
                    if (minigameUI != null)
                    {
                        minigameUI.SetActive(false);
                    }

                }
                if (workMinigame.isPlaying)
                {
                    prompText.SetActive(true);
                }
                else
                {
                    prompText.SetActive(false);
                }
                    playerMovement.canMove = !playerMovement.canMove;
                thirdPersonCamera.canLook = !thirdPersonCamera.canLook;
                workMinigame.isPlaying = !workMinigame.isPlaying;
            }
        }
    }

    private void StartWorkGame()
    {
        if (minigameUI != null)
        {
            minigameUI.SetActive(true);
        }

        if (prompText != null)
        {
            prompText.SetActive(false);
        }

        Debug.Log("Off to work!");
    }
}
