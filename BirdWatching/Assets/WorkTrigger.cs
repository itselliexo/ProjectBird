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
                workMinigame.StartWorkGame();
                prompText.SetActive(false);
                playerMovement.canMove = !playerMovement.canMove;
                thirdPersonCamera.canLook = !thirdPersonCamera.canLook;
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
