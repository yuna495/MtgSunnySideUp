using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour {
    public GameObject cardPrefab;  // Card prefab
    public Transform deckView;     // UI for displaying the deck
    public HandManager handManager; // Reference to HandManager
    private List<Sprite> deck = new List<Sprite>();  // List to store the deck

    void Start() {
        CreateDeck();    // Create the deck
        ShuffleDeck();   // Shuffle the deck
        DealCards();     // Deal the initial hand
    }

    // Create the deck from card images
    void CreateDeck() {
        Sprite[] cardImages = Resources.LoadAll<Sprite>("Sprites/Cards");
        Debug.Log("Card images loaded: " + cardImages.Length);

        foreach (Sprite cardImage in cardImages) {
            deck.Add(cardImage);
        }
    }

    // Shuffle the deck
    public void ShuffleDeck() {
        System.Random rnd = new System.Random();
        for (int i = deck.Count - 1; i > 0; i--) {
            int swapIndex = rnd.Next(i + 1);
            Sprite temp = deck[i];
            deck[i] = deck[swapIndex];
            deck[swapIndex] = temp;
        }
        UpdateDeckView(); // Update the deck display after shuffling
    }

    // Deal the first 7 cards to the hand
    void DealCards() {
        for (int i = 0; i < 7; i++) {
            if (deck.Count > 0) {
                Sprite card = deck[0];
                handManager.AddCardToHand(card);  // Add to the hand
                deck.RemoveAt(0);
                Destroy(deckView.GetChild(0).gameObject);  // Remove from deckView
            }
        }
        UpdateDeckView(); // Update the deck display after dealing
    }

    // Add card from deck to hand
    public void AddCardFromDeckToHand() {
        if (deck.Count > 0) {
            Sprite card = deck[0];
            handManager.AddCardToHand(card);  // HandManager handles adding the card
            deck.RemoveAt(0);
            Destroy(deckView.GetChild(0).gameObject);  // Remove from UI
            UpdateDeckView();  // Update deck UI
        }
        else {
            Debug.Log("Deck is empty!");
        }
    }

    // Add card to hand from DeckView UI
    public void AddCardToHandFromDeck(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        handManager.AddCardToHand(cardSprite);  // HandManager handles hand logic
        deck.Remove(cardSprite);  // Remove card from deck list
        Destroy(card);  // Remove card from DeckView UI
        UpdateDeckView(); // Update deck display
    }

    // Return card to top of the deck from hand
    public void ReturnCardToDeckTop(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        handManager.RemoveCardFromHand(cardSprite);  // Remove from hand
        deck.Insert(0, cardSprite);  // Insert at top of the deck
        Destroy(card);  // Remove from hand UI
        UpdateHandAndDeck();  // Update both hand and deck
    }

    // Return card to bottom of the deck from hand
    public void ReturnCardToDeckBottom(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        handManager.RemoveCardFromHand(cardSprite);  // Remove from hand
        deck.Add(cardSprite);  // Add to the bottom of the deck
        Destroy(card);  // Remove from hand UI
        UpdateHandAndDeck();  // Update both hand and deck
    }

    // Move card from deck to a specific area (e.g., grave, field)
    public void MoveCardToAreaFromDeck(GameObject card, Transform area) {
        // カードのSpriteを取得
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // デッキからカードを削除
        if (deck.Contains(cardSprite)) {
            deck.Remove(cardSprite);  // デッキからカードを削除

            // カードを新しいエリアに移動 (複製せず、既存のオブジェクトの親を変更)
            card.transform.SetParent(area);  // 親を新しいエリアに変更
            card.transform.localPosition = Vector3.zero;  // 位置をリセット (必要なら適切に調整)

            Debug.Log("カードが " + area.name + " に移動しました。");

            UpdateDeckView();  // デッキビューを更新
        }
    }


    // Update the deck display in the UI
    public void UpdateDeckView() {
        foreach (Transform child in deckView) {
            Destroy(child.gameObject);  // Clear current cards
        }

        // Repopulate the deckView with the current deck
        for (int i = 0; i < deck.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, deckView);
            newCard.GetComponent<Image>().sprite = deck[i];
            newCard.name = "Card_" + i;
        }
    }

    // Update both hand and deck displays
    private void UpdateHandAndDeck() {
        handManager.UpdateHand();  // Update hand display
        UpdateDeckView();  // Update deck display
    }
}
