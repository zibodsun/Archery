using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableArrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;
    public float releaseThreshold = 0.2f;

    private Rigidbody _rb;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;

        Stop();
    }

    private void OnDestroy()
    {
        PullInteraction.PullActionReleased -= Release;
    }
    // called when the release action is activated. Takes a parameter v that affects the speed of the arrow
    private void Release(float v) {
        if (v < releaseThreshold) return;                    // dead zone of the pulling amount to trigger the release action

        PullInteraction.PullActionReleased -= Release;       // after releasing the method is unbound from the action
        transform.parent = null;                             // unbind from parent
        _inAir = true;

        //_rb.useGravity = true;
        _rb.isKinematic = false;

        _rb.AddForce(transform.forward * v * speed, ForceMode.Impulse);     // add a forward force to the rigidbody of the arrow
        _lastPosition = tip.position;
    }
    // for physics calculations, using FixedUpdate is more reliable than the regular Update function
    private void FixedUpdate()
    {
        if (_inAir) {
            CheckCollision();
            _lastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, tip.position, out RaycastHit hit)) {
            if (hit.transform.TryGetComponent(out Rigidbody hitRb)) {
                transform.parent = hit.transform;
            }
            Stop();
        }
    }

    private void Stop()
    {
        _inAir = false;
        //_rb.useGravity = false;
        _rb.isKinematic = true;
    }


}
