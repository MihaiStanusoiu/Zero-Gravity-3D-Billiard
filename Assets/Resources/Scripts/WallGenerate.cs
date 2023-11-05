using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class WallGenerate : MonoBehaviour
{
    public uint BlockCount;
    private List<GameObject> bricks;
    public uint Height;

    private Object[] materials;
    public float scaleFactor;

    // Start is called before the first frame update
    private void Start()
    {
        materials = Resources.LoadAll("Materials", typeof(Material));
        bricks = new List<GameObject>();
        GenerateWall();
    }

    // Update is called once per frame
    private void Update()
    {
    }


    private void GenerateWall()
    {
        var cols = (int) Math.Ceiling((double) BlockCount / Height);
        var counter = BlockCount;

        for (var i = 0; i < cols && counter > 0; ++i)
        for (var j = 0; j < Height && counter > 0; ++j)
        {
            // Spawn brick and link through joints to left and below
            var brick = SpawnBrick(new Vector3(i * scaleFactor,
                j * scaleFactor + scaleFactor / 2));
            bricks.Add(brick);

            if (j > 0) LinkObjectsByFixedJoint(brick, bricks[i * (int) Height + j - 1]);

            if (i > 0) LinkObjectsByFixedJoint(brick, bricks[(i - 1) * (int) Height + j]);

            counter--;
        }
    }

    private void LinkObjectsByFixedJoint(GameObject obj1, GameObject obj2)
    {
        var joint = obj1.AddComponent<FixedJoint>();
        joint.breakForce = 1f;
        joint.connectedBody = obj2.GetComponent<Rigidbody>();
    }

    private GameObject SpawnBrick(Vector3 position)
    {
        var brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var rewindableLayer = LayerMask.NameToLayer("Rewindable");
        brick.layer = rewindableLayer;

        brick.GetComponent<Renderer>().material =
            Resources.Load("Materials/Brick", typeof(Material)) as Material;
        brick.transform.parent = gameObject.transform;
        brick.transform.localScale = Vector3.one * scaleFactor;
        brick.transform.localPosition = position;
        var rigidBody = brick.AddComponent<Rigidbody>();
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;

        // Add rewind script to brick
        brick.AddComponent<Rewind>();

        return brick;
    }
}