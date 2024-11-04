using UnityEngine;
using System.Collections.Generic;

public class CardLoader : MonoBehaviour
{
    public Dictionary<string, GameObject> cardPrefabs = new();

    private void Awake()
    {
        LoadCardPrefabs();
    }

    // Loads all card prefabs from the Resources/Prefabs folder
    private void LoadCardPrefabs()
    {
        GameObject[] loadedPrefabs = Resources.LoadAll<GameObject>("Prefabs");

        foreach (GameObject prefab in loadedPrefabs)
        {
            if (prefab != null)
            {
                // Use prefab name as key (ensure it’s unique)
                cardPrefabs[prefab.name] = prefab;
            }
        }
    }

    // Retrieves a specific card prefab by name
    public GameObject GetCardPrefab(string cardName)
    {
        if (cardPrefabs.TryGetValue(cardName, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"Card prefab with name {cardName} not found!");
        return null;
    }
}
