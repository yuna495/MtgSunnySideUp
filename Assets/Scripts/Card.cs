using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    public Image cardImage;  // カードに画像を表示するためのImageコンポーネント

    // カードの画像を設定するメソッド
    public void SetCardImage(Sprite newImage) {
        cardImage.sprite = newImage;   // 新しい画像を設定
    }
}
