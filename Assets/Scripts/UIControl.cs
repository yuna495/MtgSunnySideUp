using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIControl : MonoBehaviour {
    public GameObject toggleDeckViewButton;  // トグルボタン
    public GameObject deckViewPanel;         // デッキビューのパネル
    public GameObject[] manaTexts = new GameObject[6];    // Manaを表示するTextオブジェクト
    public GameObject[] manaUpButtons = new GameObject[6]; // Manaを増やすボタン
    public GameObject[] manaDownButtons = new GameObject[6]; // Manaを減らすボタン
    public GameObject turnText;  // ターン数を表示するTextオブジェクト
    public GameObject nextTurnButton;  // 次のターンに進むボタン
    public GameObject lifeText;  // 生命力を表示するTextオブジェクト
    public GameObject lifeDownButton;
    public GameObject retryButton;  // リトライボタン
    public DeckManager deckManager;
    private int life = 20;
    private int turn = 1;

    private int[] manaValues = new int[6]; // 各Manaの値を保持

    void Start() {
        // ゲーム開始時にdeckViewPanelを非表示にする
        deckViewPanel.SetActive(false);

        // ボタンにクリックイベントリスナーを追加
        toggleDeckViewButton.GetComponent<Button>().onClick.AddListener(ToggleDeckView);
        nextTurnButton.GetComponent<Button>().onClick.AddListener(NextTurn);
        lifeDownButton.GetComponent<Button>().onClick.AddListener(Life);
        retryButton.GetComponent<Button>().onClick.AddListener(Retry);

        // ターン数と生命力の初期値を設定
        turnText.GetComponent<Text>().text = turn.ToString();
        lifeText.GetComponent<Text>().text = life.ToString();

        // Manaの初期値を設定し、UIに反映
        for (int i = 0; i < manaTexts.Length; i++) {
            manaValues[i] = 0;  // 初期値は0
            UpdateManaText(i);   // 初期値をテキストに表示

            // 各Upボタンにリスナーを追加
            int index = i;  // ローカル変数で固定
            manaUpButtons[i].GetComponent<Button>().onClick.AddListener(() => IncreaseMana(index));

            // 各Downボタンにリスナーを追加
            manaDownButtons[i].GetComponent<Button>().onClick.AddListener(() => DecreaseMana(index));
        }
    }

    void Update() {
        // シフトキーが押されているか確認
        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // シフトキーが押されていない場合にIncreaseManaを呼び出す
        if (!isShiftHeld) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { IncreaseMana(0); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { IncreaseMana(1); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { IncreaseMana(2); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { IncreaseMana(3); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { IncreaseMana(4); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { IncreaseMana(5); }
        }
        // シフトキーが押されている場合にDecreaseManaを呼び出す
        else {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { DecreaseMana(0); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { DecreaseMana(1); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { DecreaseMana(2); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { DecreaseMana(3); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { DecreaseMana(4); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { DecreaseMana(5); }
        }
    }

    // DeckViewパネルの表示/非表示をトグル
    void ToggleDeckView() {
        bool isActive = deckViewPanel.activeSelf;
        deckViewPanel.SetActive(!isActive);
    }

    // Manaを増やすメソッド
    void IncreaseMana(int index) {
        manaValues[index]++;
        UpdateManaText(index);
    }

    // Manaを減らすメソッド
    void DecreaseMana(int index) {
        if (manaValues[index] > 0) {
            manaValues[index]--;
            UpdateManaText(index);
        }
    }

    // Manaのテキストを更新するメソッド
    void UpdateManaText(int index) {
        manaTexts[index].GetComponent<Text>().text = manaValues[index].ToString();
    }

    void NextTurn() {
        turn++;
        turnText.GetComponent<Text>().text = turn.ToString();
        deckManager.AddCardFromDeckToHand();
        // 全てのカードを検索し、Z=90のカードをZ=0に戻す
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards) {
            // Z軸の回転角度が90度であるかをチェック
            float zRotation = card.transform.eulerAngles.z;
            if (Mathf.Approximately(zRotation, 90f)) {
                card.transform.rotation = Quaternion.Euler(0, 0, 0);  // Z軸を0度にリセット
            }
        }
    }

    void Life() {
        life--;
        lifeText.GetComponent<Text>().text = life.ToString();
    }
    void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
