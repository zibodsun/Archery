using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildArrow : ControllableArrow
{
    private bool _hasForce = false;     // check if force has been applied from the original arrow
    public override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        levelManager = FindAnyObjectByType<LevelManager>();
        notch = GameObject.Find("Notch").GetComponent<Transform>();     // get the notch from the bow
        lastPosition = transform.position;         // the object is now in global scale, so the last position cannot be initialised at Vector3.zero
        inAir = true;                              // child arrow is already in air
        interpolationSpeed = 0;

        rb.isKinematic = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // once force is added from the main arrow, add it to this child arrow
        if (!_hasForce) {
            rb.AddForce(transform.forward * force, ForceMode.Impulse);     // add a forward force to the rigidbody of the arrow
            _hasForce = true;
        }
    }
}
