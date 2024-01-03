using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullInteraction : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

    public Transform start, end;
    public GameObject notch;

    public float pullAmount { get; private set; } = 0.0f;

    private LineRenderer _lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake();
        _lineRenderer = GetComponent<LineRenderer>();
    }
    // When entering the grab interactor, set interactor as the hand that is interacting
    public void SetPullInteractor(SelectEnterEventArgs args) {
        pullingInteractor = args.interactorObject;
    }
    // Runs when string is released
    public void Release() {
        PullActionReleased?.Invoke(pullAmount);
        pullingInteractor = null;
        pullAmount = 0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0f);  // notch gets pulled back with arrow
        UpdateString();
    }
    // Called when Interaction with this Interactable occurs
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
        base.ProcessInteractable(updatePhase);

        // during dynamic phase update based on controller movement
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic) {
            if (isSelected) {
                Vector3 pullPosition = pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosition);

                UpdateString();
            }
        }
    }
    // Calculates the magnitude between the start of the pull and the end and turns it into a 0-1 value
    private float CalculatePull(Vector3 pullPosition) {
        Vector3 pullDirection = pullPosition - start.position;
        Vector3 targetDirection = end.position - start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();    // turns the length of the Vector3 to 1, keeps the same direction
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;  // value between 0-1 that tells if the values point in the same direction
        return Mathf.Clamp(pullValue, 0, 1);    // returns a value within constraints of a min and max
    }
    // sets the visuals of the line renderer
    private void UpdateString() {
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(start.transform.localPosition.z, end.transform.localPosition.z, pullAmount);    // take the z value of the pulling point
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z + .2f);
        _lineRenderer.SetPosition(1, linePosition);
    }
}
