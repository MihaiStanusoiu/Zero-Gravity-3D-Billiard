using UnityEngine;

public class PocketController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Interactables"))
        {
            // rb.isKinematic = true;
            Debug.Log("Ball collided with pocket");
            collider.gameObject.GetComponent<BallController>().PocketTriggered(gameObject);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Scored Ball"))
            collider.gameObject.GetComponent<BallController>().PocketTriggeredExit(gameObject);
    }
}