using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {
    public GameObject toggleDeckViewButton;  // トグルボタン
    public GameObject deckViewPanel;         // デッキビューのパネル

    void Start() {
        // ゲーム開始時にdeckViewPanelを非表示にする
        deckViewPanel.SetActive(false);

        // ボタンにクリックイベントリスナーを追加
        toggleDeckViewButton.GetComponent<Button>().onClick.AddListener(ToggleDeckView);
    }

    // DeckViewパネルの表示/非表示をトグル
    void ToggleDeckView() {
        // 現在の状態をトグルする
        bool isActive = deckViewPanel.activeSelf;
        deckViewPanel.SetActive(!isActive);
    }
}
