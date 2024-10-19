using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour {
    public GameObject cardPrefab;  // カードPrefab
    public Transform cardParent;   // カードを配置する親オブジェクト
    public Transform deckView;     // デッキ表示用のUIパネル
    public Transform handArea;     // 手札表示用のUIパネル
    private List<Sprite> deck = new List<Sprite>();  // デッキのカードリスト
    private List<Sprite> hand = new List<Sprite>();  // 手札のカードリスト
    public float radius = 700f;    // 手札を扇状に配置するための半径
    public float maxAngle = 60f;   // 手札全体の最大角度

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
    public void ShuffleDeck() {
        System.Random rnd = new System.Random();
        for (int i = deck.Count - 1; i > 0; i--) {
            int swapIndex = rnd.Next(i + 1);
            Sprite temp = deck[i];
            deck[i] = deck[swapIndex];
            deck[swapIndex] = temp;
        }
    }

    // カードをデッキに並べ、手札に配る
    void DealCards() {
        // デッキ全体を表示
        for (int i = 0; i < deck.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, deckView);
            newCard.GetComponent<Image>().sprite = deck[i];

            // DeckViewにインスタンスとして追加されたオブジェクトにアクセスするためにリストで保持
            newCard.name = "Card_" + i;
        }

        // 最初の7枚を手札に加える
        for (int i = 0; i < 7; i++) {
            Sprite card = deck[i];
            hand.Add(card);

            // DeckView内のカードオブジェクトを削除
            Destroy(deckView.Find("Card_" + i).gameObject);
        }

        // デッキリストから手札に加えたカードを削除し、デッキリストを更新
        deck.RemoveRange(0, 7);

        // 残りのデッキを整理
        RearrangeDeckView();

        // 手札を配置
        ArrangeHand();
    }

    // DeckViewの再配置
    public void RearrangeDeckView() {
        for (int i = 0; i < deck.Count; i++) {
            GameObject remainingCard = deckView.Find("Card_" + (i + 7)).gameObject;
            if (remainingCard != null) {
                remainingCard.name = "Card_" + i;
                remainingCard.transform.SetSiblingIndex(i);
            }
        }
    }

    // 手札を円弧状に配置する
    void ArrangeHand() {
        int cardCount = hand.Count;
        float angleStep = Mathf.Min(maxAngle / (cardCount - 1), maxAngle / 2);
        float startAngle = -(angleStep * (cardCount - 1) / 2);

        for (int i = 0; i < cardCount; i++) {
            GameObject handCard = Instantiate(cardPrefab, handArea);
            handCard.GetComponent<Image>().sprite = hand[i];

            float angle = startAngle + (i * angleStep);
            float radian = angle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radian) * radius;
            float y = Mathf.Cos(radian) * radius;

            handCard.transform.localPosition = new Vector3(x, y - 1100, 0);
            handCard.transform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    // デッキから手札にカードを追加
    public void AddCardFromDeckToHand() {
        if (deck.Count > 0) {
            Sprite card = deck[0];
            hand.Add(card);
            deck.RemoveAt(0);

            Destroy(deckView.GetChild(0).gameObject);

            ArrangeHand();
        }
        else {
            Debug.Log("デッキが空です");
        }
    }

    // カードをデッキの先頭に戻す
    public void ReturnCardToDeckTop(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        hand.Remove(cardSprite);
        deck.Insert(0, cardSprite);
        Destroy(card);

        UpdateHand();
        UpdateDeckView();
    }

    // カードをデッキの最後に戻す
    public void ReturnCardToDeckBottom(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        hand.Remove(cardSprite);
        deck.Add(cardSprite);
        Destroy(card);

        UpdateHand();
        UpdateDeckView();
    }

    // デッキから手札にカードを移動
    public void AddCardToHandFromDeck(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;
        hand.Add(cardSprite);
        deck.Remove(cardSprite);
        Destroy(card);

        UpdateHand();
        UpdateDeckView();
    }
    // 手札からカードを削除し、各エリアにカードを移動
    public void MoveCardToAreaFromHand(GameObject card, Transform area) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // 手札から削除
        if (hand.Contains(cardSprite)) {
            hand.Remove(cardSprite);
            Destroy(card);  // 手札のUIから削除

            // カードを新しいエリアに移動
            GameObject newCard = Instantiate(cardPrefab, area);
            newCard.GetComponent<Image>().sprite = cardSprite;
            Debug.Log("Card moved to new area.");

            // 手札の表示を更新
            UpdateHand();
        }
    }
    public void MoveCardToAreaFromDeck(GameObject card, Transform area) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        // デッキから削除
        if (deck.Contains(cardSprite)) {
            deck.Remove(cardSprite);
            Destroy(card);  // デッキのUIから削除

            // カードを新しいエリアに移動
            GameObject newCard = Instantiate(cardPrefab, area);
            newCard.GetComponent<Image>().sprite = cardSprite;
            Debug.Log("Card moved to new area.");

            // デッキの表示を更新
            UpdateDeckView();
        }
    }

    // 手札リストを更新
    public void UpdateHandList(GameObject card) {
        Sprite cardSprite = card.GetComponent<Card>().cardImage.sprite;

        if (!hand.Contains(cardSprite)) {
            hand.Add(cardSprite);
            UpdateHand();
        }
    }

    // 手札の更新
    void UpdateHand() {
        foreach (Transform child in handArea) {
            Destroy(child.gameObject);
        }
        ArrangeHand();
    }

    // DeckViewを更新
    public void UpdateDeckView() {
        foreach (Transform child in deckView) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < deck.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, deckView);
            newCard.GetComponent<Image>().sprite = deck[i];
        }
    }
}
