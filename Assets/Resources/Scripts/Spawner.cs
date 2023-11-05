using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class Spawner : MonoBehaviour
{
    public InputActionReference SpawnThrowable;

    private GameObject table;

    // public InputActionAsset InputActionAsset;
    public GameObject Throwable;


    // Start is called before the first frame update
    private void Start()
    {
        table = GameObject.Find("/Table");
        SpawnThrowable.action.started += SpawnThrowableOnperformed;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void SpawnThrowableOnperformed(InputAction.CallbackContext obj)
    {
        // Spawn ball on table
        var position = new Vector3(0, 0.6f, 0);
        var throwable = Instantiate(Throwable, position, Quaternion.identity);

        throwable.transform.position = table.transform.position + position;

        var interactionManager = FindObjectOfType<XRInteractionManager>();

        if (interactionManager != null)
            // Add the interactive object to the XR Interaction Manager's interactables list
            interactionManager.RegisterInteractable(throwable.GetComponent<IXRInteractable>());
        else
            Debug.LogError("No XR Interaction Manager found in the scene.");
    }
}