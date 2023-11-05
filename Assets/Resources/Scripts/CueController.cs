using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Resources.Scripts
{
    public class CueController : MonoBehaviour
    {
        private readonly List<Rigidbody> cueRbs = new();
        public Vector3 CenterOfMassOffset = Vector3.back * 0.5f;
        private GameObject cue;

        private int cueHits;
        public EventHandler<int> CueHitsUpdate;

        private GameObject currentCollidingBall;
        private GameState gameState;

        public float HitAccelerationScaleFactor = 50f;
        private XRBaseInteractable interactable;
        private Rigidbody rb;

        // Start is called before the first frame update
        private void Start()
        {
            gameState = GameObject.Find("Game").GetComponent<GameState>();

            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = CenterOfMassOffset;

            cue = GameObject.FindGameObjectsWithTag("Cue")[0];
            cueRbs.Add(cue.GetComponent<Rigidbody>());
            cueRbs.AddRange(cue.GetComponentsInChildren<Rigidbody>());

            interactable = GetComponent<XRBaseInteractable>();
            interactable.hoverEntered.AddListener(CueIsGrabbable);
            interactable.hoverExited.AddListener(CueIsNotGrabbable);
            interactable.selectEntered.AddListener(CueIsGrabbed);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void CueIsGrabbable(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRBaseControllerInteractor controller) PulseController(controller);
        }

        private void PulseController(XRBaseControllerInteractor controller)
        {
            controller.SendHapticImpulse(0.1f, 0.2f);
            // yield return new WaitForSecondsRealtime(0.2f);
            controller.SendHapticImpulse(0.4f, 0.2f);
            // yield return new WaitForSecondsRealtime(0.2f);
            controller.SendHapticImpulse(0.1f, 0.2f);
            // yield return new WaitForSecondsRealtime(0.2f);
        }

        private void CueIsNotGrabbable(HoverExitEventArgs args)
        {
            if (args.interactorObject is XRBaseControllerInteractor controller) controller.SendHapticImpulse(0f, 0);
        }

        private void CueIsGrabbed(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRBaseControllerInteractor controller)
            {
                controller.SendHapticImpulse(0f, 0);
                controller.SendHapticImpulse(1, 0.1f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Interactables")) return;

            if (!collision.gameObject.Equals(currentCollidingBall))
            {
                currentCollidingBall = collision.gameObject;
                gameState.CueHits++;
                CueHitsUpdate?.Invoke(this, cueHits);
            }

            if (collision.relativeVelocity.magnitude < float.Epsilon) return;

            var otherRb = collision.collider.attachedRigidbody;

            // Apply force to ball based on velocity, and dampen over time based on drag
            var collisionDir = collision.GetContact(0).point - transform.position;
            var inlineSpeed = Vector3.Dot(collisionDir.normalized, rb.velocity);
            var force = collisionDir * inlineSpeed * HitAccelerationScaleFactor;
            otherRb.AddForceAtPosition(force / (1 - Time.deltaTime * otherRb.drag),
                collision.GetContact(0).point);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer != LayerMask.NameToLayer("Interactables")) return;

            currentCollidingBall = null;
        }
    }
}