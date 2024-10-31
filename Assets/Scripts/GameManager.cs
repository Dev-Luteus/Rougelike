using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; } 
    /* Public Static Instance, Singleton Pattern, One instance per class,
     * but can be accessed everywhere. Bad for Multi-threading,
     * but for this simple game it gives much simpler code.
     * not private static, because it can't be accessed without serialize? */
    
    [SerializeField] private BoardManager     BoardManager;
    [SerializeField] private PlayerController PlayerController;
    
    public UIDocument UIDoc;
    private int m_FoodAmount = 100; 
    private Label m_FoodLabel; // Label (Unity UI (UnityDoc))
    
    /* If TurnManager of GameManager public,
     * other scripts will also be able to access it through GameManager.Instance .*/
    public TurnManager TurnManager { get; private set;}
    private void Awake() {
        if (Instance != null) {
           Destroy(gameObject);
           return;
        }
        Instance = this;
    }
    private void Start() {
        TurnManager = new TurnManager();
        InitializeGame();
        InitializeUI();
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;
    }
    private void InitializeGame() {
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));
    }
    private void InitializeUI() {
        // .rootVisualElement = GameUI or UXML file. Its the first element in the hierarchy, necessary.
        // Q method looks for: element of given type, f.e label
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        UpdateFoodDisplay();
    }
    private void UpdateFoodDisplay() {
        m_FoodLabel.text = $"Food: {m_FoodAmount}";
    }
    private void OnTurnHappen() {
        m_FoodAmount -= 1;
        m_FoodLabel.text = "Food : "+ m_FoodAmount;
        Debug.Log("Current amount of food : " + m_FoodAmount);
    }
}
