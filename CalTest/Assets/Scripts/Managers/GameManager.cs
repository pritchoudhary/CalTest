using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

// Manages the main game logic, including card instantiation, matching, sound, and scoring.
public class GameManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;               // Manages card instantiation and shuffling
    [SerializeField] private TextMeshProUGUI scoreText;             // UI element to display score
    [SerializeField] private AudioService audioService;             // Manages audio playback
    private ScoreService scoreService;                              // Manages scoring and combo system

    private readonly Queue<Card> flippedCardsQueue = new Queue<Card>(); // Queue to manage flipped cards for match checks

    private void Start()
    {
        scoreService = new ScoreService();                          // Initialize score service
        cardManager.InitializeCards(HandleCardFlipped);             // Set up cards with flip event handler
        UpdateScoreUI();                                            // Display the initial score
    }

    // Called when a card is flipped; handles matching logic
    private void HandleCardFlipped(Card flippedCard)
    {
        audioService.PlayFlipSound();
        flippedCardsQueue.Enqueue(flippedCard); // Add the flipped card to the queue

        // If there are two cards in the queue, start a match check
        if (flippedCardsQueue.Count >= 2)
        {
            // Dequeue two cards for the match check
            Card firstCard = flippedCardsQueue.Dequeue();
            Card secondCard = flippedCardsQueue.Dequeue();

            // Start the match-check coroutine for these two cards
            StartCoroutine(CheckMatch(firstCard, secondCard));
        }
    }

    // Coroutine to check if the two dequeued cards match
    private IEnumerator CheckMatch(Card firstCard, Card secondCard)
    {
        yield return new WaitForSeconds(0.5f); // Brief delay to show both cards

        if (firstCard.IsMatch(secondCard))
        {
            audioService.PlayMatchSound();
            scoreService.AddMatchScore();
            UpdateScoreUI();
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;
        }
        else
        {
            audioService.PlayMismatchSound();
            scoreService.ResetCombo();   // Reset combo on mismatch
            UpdateScoreUI();
            firstCard.FlipBack();
            secondCard.FlipBack();
        }
    }

    // Updates the score display in the UI
    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {scoreService.Score}";
    }
}
