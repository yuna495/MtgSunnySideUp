using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour {
    public GameObject cardPrefab;  // カードPrefab
    public Transform cardParent;   // カードを配置する親オブジェクト
    public Transform decView;      // デッキ表示用のUIパネル
    public Transform handArea;     // 手札表示用のUIパネル
    private List<Sprite> deck = new List<Sprite>();  // デッキのカードリスト
    private List<Sprite> hand = new List<Sprite>();  // 手札のカードリスト

    void Start() {
        CreateDeck();    // デッキの作成
        ShuffleDeck();   // デッキをシャッフル
        DealCards();     // 手札にカードを配る
    }

    // デッキを生成する
    void CreateDeck() {
        // Resources/Sprites/Cardsフォルダからカード画像を読み込む
        Sprite[] cardImages = Resources.LoadAll<Sprite>("Sprites/Cards");
        Debug.Log("Card images loaded: " + cardImages.Length);

        // デッキにカードを追加
        for (int i = 0; i < cardImages.Length; i++) {
            deck.Add(cardImages[i]);
        }
    }

    // デッキをシャッフルする
    void ShuffleDeck() {
        System.Random rnd = new System.Random();
        for (int i = deck.Count - 1; i > 0; i--) {
            int swapIndex = rnd.Next(i + 1);
            Sprite temp = deck[i];
            deck[i] = deck[swapIndex];
            deck[swapIndex] = temp;
        }
    }

    // カードをデッキ（DecView）に並べ、手札に配る
    void DealCards() {
        // デッキ全体を表示
        for (int i = 0; i < deck.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, decView);
            newCard.GetComponent<Image>().sprite = deck[i];
            if (i < 7) hand.Add(deck[i]);  // 最初の7枚を手札に
        }

        // 手札を配置
        ArrangeHand();
    }

    // 手札のカードを円弧状に配置する
    void ArrangeHand() {
        float radius = 200f;  // 手札を円弧状に配置する際の半径
        float angleStep = Mathf.PI / (hand.Count + 1);  // 角度のステップ

        for (int i = 0; i < hand.Count; i++) {
            GameObject handCard = Instantiate(cardPrefab, handArea);
            handCard.GetComponent<Image>().sprite = hand[i];

            // 円弧上に配置
            float angle = angleStep * (i + 1);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            handCard.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}
