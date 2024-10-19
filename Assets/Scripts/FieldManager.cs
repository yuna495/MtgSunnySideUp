using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class FieldManager : MonoBehaviour {
    public Transform deckView;        // DeckViewを親に持たないカードを対象とするために必要
    public Transform grave;
    public Transform field;
    public Transform land;
    public Transform hand;
    private bool isRotate = false;     // 回転状態をトグルするフラグ
    public DeckManager deckManager;  // DeckManagerの参照を追加
    public GraphicRaycaster raycaster; // CanvasにアタッチされているGraphicRaycaster
    public EventSystem eventSystem;   // シーンに配置されているEventSystem

    // カードを回転させるメソッド
    public void CardRotation() {
        PointerEventData pointerData = new PointerEventData(eventSystem) {
            position = Input.mousePosition
        };

        // レイキャストでヒットしたUI要素を格納するリスト
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // レイキャストでヒットしたオブジェクトをループ処理
        foreach (RaycastResult result in results) {
            if (result.gameObject.CompareTag("Card")) {
                GameObject targetCard = result.gameObject;

                // カードの親オブジェクトがDeckViewかつHandでないか確認
                if (targetCard.transform.parent != deckView && targetCard.transform.parent != hand) {
                    // カードが回転していない場合
                    if (!isRotate) {
                        targetCard.transform.Rotate(0, 0, 90);  // 90度回転
                        isRotate = true;
                    }
                    // カードが回転している場合
                    else {
                        targetCard.transform.Rotate(0, 0, -90); // 元に戻す（-90度回転）
                        isRotate = false;
                    }
                }
                break; // 最初にヒットしたカードのみ処理
            }
        }
    }
    public void CardParentChange(KeyCode key) {
        PointerEventData pointerData = new PointerEventData(eventSystem) {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results) {
            if (result.gameObject.CompareTag("Card")) {
                GameObject selectedCard = result.gameObject;

                // Handエリアにあるカードかどうかを確認
                if (selectedCard.transform.parent == hand) {
                    // Handにあるカードは手札から削除し、新しいエリアに移動
                    switch (key) {
                        case KeyCode.R:
                            deckManager.MoveCardToArea(selectedCard, land);
                            break;
                        case KeyCode.F:
                            deckManager.MoveCardToArea(selectedCard, field);
                            break;
                        case KeyCode.G:
                            deckManager.MoveCardToArea(selectedCard, grave);
                            break;
                    }
                }
                else {
                    // Handエリアにない場合は単純に親を変更
                    switch (key) {
                        case KeyCode.R:
                            selectedCard.transform.SetParent(land);
                            Debug.Log("Card moved to Land.");
                            break;
                        case KeyCode.F:
                            selectedCard.transform.SetParent(field);
                            Debug.Log("Card moved to Field.");
                            break;
                        case KeyCode.G:
                            selectedCard.transform.SetParent(grave);
                            Debug.Log("Card moved to Grave.");
                            break;
                        case KeyCode.H:
                            //選択したカードを手札リストに追加し、手札を更新。選択したカードを削除
                            deckManager.UpdateHandList(selectedCard);
                            Destroy(selectedCard);
                            break;
                    }
                }
                break; // 最初にヒットしたカードのみ処理
            }
        }
    }


}