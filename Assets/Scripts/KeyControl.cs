using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyControl : MonoBehaviour {
    public DeckManager deckManager;     // DeckManagerスクリプトへの参照
    public FieldManager fieldManager;   // FieldManagerスクリプトへの参照
    public GraphicRaycaster raycaster;  // CanvasにアタッチされているGraphicRaycaster
    public EventSystem eventSystem;     // シーンに配置されているEventSystem

    // Update is called once per frame
    void Update() {
        // Wキーで1番目のカードを手札に加える
        if (Input.GetKeyDown(KeyCode.W)) {
            deckManager.AddCardFromDeckToHand();
        }
        // A,D,Sキーによるカードの操作
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)) {
            CardInteraction((KeyCode)Input.inputString[0]);
        }
        // Eキーでの操作
        if (Input.GetKeyDown(KeyCode.E)) {
            fieldManager.CardRotation();        // FieldManagerのCardRotationメソッドを呼び出す
        }
        // R,F,G,Hキーでカードの親を変更
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H)) {
            fieldManager.CardParentChange((KeyCode)Input.inputString[0]);
        }
    }

    // カード操作を行う
    void CardInteraction(KeyCode key) {
        PointerEventData pointerData = new PointerEventData(eventSystem) {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        if (results.Count == 0) {
            Debug.Log("UI要素にヒットしませんでした。");
        }

        foreach (RaycastResult result in results) {
            Debug.Log("ヒットしたオブジェクト: " + result.gameObject.name);

            if (result.gameObject.CompareTag("Card")) {
                GameObject selectedCard = result.gameObject;

                // カードの親オブジェクトがDeckViewかどうかを確認
                if (key == KeyCode.S && selectedCard.transform.parent == deckManager.deckView) {
                    Debug.Log("DeckView内のカードが選択されました。");
                    deckManager.AddCardToHandFromDeck(selectedCard);    // SキーでDeckViewからカードを手札に加える
                }
                else if (key == KeyCode.A) {
                    deckManager.ReturnCardToDeckTop(selectedCard);      // Aキーでカードをデッキの先頭に戻す
                }
                else if (key == KeyCode.D) {
                    deckManager.ReturnCardToDeckBottom(selectedCard);   // Dキーでカードをデッキの最後に戻す
                }

                break;
            }
        }
    }
}
