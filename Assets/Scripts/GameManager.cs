using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour, ICubeObserver, IGameController
{
    [SerializeField] GameObject spawner;
    [SerializeField] TMP_Text scoreText; // Reference to the UI text component for displaying the score
    [SerializeField] GameObject losePanel;
    [SerializeField] TMP_Text gameOverText; // Reference to the UI text component for displaying the game over message
    private float score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; private set; }

    // The protection proxy every game-control request must go through.
    public IGameController Controller { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Controller = new GameControllerProxy(this); // proxy wraps the real controller (this)

        OnGameBegin(); // Subscribe to cube events
        GameState("Start");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString(); // Update the score text in the UI
    }
   
    void IGameController.AddScore(float points)
    {
        score += points; // Increment the score by the specified points
    }

    void IGameController.EndGame()
    {
        GameState("End");
    }

    private void OnDestroy()
    {
        OnGameEnd();
    }

    private void OnGameBegin()
    {
        spawner.GetComponent<CubePlacement>().Subscribe(this); // GameManager IS the observer
    }
    private void OnGameEnd()
    {
        spawner.GetComponent<CubePlacement>().Unsubscribe(this); // unsubscribe the same instance
    }

    public void OnCubeFellOff(Cube cube)
    {
        Controller.EndGame(); // route through the proxy (dedupes + logs) instead of calling GameState directly
    }

    public void GameState(string state)
    {
        switch (state)
        {
            case "Start":
                spawner.SetActive(true); // Activate the spawner to start spawning cubes
                break;
            case "End":
                spawner.SetActive(false);
                losePanel.SetActive(true); // Show the lose panel when the game ends
                gameOverText.text = "Game Over! Final Score: " + score.ToString(); // Display the game over message with the final score
                break;
            default:
                Debug.LogWarning("Unknown game state: " + state);
                break;
        }
    }

}
