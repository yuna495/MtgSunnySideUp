using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Transform originalParent;  // 元の親オブジェクト
    private CanvasGroup canvasGroup;   // ドラッグ中のレイキャストブロック用
    private Canvas canvas;             // Canvasの参照

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();  // 親のCanvasを取得
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;  // ドラッグ中はレイキャストを無効にする
    }

    public void OnDrag(PointerEventData eventData) {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;  // マウスに追従
    }
    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.blocksRaycasts = true;  // レイキャストを再有効化
        Debug.Log("EndDrag: Card parent is now " + transform.parent.name);
        // ドラッグされたカードの処理
        GameObject selectedCard = this.gameObject;
        Sprite cardSprite = selectedCard.GetComponent<Card>().cardImage.sprite;

        Debug.Log("Original Parent: " + originalParent.name);
        Debug.Log("Current Parent: " + transform.parent.name);
        if (transform.parent == originalParent) {
            transform.position = originalParent.position;
            Debug.Log("Card returned to original position.");
        }
        else {
            Debug.Log("Card successfully dropped into: " + transform.parent.name);
        }

        if (originalParent.name == "Hand") {
            // HandManagerをシーン内から探す
            HandManager handManager = Object.FindFirstObjectByType<HandManager>();
            if (handManager != null) {
                Debug.Log("Remove from hand list called");
                handManager.RemoveCardFromHand(cardSprite);  // Remove from hand list
            }
            else {
                // HandManager が見つからなかった場合の処理
                Debug.LogError("HandManager が見つかりませんでした。");
            }

            if (transform.parent == originalParent) {
                handManager.AddCardToHand(cardSprite);
                Destroy(gameObject);  // ドロップ失敗時は消去
            }
        }
        else if (originalParent.name == "DeckView") {
            DeckManager deckManager = Object.FindFirstObjectByType<DeckManager>();
            if (deckManager != null) {
                // DeckManager が見つかった場合の処理
                deckManager.RemoveCardFromDeck(cardSprite);
            }
            else {
                // DeckManager が見つからなかった場合の処理
                Debug.LogError("DeckManager が見つかりませんでした。");
            }
            if (transform.parent == originalParent) {
                deckManager.AddCardToDeck(cardSprite);
                Destroy(gameObject);  // ドロップ失敗時は消去
            }
        }
        else {
            if (transform.parent == originalParent) {
                transform.position = originalParent.position;  // ドロップ失敗時は元に戻す
            }
        }
    }
}
