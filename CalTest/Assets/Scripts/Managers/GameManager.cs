using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Manages the main game logic, including card instantiation, matching, sound, and scoring.
public class GameManager : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;               // Manages card instantiation and shuffling
    [SerializeField] private TextMeshProUGUI scoreText;             // UI element to display score
    [SerializeField] private GameObject nextLevelButton;            // Button to go to the next level
    [SerializeField] private AudioService audioService;             // Manages audio playback
    private ScoreService scoreService;                              // Manages scoring and combo system

    private readonly Queue<Card> flippedCardsQueue = new Queue<Card>(); // Queue to manage flipped cards for match checks

    private void Start()
    {
        scoreService = new ScoreService();                          // Initialize score service
        cardManager.InitializeCards(HandleCardFlipped);             // Set up cards with flip event handler
        UpdateScoreUI();                                            // Display the initial score
        nextLevelButton.SetActive(false);                           // Hide the button at the start
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

            // Check if all cards are matched
            if (AreAllCardsMatched())
            {
                ShowNextLevelButton(); // Show the button to go to the next level
            }
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

    // Check if all cards have been matched
    private bool AreAllCardsMatched()
    {
        foreach (Card card in cardManager.GetAllCards())
        {
            if (!card.IsMatched)
            {
                return false; // If any card is unmatched, return false
            }
        }
        return true; // All cards are matched
    }

    // Show the next level button
    private void ShowNextLevelButton()
    {
        nextLevelButton.SetActive(true);   // Activate the next level button
        cardManager.DeactivateCardContainer(); // Deactivate the card container
    }

    // Updates the score display in the UI
    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {scoreService.Score}";
    }

    // Load the next level
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1; // Get the next scene index
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) // Check if there is a next scene
        {
            SceneManager.LoadScene(nextSceneIndex); // Load the next level
        }
    }

    // Button Click functionality to return to the MainMenu
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        scoreService.ResetScore(); // Reset the score
        PlayerPrefs.DeleteAll(); // Clear saved data
        SceneManager.LoadScene("MainMenu"); // Return to main menu
    }
}
