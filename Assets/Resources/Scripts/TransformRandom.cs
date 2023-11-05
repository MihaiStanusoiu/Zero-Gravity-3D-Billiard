using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRandom : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var randomPos = new Vector3(Random.value, Random.value, Random.value);
        transform.localPosition = randomPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
