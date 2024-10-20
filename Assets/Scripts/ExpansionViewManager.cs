using UnityEngine;
using UnityEngine.UI;

public class ExpansionViewManager : MonoBehaviour {
    public GameObject expansionView;         // 拡大表示用のパネル
    public GameObject expansionCardPrefab;   // 拡大表示用のカードPrefab
    public Transform expansionCardParent;    // 拡大表示カードの親となるTransform

    // ExpansionViewにカードのPrefabを表示するメソッド
    public void DisplayExpansionCard(Sprite cardSprite) {
        // 以前の表示をクリア
        foreach (Transform child in expansionCardParent) {
            Destroy(child.gameObject);
        }

        // ExpansionCardPrefabを生成
        GameObject newCard = Instantiate(expansionCardPrefab, expansionCardParent);
        newCard.GetComponent<Image>().sprite = cardSprite;  // カードの画像を設定

        // ExpansionViewをアクティブにする
        expansionView.SetActive(true);
    }

    // ExpansionViewを閉じるメソッド
    public void CloseExpansionView() {
        expansionView.SetActive(false);
    }
}
