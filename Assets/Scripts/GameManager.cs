using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour {
    #region Public Static Instance > Info
    /* Public Static Instance, Singleton Pattern, One instance per class,
     * but can be accessed everywhere. Bad for Multi-threading,
     * but for this simple game it gives much simpler code.
     * not private static, because it can't be accessed without serialize? */
    #endregion
    public static GameManager Instance { get; private set; } 
    public UIDocument UIDoc;
    
    private int       m_FoodAmount = 100; 
    private Label     m_FoodLabel; // Label (Unity UI (UnityDoc))
    
    [SerializeField] private BoardManager     boardManager;
    [SerializeField] private PlayerController playerController;
    
    #region TurnManager Public 
    /* If TurnManager of GameManager public,
     * other scripts will also be able to access it through GameManager.Instance .*/
    #endregion
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
        TurnManager.OnTick += OnTurnHappen;  // Subscribe OnTurnHappen to OnTick : In order to Update label
        InitializeGame();
        InitializeUI();
        UpdateFoodDisplay(); // Must be Initialised once ( this is bad code, should fix )
    }
    private void InitializeGame() {
        boardManager.Init();
        playerController.Spawn(boardManager, new Vector2Int(1, 1));
    }
    private void InitializeUI() {
        #region .rootVisualElement - Q Method > Info
        /* .rootVisualElement = GameUI or UXML file.
         * Its the first element in the hierarchy, necessary.
         * Q method looks for: element of given type, f.e label */
        #endregion
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
    }
    private void OnTurnHappen() {
        m_FoodAmount -= 1;
        UpdateFoodDisplay();
        Debug.Log("Current amount of food : " + m_FoodAmount);
    }
    #region UpdateFoodDisplay > LAMBDA ALTERNATIVE
    /* Lambda Expression Alternative
    private System.Action<int> UpdateFoodDisplay => (food) => m_FoodLabel.text = $"Food: {food}";
    */
    #endregion
    private void UpdateFoodDisplay() {
        m_FoodLabel.text = $"Food: {m_FoodAmount}";
    }
}
