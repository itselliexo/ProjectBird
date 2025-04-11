using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script will handle the minigame of work. this is the way to make money in the game and the money is used for everything.
  it will be a card jitzu inspired game where players will have a random selection of cards drawn from their deck and will play against the computer or other player.
  cards range from 1-10 and have 1 of 3 suits ; fire, ice and water. Fire beats ice, ice beats water, water beats fire.
  the player and their opponent will have a "task" which is their health pool, players start with 50 health and the first person to deplete the other players health wins.
  cards will do damage based on their number and have a monetary value which decreases the higher the number is. this prevents players from just playing high cards and winning every time
  players will draw 5 random cards and choose one to play at the start of the round. the opponent will also choose and if the suit is winning, deal 3x damage, if the suit is the same, no extra damage.*/

public class WorkMinigame : MonoBehaviour
{
    [Header("Lists")]
    [SerializeField] private List<Card> allPlayerCards;
    [SerializeField] private List<Card> allOpponentCards;
    [SerializeField] private List<Card> currentHand;
    [SerializeField] private List<Card> opponentHand;

    [Header("Round Settings")]
    [SerializeField] private int roundNumber;
    [SerializeField] private int difficultySetting;
    [SerializeField] public bool isPlaying = false;

    [Header("Player Settings")]
    [SerializeField] private int playerHealth;
    [SerializeField] private Card playerPlayedCard;

    [Header("Opponent Settings")]
    [SerializeField] private int opponentHealth;
    [SerializeField] private Card opponentPlayedCard;

    void Start()
    {

    }

    void Update()
    {
        if (playerHealth <= 0 || opponentHealth <= 0)
        {
            ResetWorkGame();
        }
    }

    public void StartWorkGame()
    {
        AddPlayerCards();

        AddOpponentCards();

        DrawPlayerHand();

        DrawOpponentHand();
    }

    private void AddPlayerCards()
    {
        var allPlayerCardsArray = Resources.LoadAll<Card>("Cards");

        allPlayerCards.AddRange(allPlayerCardsArray);
    }

    private void DrawPlayerHand()
    {
        currentHand.Clear();

        while (currentHand.Count < 5)
        {
            int randomValue = Random.Range(0, 1000) % 30;
            if (currentHand.Contains(allPlayerCards[randomValue]))
            {
                continue;
            }
            else
            {
                currentHand.Add(allPlayerCards[randomValue]);
            }
        }
    }

    private void AddOpponentCards()
    {
        var allOpponentCardsArray = Resources.LoadAll<Card>("Cards");

        allOpponentCards.AddRange(allOpponentCardsArray);
    }

    private void DrawOpponentHand()
    {
        opponentHand.Clear();

        while (opponentHand.Count < 5)
        {
            int randomValue = Random.Range(0, 1000) % 30;
            if (opponentHand.Contains(allOpponentCards[randomValue]))
            {
                continue;
            }
            else
            {
                opponentHand.Add(allOpponentCards[randomValue]);
            }
        }
    }

    private void ResetWorkGame()
    {
        allPlayerCards.Clear();
        allOpponentCards.Clear();
        currentHand.Clear();
        opponentHand.Clear();
    }

    public void EndWorkGame()
    {
        ResetWorkGame();
    }
}
