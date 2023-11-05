using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerHaptics : MonoBehaviour
{
    private XRController controller;
    private XRBaseInteractor interactor;

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<XRController>();
        interactor = GetComponent<XRBaseInteractor>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ItemHovered(HoverEnterEventArgs args)
    {
        if (!args.interactorObject.Equals(interactor) || args.interactableObject.transform.gameObject.layer !=
            LayerMask.NameToLayer("Player Items"))
            return;

        controller.SendHapticImpulse(0.1f, float.MaxValue);
    }

    public void CueIsNotGrabbable(HoverExitEventArgs args)
    {
        if (!args.interactorObject.Equals(interactor) || args.interactableObject.transform.gameObject.layer !=
            LayerMask.NameToLayer("Player Items"))
            return;

        controller.SendHapticImpulse(0f, 0);
    }

    public void CueIsGrabbed(ActivateEventArgs args)
    {
        if (!args.interactorObject.Equals(interactor) || args.interactableObject.transform.gameObject.layer !=
            LayerMask.NameToLayer("Player Items"))
            return;

        controller.SendHapticImpulse(0f, 0);
        controller.SendHapticImpulse(1, 0.1f);
    }
}