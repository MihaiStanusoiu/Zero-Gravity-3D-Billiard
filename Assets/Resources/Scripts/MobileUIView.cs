using UnityEngine;
using UnityEngine.UI;

public class MobileUIView : MonoBehaviour
{
    public Text CueHitsText;
    private GameState gameState;

    // Start is called before the first frame update
    private void Start()
    {
        gameState = GameObject.Find("Game").GetComponent<GameState>();
        gameState.CueHitsUpdated += CueHitsUpdated;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void CueHitsUpdated(object sender, int e)
    {
        CueHitsText.text = $"Cue Hits: {e}";
    }
}