using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler {
    public Image cardImage;  // カードに画像を表示するためのImageコンポーネント
    private ExpansionViewManager expansionViewManager;  // ExpansionViewManagerへの参照

    // 生成時に ExpansionViewManager を自動で探す
    private void Awake() {
        expansionViewManager = Object.FindFirstObjectByType<ExpansionViewManager>();
        if (expansionViewManager == null) {
            Debug.LogError("ExpansionViewManager not found in the scene.");
        }
    }

    // カードの画像を設定するメソッド
    public void SetCardImage(Sprite newImage) {
        cardImage.sprite = newImage;   // 新しい画像を設定
    }

    // 右クリック時に呼び出される
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            // ExpansionViewにカード情報を渡して拡大表示
            if (expansionViewManager != null) {
                expansionViewManager.DisplayExpansionCard(cardImage.sprite);
            }
        }
    }
}
