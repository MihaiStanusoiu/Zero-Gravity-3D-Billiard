using UnityEngine;
using UnityEngine.UI;

public class SettingsView : MonoBehaviour
{
    private GameState gameState;
    private Button resetButton;
    public Text ScoreText;
    private Text timer;

    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameObject.Find("Game").GetComponent<GameState>();
        gameState.BallsScoredUpdated += BallsScoredUpdated;

        resetButton = GameObject.FindGameObjectWithTag("ResetButton").GetComponent<Button>();
        timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
    }

    private void BallsScoredUpdated(object sender, int e)
    {
        ScoreText.text = $"{e}";
    }

    // Update is called once per frame
    private void Update()
    {
        timer.text = $"{gameState.SecondsElapsed} seconds";
    }

    public void ToggleReset()
    {
        resetButton.interactable = !resetButton.interactable;
    }
}