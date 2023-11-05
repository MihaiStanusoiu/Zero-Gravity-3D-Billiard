using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CueHapticsController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void CueIsGrabbable(HoverEnterEventArgs args)
    {
        var controller = args.interactorObject.transform.gameObject.GetComponent<XRController>();
        controller.SendHapticImpulse(0.1f, float.MaxValue);
    }

    public void CueIsNotGrabbable(HoverExitEventArgs args)
    {
        var controller = args.interactorObject.transform.gameObject.GetComponent<XRController>();
        controller.SendHapticImpulse(0f, 0);
    }

    public void CueIsGrabbed(ActivateEventArgs args)
    {
        var controller = args.interactorObject.transform.gameObject.GetComponent<XRController>();
        controller.SendHapticImpulse(0f, 0);
        controller.SendHapticImpulse(1, 0.1f);
    }
}