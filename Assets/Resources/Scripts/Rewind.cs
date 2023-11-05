using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rewind : MonoBehaviour
{
    private InputActionAsset ActionMap;

    private int frameCounter;

    private Stack<Tuple<Vector3, Quaternion>> history;

    private InputAction RewindButton;
    private bool rewinding;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        ActionMap = Resources.Load<InputActionAsset>("XRI Input Actions");
        RewindButton = ActionMap.FindAction("XRI LeftHand Interaction/Activate Value");

        rigidbody = GetComponent<Rigidbody>();
        history = new Stack<Tuple<Vector3, Quaternion>>();

        // Rewinding by holding does not work
        // RewindButton.performed += StartRewind;
        // RewindButton.canceled += RewindEnded;
        RewindButton.started += RewindButtonOnstarted;
    }

    private void RewindButtonOnstarted(InputAction.CallbackContext obj)
    {
        rigidbody.isKinematic = true;
        rewinding = true;

        try
        {
            var prevTransform = history.Pop();
            gameObject.transform.SetPositionAndRotation(prevTransform.Item1, prevTransform.Item2);
        }
        catch (InvalidOperationException)
        {
        }

        rewinding = false;
        rigidbody.isKinematic = false;
    }

    private void RewindEnded(InputAction.CallbackContext obj)
    {
        // DOES NOT WORK
        rigidbody.isKinematic = false;
        rewinding = false;
    }

    private void StartRewind(InputAction.CallbackContext obj)
    {
        // DOES NOT WORK
        rewinding = true;
        rigidbody.isKinematic = true;
        try
        {
            var prevTransform = history.Pop();
            gameObject.transform.SetPositionAndRotation(prevTransform.Item1, prevTransform.Item2);
        }
        catch (InvalidOperationException)
        {
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (rewinding) return;

        frameCounter++;
        // Sample and track pos and rotation every 60 frames
        if (frameCounter % 60 == 0)
        {
            gameObject.transform.GetPositionAndRotation(out var position, out var rotation);
            history.Push(new Tuple<Vector3, Quaternion>(position, rotation));
        }

        if (frameCounter == 100000)
            frameCounter = 0;
    }
}