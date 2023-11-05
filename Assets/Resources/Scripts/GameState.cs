using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Resources.Scripts;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private List<BallController> balls;
    [ReadOnly] public int BallsScored;
    public EventHandler<int> BallsScoredUpdated;

    [ReadOnly] public int CueHits;

    public EventHandler<int> CueHitsUpdated;
    public float GameScale = 1f;

    [ReadOnly] public float SecondsElapsed;

    // Start is called before the first frame update
    private void Start()
    {
        // Init position at HMD level
        // transform.SetPositionAndRotation(
        //     GameObject.FindGameObjectWithTag("MainCamera").transform.position + new Vector3(0, 0, 0.5f),
        //     Quaternion.identity);

        var balls = GameObject.FindGameObjectsWithTag("Ball");
        this.balls = balls.Select(ball => ball.GetComponent<BallController>()).ToList();
        this.balls.ForEach(ball => ball.BallScored += BallScored);
        var cueController = GameObject.FindGameObjectWithTag("Cue").GetComponent<CueController>();
        cueController.CueHitsUpdate += CueHitsUpdate;

        SecondsElapsed = Time.realtimeSinceStartup;
    }

    private void CueHitsUpdate(object sender, int e)
    {
        CueHits++;
        CueHitsUpdated?.Invoke(this, CueHits);
    }

    private void BallScored(object sender, EventArgs e)
    {
        BallsScored++;
        BallsScoredUpdated?.Invoke(this, BallsScored);
    }

    // Update is called once per frame
    private void Update()
    {
        SecondsElapsed += Time.deltaTime;
    }

    public void ScaleGame(float scale)
    {
        if (!(0f <= scale && scale <= 1f))
            return;

        GameScale = scale;
        transform.localScale = Vector3.one * scale;
    }

    public void ResetGameState()
    {
        balls.ForEach(ball =>
            ball.ResetState());
        BallsScored = 0;
        BallsScoredUpdated?.Invoke(this, BallsScored);
        CueHits = 0;
        CueHitsUpdated?.Invoke(this, CueHits);

        SecondsElapsed = 0f;
    }
}