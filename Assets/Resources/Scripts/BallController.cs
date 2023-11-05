using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public EventHandler BallScored;
    private Quaternion initialOrientation;
    private Vector3 initialPosition;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.transform.GetPositionAndRotation(out initialPosition, out initialOrientation);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ResetState()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactables");
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        gameObject.transform.SetPositionAndRotation(initialPosition, initialOrientation);
        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
    }

    public void PocketTriggered(GameObject pocket)
    {
        Debug.Log("Ball collided with pocket");
        gameObject.layer = LayerMask.NameToLayer("Scored Ball");
        BallScored?.Invoke(this, EventArgs.Empty);
    }

    public void PocketTriggeredExit(GameObject pocket)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        // rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = true;
        // rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
    }
}