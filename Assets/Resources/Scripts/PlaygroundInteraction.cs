using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaygroundInteraction : MonoBehaviour
{
    private readonly List<Transform> initialBallsTransform = new();
    public string BallTag;
    public string WallTag;

    private GameObject whiteBall;
    public GameObject WhiteBallPrefab;

    // Start is called before the first frame update
    private void Start()
    {
        Bounds pyramidBound;
        Bounds cubeBound;

        // Compute bounding box for pyramid and playground
        var balls = GameObject.FindGameObjectsWithTag(BallTag);
        pyramidBound = balls[0].GetComponent<Renderer>().bounds;
        foreach (var ball in balls)
        {
            initialBallsTransform.Add(ball.transform);
            var mesh = ball.GetComponent<Renderer>();
            pyramidBound.Encapsulate(mesh.bounds);
        }

        var walls = GameObject.FindGameObjectsWithTag(WallTag);
        cubeBound = walls[0].GetComponent<Renderer>().bounds;
        var wallWidth = 0f;
        foreach (var wall in walls)
        {
            var mesh = wall.GetComponent<Renderer>();
            wallWidth = wall.transform.lossyScale.z;
            cubeBound.Encapsulate(mesh.bounds);
        }

        // Spawn white ball and set random position outside pyramid bounds
        whiteBall = Instantiate(WhiteBallPrefab, Vector3.zero, Quaternion.identity);
        whiteBall.transform.SetParent(transform);

        var x = RandomInRangeExcludingRange(cubeBound.min.x + wallWidth, cubeBound.max.x - wallWidth,
            pyramidBound.min.x, pyramidBound.max.x);
        var y = RandomInRangeExcludingRange(cubeBound.min.y + wallWidth, cubeBound.max.y - wallWidth,
            pyramidBound.min.y, pyramidBound.max.y);
        //var z = RandomInRangeExcludingRange(cubeBound.min.z + wallWidth, cubeBound.max.z - wallWidth, pyramidBound.min.z, pyramidBound.max.z);
        var z = Random.Range(cubeBound.min.z + wallWidth, pyramidBound.min.z);


        whiteBall.transform.SetPositionAndRotation(new Vector3(x, y, z), Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private static float RandomInRangeExcludingRange(float lower, float upper, float lowerSub, float upperSub)
    {
        if (!(lower <= upper && lowerSub <= upperSub && lowerSub >= lower && upperSub <= upper))
            throw new Exception(string.Format("Invalid arguments: [{0}, {1}], [{2}, {3}]", lower, upper, lowerSub,
                upperSub));

        var subRangeSize = upperSub - lowerSub;
        var x = Random.Range(lower, upper - subRangeSize);
        if (x >= lowerSub) x += subRangeSize;

        return x;
    }
}