using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour, ICubeObserver
{
    [SerializeField] GameObject spawner;
    [SerializeField] TMP_Text scoreText; // Reference to the UI text component for displaying the score
    [SerializeField] GameObject losePanel;
    [SerializeField] TMP_Text gameOverText; // Reference to the UI text component for displaying the game over message
    private float score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnGameBegin(); // Subscribe to game events
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        GameState("Start");
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score.ToString(); // Update the score text in the UI
    }
    public void AddScore(float points)
    {
        score += points; // Increment the score by the specified points
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
        GameState("End"); // End the game when a cube falls off
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
