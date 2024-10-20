using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HandManager : MonoBehaviour {
    public GameObject cardPrefab;  // Prefab for creating cards in hand
    public Transform handArea;     // UI Panel for hand cards
    private List<Sprite> hand = new List<Sprite>();  // List to store hand cards (sprites)

    public float radius = 1100f;    // Radius for hand layout

    // Add a card to the hand
    public void AddCardToHand(Sprite cardSprite) {
        hand.Add(cardSprite);
        UpdateHand();  // Update hand display
    }

    // Remove a card from the hand
    public void RemoveCardFromHand(Sprite cardSprite) {
        Debug.Log("Removing card from hand: " + cardSprite.name);

        if (hand.Contains(cardSprite)) {
            hand.Remove(cardSprite);
            Debug.Log("Card removed successfully from hand.");
        }
        else {
            Debug.LogWarning("Card not found in hand.");
        }

        UpdateHand();  // 手札の表示を更新
    }

    // Move card from hand to another area (e.g., grave, field)
    public void MoveCardToAreaFromHand(GameObject card, Transform area) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        RemoveCardFromHand(cardSprite);  // Remove from hand list
        Destroy(card);  // Remove from UI

        // Instantiate card in the new area
        GameObject newCard = Instantiate(cardPrefab, area);
        newCard.GetComponent<Image>().sprite = cardSprite;
        Debug.Log("Card moved to " + area.name);
    }

    // ** New Method: Update the hand list and check if the card is already in hand **
    public void UpdateHandList(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // Add to hand if not already present
        if (!hand.Contains(cardSprite)) {
            hand.Add(cardSprite);
            UpdateHand();  // Update hand display after adding new card
        }
    }

    // Update the hand display
    public void UpdateHand() {
        foreach (Transform child in handArea) {
            Destroy(child.gameObject);  // Clear current cards
        }

        // Arrange the new hand in a fan layout
        ArrangeHand();
    }

    // Arrange the hand in a fan layout
    private void ArrangeHand() {
        int cardCount = hand.Count;

        // カードが2枚のときは5度、3枚のときは10度、... 7枚のときは30度、最大で50度まで広げる
        float totalAngle = Mathf.Min(5f * (cardCount - 1), 50f);  // 最大50度まで広げる
        float angleStep = (cardCount > 1) ? totalAngle / (cardCount - 1) : 0;  // カード間の角度ステップ
        float startAngle = -(totalAngle / 2);  // 扇状に中央から広げる

        for (int i = 0; i < cardCount; i++) {
            GameObject handCard = Instantiate(cardPrefab, handArea);
            handCard.GetComponent<Image>().sprite = hand[i];

            // カードの位置と回転を計算
            float angle = startAngle + (i * angleStep);  // カード間の角度
            float radian = angle * Mathf.Deg2Rad;
            float x = Mathf.Sin(radian) * radius;
            float y = Mathf.Cos(radian) * radius;

            handCard.transform.localPosition = new Vector3(x, y - 1100, 0);
            handCard.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }


    public bool IsCardInHand(Sprite cardSprite) {
        return hand.Contains(cardSprite);  // 手札にすでにカードがあるか確認
    }
}
