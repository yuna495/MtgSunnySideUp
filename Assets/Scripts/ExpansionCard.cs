using UnityEngine;
using UnityEngine.EventSystems;

public class ExpansionCard : MonoBehaviour, IPointerClickHandler {
    private GameObject expansionView;

    // Start is called before the first frame update
    void Start() {
        // ExpansionView をシーンから探す
        expansionView = GameObject.Find("ExpansionView");

        if (expansionView == null) {
            Debug.LogError("ExpansionView が見つかりませんでした");
        }
    }

    // IPointerClickHandler の OnPointerClick を実装
    public void OnPointerClick(PointerEventData eventData) {
        // 右クリック時に反応する
        if (eventData.button == PointerEventData.InputButton.Right) {
            if (expansionView != null) {
                // ExpansionView を非表示にする
                expansionView.SetActive(false);
                Debug.Log("ExpansionView を非表示にしました");
            }

            // このオブジェクト（ExpansionCard）を削除する
            Destroy(gameObject);
            Debug.Log("ExpansionCardPrefab を削除しました");
        }
    }
}
