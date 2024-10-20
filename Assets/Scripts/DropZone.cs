using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler {

    public void OnDrop(PointerEventData eventData) {
        GameObject droppedCard = eventData.pointerDrag;
        Sprite droppedCardSprite = droppedCard.GetComponent<Card>().cardImage.sprite;
        if (droppedCard != null) {
            Transform newParent = transform;
            // Handエリアの場合の処理
            if (newParent.name == "Hand") {
                Debug.Log("Card dropped into Hand");
                HandManager handManager = Object.FindFirstObjectByType<HandManager>();
                if (handManager != null) {
                    // 手札にカードが存在するかを確認
                    if (!handManager.IsCardInHand(droppedCardSprite)) {
                        handManager.AddCardToHand(droppedCardSprite);
                        Destroy(droppedCard);
                    }
                    // 元の親が DeckView だった場合、デッキから削除
                    if (droppedCard.transform.parent.name == "DeckView") {
                        DeckManager deckManager = Object.FindFirstObjectByType<DeckManager>();
                        if (deckManager != null) {
                            Debug.Log("Remove from DeckView list called");
                            deckManager.RemoveCardFromDeck(droppedCardSprite);  // デッキから削除
                        }
                        else {
                            Debug.LogError("DeckManager が見つかりませんでした。");
                        }
                    }
                }
            }
            // DeckViewエリアの場合の処理
            else if (newParent.name == "DeckView") {
                DeckManager deckManager = Object.FindFirstObjectByType<DeckManager>();
                if (deckManager != null) {
                    Debug.Log("Attempting to add card to deck: " + droppedCardSprite.name);
                    if (!deckManager.IsCardInDeck(droppedCardSprite)) {
                        deckManager.AddCardToDeck(droppedCardSprite);
                        Debug.Log("Card added to deck, now destroying dropped card.");
                        // 元の親がHandだった場合、手札から削除
                        if (droppedCard.transform.parent.name == "Hand") {
                            HandManager handManager = Object.FindFirstObjectByType<HandManager>();
                            if (handManager != null) {
                                Debug.Log("Remove from hand list called");
                                handManager.RemoveCardFromHand(droppedCardSprite);  // 手札から削除
                            }
                            else {
                                Debug.LogError("HandManager が見つかりませんでした。");
                            }
                        }

                        // ドロップしたカードを削除
                        Destroy(droppedCard);
                    }
                    else {
                        Debug.LogWarning("This card already exists in the deck.");
                    }
                }
                else {
                    Debug.LogError("DeckManager not found.");
                }
            }
            else {
                // その他のエリアの場合
                droppedCard.transform.SetParent(newParent);
                droppedCard.transform.localPosition = Vector3.zero;

                Quaternion cardRotation = droppedCard.transform.rotation;
                if (cardRotation.eulerAngles.z != 0 && cardRotation.eulerAngles.z != 90) {
                    droppedCard.transform.rotation = Quaternion.Euler(cardRotation.eulerAngles.x, cardRotation.eulerAngles.y, 0);  // Z軸の回転を0度にリセット
                }
            }

        }
    }
}
