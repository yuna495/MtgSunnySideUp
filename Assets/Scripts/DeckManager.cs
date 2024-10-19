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
    public float radius = 700f;  // 手札を扇状に配置するための半径
    public float maxAngle = 60f;  // 手札全体の最大角度

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

            // DeckViewにインスタンスとして追加されたオブジェクトにアクセスするためにリストで保持する
            newCard.name = "Card_" + i;
        }

        // 最初の7枚を手札に加える
        for (int i = 0; i < 7; i++) {
            // 手札に加えるカードをデッキから取得し、リストに追加
            Sprite card = deck[i];
            hand.Add(card);

            // DeckView内のカードオブジェクトを削除
            Destroy(decView.Find("Card_" + i).gameObject);
        }

        // デッキリストから手札に加えたカードを削除し、デッキリストを更新
        deck.RemoveRange(0, 7);

        // 残りのデッキを整理
        RearrangeDeckView();

        // 手札を配置
        ArrangeHand();
    }
    void RearrangeDeckView() {
        // DeckView内のすべてのカードを取得し、再配置
        for (int i = 0; i < deck.Count; i++) {
            GameObject remainingCard = decView.Find("Card_" + (i + 7)).gameObject;  // 残っているカードを探す
            if (remainingCard != null) {
                remainingCard.name = "Card_" + i;  // カード名を更新
                                                   // 必要に応じて位置を再設定
                remainingCard.transform.SetSiblingIndex(i);
            }
        }
    }

    // 手札のカードを円弧状に配置する
    void ArrangeHand() {

        int cardCount = hand.Count;

        // カードの枚数によって扇の広がりを調整
        float angleStep = Mathf.Min(maxAngle / (cardCount - 1), maxAngle / 2);  // 枚数に応じた角度のステップ
        float startAngle = -(angleStep * (cardCount - 1) / 2);  // 扇の左端の開始角度

        for (int i = 0; i < cardCount; i++) {
            GameObject handCard = Instantiate(cardPrefab, handArea);
            handCard.GetComponent<Image>().sprite = hand[i];

            // カードの角度を計算 (負の方向に調整)
            float angle = startAngle + (i * angleStep);  // カードが正しく配置されるように符号を調整
            float radian = angle * Mathf.Deg2Rad;

            // カードの位置を計算（円の中心からカードを配置）
            float x = Mathf.Sin(radian) * radius;
            float y = Mathf.Cos(radian) * radius;

            // カードの位置と回転を設定
            handCard.transform.localPosition = new Vector3(x, y - 1100, 0);
            handCard.transform.rotation = Quaternion.Euler(0, 0, -angle);  // カードが扇状に広がるように角度を反転
        }
    }
    public void AddCardFromDeckToHand() {
        if (deck.Count > 0) {
            // 1番目のカードを取得
            Sprite card = deck[0];

            // 手札に追加
            hand.Add(card);

            // デッキリストから削除
            deck.RemoveAt(0);

            // DeckViewの一番目のカードを削除
            Destroy(decView.GetChild(0).gameObject);

            // 手札を再配置
            ArrangeHand();
        }
        else {
            Debug.Log("デッキが空です");
        }
    }
    // カードをデッキの先頭に戻す
    public void ReturnCardToDeckTop(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // 手札から削除
        hand.Remove(cardSprite);

        // デッキリストの先頭に追加
        deck.Insert(0, cardSprite);

        // 手札からカードを削除（UI上のカードを削除）
        Destroy(card);

        // 手札を再配置
        UpdateHand();

        // DeckViewを更新
        UpdateDeckView();
    }
    // カードをデッキの最後に戻す
    public void ReturnCardToDeckBottom(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // 手札から削除
        hand.Remove(cardSprite);

        // デッキリストの最後に追加
        deck.Add(cardSprite);

        // 手札からカードを削除（UI上のカードを削除）
        Destroy(card);

        // 手札を再配置
        UpdateHand();

        // DeckViewを更新
        UpdateDeckView();
    }
    // 手札の再配置
    void UpdateHand() {
        // 既存の手札をクリア（UI上で二重表示を防ぐ）
        foreach (Transform child in handArea) {
            Destroy(child.gameObject);
        }
        ArrangeHand();
    }
    // DeckViewを更新してカードを再表示
    void UpdateDeckView() {
        // DeckView内の既存のカードをすべて削除してから再表示
        foreach (Transform child in decView) {
            Destroy(child.gameObject);
        }

        // デッキリストを基に新しくカードを表示
        for (int i = 0; i < deck.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, decView);
            newCard.GetComponent<Image>().sprite = deck[i];
        }
    }
}
