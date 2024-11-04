using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

// Manages the main game logic, including card instantiation, matching, sound, and scoring.
public class GameManager : MonoBehaviour
{
    [SerializeField] private CardLoader cardLoader;           // Reference to the CardLoader script to load card prefabs
    [SerializeField] private Transform cardContainer;         // Parent transform for card instances in the grid layout
    [SerializeField] private GridLayoutManager gridLayoutManager; // Manages card positions in a grid
    [SerializeField] private TextMeshProUGUI scoreText;        // Reference to the score UI element

    private AudioService audioService;                        // Service for managing audio playback
    private ScoreService scoreService;                        // Service for managing scores and combo multipliers

    private readonly List<ICard> instantiatedCards = new();   // List to track instantiated cards for gameplay
    private Card firstFlippedCard = null;                     // Reference to the first flipped card
    private Card secondFlippedCard = null;                    // Reference to the second flipped card

    private void Start()
    {
        // Initialize services
        audioService = GetComponent<AudioService>();          // Get AudioService component attached to GameManager
        scoreService = new ScoreService();                    // Initialize a new instance of ScoreService

        InitializeGame();                                     // Set up the game with cards and initial layout
        UpdateScoreUI();                                      // Initialize the score UI to display the starting score
    }

    // Initializes the game by generating card pairs and arranging them in a grid
    private void InitializeGame()
    {
        int numCards = gridLayoutManager.rows * gridLayoutManager.columns; // Total number of cards needed
        List<string> cardNames = GenerateCardPairs(numCards / 2);          // Generates pairs of card names for matching

        Shuffle(cardNames);                                                // Shuffle the list to randomize card positions

        for (int i = 0; i < numCards; i++)
        {
            Vector3 position = gridLayoutManager.GetCardPosition(i);       // Get the position for each card in the grid
            GameObject cardObject = InstantiateCard(cardNames[i], position); // Instantiate the card at the specified position

            if (cardObject != null)
            {
                if (cardObject.TryGetComponent<Card>(out var cardComponent))
                {
                    cardComponent.CardID = cardNames[i];                   // Assign a unique ID for matching purposes
                    cardComponent.OnFlipped += HandleCardFlipped;          // Subscribe to the flip event
                    instantiatedCards.Add(cardComponent);                  // Add the card to the list of instantiated cards
                }
            }
        }
    }

    // Instantiates a card prefab at the given position and sets its initial properties
    private GameObject InstantiateCard(string cardName, Vector3 localPosition)
    {
        GameObject cardPrefab = cardLoader.GetCardPrefab(cardName);        // Retrieve the prefab from the card loader

        if (cardPrefab != null)
        {
            GameObject cardInstance = Instantiate(cardPrefab, cardContainer); // Instantiate the card as a child of cardContainer
            cardInstance.transform.localPosition = localPosition;          // Position the card in the grid
            cardInstance.transform.localScale = new Vector3(1500, 1500, 1);// Set scale to fit the UI layout
            cardInstance.transform.localRotation = Quaternion.Euler(0, 180, 0); // Start the card back-facing
            return cardInstance;
        }
        return null;
    }

    // Handles the event when a card is flipped
    private void HandleCardFlipped(Card flippedCard)
    {
        if (firstFlippedCard == null)
        {
            firstFlippedCard = flippedCard;
            audioService.PlayFlipSound();
        }
        else if (secondFlippedCard == null && firstFlippedCard != flippedCard)
        {
            secondFlippedCard = flippedCard;
            audioService.PlayFlipSound();
            StartCoroutine(CheckMatch()); // Start checking for a match when two cards are flipped
        }
    }

    // Coroutine to check for a match between two flipped cards
    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f); // Delay for half a second to show both cards

        if (firstFlippedCard.IsMatch(secondFlippedCard))
        {
            audioService.PlayMatchSound();
            scoreService.AddMatchScore();
            UpdateScoreUI();
            firstFlippedCard.IsMatched = true;
            secondFlippedCard.IsMatched = true;
        }
        else
        {
            audioService.PlayMismatchSound();
            scoreService.ResetCombo();   // Reset the combo multiplier on mismatch
            UpdateScoreUI();             // Update the score display in case the UI shows the combo multiplier
            firstFlippedCard.FlipBack();
            secondFlippedCard.FlipBack();
        }

        // Reset the selected cards after checking
        firstFlippedCard = null;
        secondFlippedCard = null;
    }

    // Updates the score displayed in the UI
    private void UpdateScoreUI()
    {
        scoreText.text = $"Score: {scoreService.Score}";                   // Update the scoreText with the current score
    }

    // Generates a list of unique card IDs to create matching pairs
    private List<string> GenerateCardPairs(int numPairs)
    {
        List<string> pairs = new();
        List<string> availableCardNames = new(cardLoader.cardPrefabs.Keys);

        // Check if there are enough unique card names available
        if (availableCardNames.Count < numPairs)
        {
            Debug.LogError("Not enough unique card types available for the desired number of pairs.");
            return pairs;
        }

        Shuffle(availableCardNames); // Shuffle available cards to randomize the pairs

        // Select the first `numPairs` unique card names to create pairs
        for (int i = 0; i < numPairs; i++)
        {
            string cardID = availableCardNames[i];
            pairs.Add(cardID); // Add the first card of the pair
            pairs.Add(cardID); // Add the second card of the pair
        }

        return pairs;
    }

    // Shuffles a list of strings randomly
    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }
}
