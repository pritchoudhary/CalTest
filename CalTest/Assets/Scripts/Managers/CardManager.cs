using System.Collections.Generic;
using UnityEngine;

// Manages card instantiation, layout, and shuffling.
public class CardManager : MonoBehaviour
{
    [SerializeField] private CardLoader cardLoader;                // Loads card prefabs
    [SerializeField] private Transform cardContainer;              // Parent container for card instances
    [SerializeField] private GridLayoutManager gridLayoutManager;  // Manages card grid layout

    // Initializes cards by generating pairs, shuffling, and positioning them
    public void InitializeCards(System.Action<Card> onCardFlipped)
    {
        int numCards = gridLayoutManager.rows * gridLayoutManager.columns;
        List<string> cardNames = GenerateCardPairs(numCards / 2); // Generate pairs
        Shuffle(cardNames); // Shuffle the list

        for (int i = 0; i < numCards; i++)
        {
            Vector3 position = gridLayoutManager.GetCardPosition(i);
            GameObject cardObject = InstantiateCard(cardNames[i], position);

            if (cardObject != null && cardObject.TryGetComponent<Card>(out var cardComponent))
            {
                cardComponent.CardID = cardNames[i];                // Set unique ID
                cardComponent.OnFlipped += onCardFlipped;           // Subscribe to flip event
            }
        }
    }

    // Instantiates a card prefab and sets its position
    private GameObject InstantiateCard(string cardName, Vector3 position)
    {
        GameObject cardPrefab = cardLoader.GetCardPrefab(cardName);
        if (cardPrefab == null) return null;

        GameObject cardInstance = Instantiate(cardPrefab, cardContainer);
        cardInstance.transform.localPosition = position;
        cardInstance.transform.localScale = new Vector3(1500, 1500, 1);
        cardInstance.transform.localRotation = Quaternion.Euler(0, 180, 0); // Start back-facing
        return cardInstance;
    }

    // Generates pairs of card names for matching, selecting random cards each time
    private List<string> GenerateCardPairs(int numPairs)
    {
        List<string> pairs = new();
        List<string> availableCardNames = new(cardLoader.cardPrefabs.Keys);

        // Shuffle the list of available card names to ensure random selection each playthrough
        Shuffle(availableCardNames);

        // Select the first `numPairs` unique card names to create pairs
        for (int i = 0; i < numPairs; i++)
        {
            string cardID = availableCardNames[i];
            pairs.Add(cardID); // Add the first card of the pair
            pairs.Add(cardID); // Add the second card of the pair
        }

        // Shuffle the pairs list to randomize card placement in the grid
        Shuffle(pairs);
        return pairs;
    }


    // Shuffles the list of card names to randomize card positions
    private void Shuffle(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }
}
