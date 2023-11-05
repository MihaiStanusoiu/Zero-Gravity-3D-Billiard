using UnityEngine;

public class CueConfigurator : MonoBehaviour
{
    public Vector3 CenterOfMassOffset;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        rb.centerOfMass = CenterOfMassOffset;
    }
}