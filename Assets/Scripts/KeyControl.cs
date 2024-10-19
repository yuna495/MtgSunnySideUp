using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyControl : MonoBehaviour {
    public DeckManager deckManager;  // DeckManagerスクリプトへの参照
    public GraphicRaycaster raycaster;  // CanvasにアタッチされているGraphicRaycaster
    public EventSystem eventSystem;  // シーンに配置されているEventSystem

    // Update is called once per frame
    void Update() {
        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.W)) {
            // デッキリストから1番目のカードを手札に加えるメソッドを呼び出す
            deckManager.AddCardFromDeckToHand();
        }

        // Aキーが押されたら、カードをデッキの先頭に戻す
        if (Input.GetKeyDown(KeyCode.A)) {
            Debug.Log("Aキーが押されました");
            CardInteraction(KeyCode.A);
        }

        // Dキーが押されたら、カードをデッキの最後に戻す
        if (Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Dキーが押されました");
            CardInteraction(KeyCode.D);
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
                Debug.Log("カードが選択されました: " + result.gameObject.name);
                GameObject selectedCard = result.gameObject;
                if (key == KeyCode.A) {
                    deckManager.ReturnCardToDeckTop(selectedCard);
                }
                else if (key == KeyCode.D) {
                    deckManager.ReturnCardToDeckBottom(selectedCard);
                }
                break;
            }
        }
    }
}
