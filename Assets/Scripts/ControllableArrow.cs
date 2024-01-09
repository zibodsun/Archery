using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllableArrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;
    public float releaseThreshold = 0.2f;
    public float force;
    public GameObject camera;

    public Transform notch;
    public float interpolationSpeed = 0.03f;
    private float _timeCount = 0.0f;

    private ChildArrow _rightSplitArrow, _leftSplitArrow;     // child arrows for the multiply modifier
    public GameObject childArrowPrefab;
    public Rigidbody rb;
    public bool inAir = false;
    public Vector3 lastPosition = Vector3.zero;
    public LevelManager levelManager;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PullInteraction.PullActionReleased += Release;
        notch = transform.parent.GetComponent<Transform>();
        camera = transform.Find("Camera").gameObject;
        levelManager = FindAnyObjectByType<LevelManager>();
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
        inAir = true;
        camera.SetActive(true);
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);   // reset rotation so camera stays on top

        rb.isKinematic = false;
        force = v * speed;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);     // add a forward force to the rigidbody of the arrow
        lastPosition = tip.position;
    }
    // for physics calculations, using FixedUpdate is more reliable than the regular Update function
    public virtual void FixedUpdate()
    {
        if (inAir) {
            RemoteControl();                       // swerves the arrow during flight
            CheckCollision();
            lastPosition = tip.position;
        }
    }
    // Checks if the arrow has collided with anything
    private void CheckCollision()
    {
        if (Physics.Linecast(lastPosition, tip.position, out RaycastHit hit)) {
            if (hit.transform.tag != "IgnoreCollision" && hit.transform.tag != "Arrow") {   // ignore the modifiers and arrows
                if (hit.transform.TryGetComponent(out Rigidbody hitRb))
                {
                    transform.parent = hit.transform;
                    if (hitRb.gameObject.tag == "ReactiveTarget") {
                        hitRb.AddForce(rb.velocity, ForceMode.Impulse);
                        levelManager.UpdateScore();
                    }

                    MenuButton b = hitRb.GetComponent<MenuButton>();                        // for triggering menu buttons
                    if ( b != null) {
                        b.OnArrowHit();
                    }
                }
                Stop();
                StartCoroutine(Despawn());
                StartCoroutine(DestroyCamera());
                Debug.Log("Collided with something that I should collide with: " + hit.transform.gameObject.name);
            }
            
        }
    }

    private void Stop()
    {
        inAir = false;
        rb.isKinematic = true;
    }
    // despawn timer for arrows that have landed
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
    // detach camera and destroy after timer
    IEnumerator DestroyCamera() {
        if (camera == null) yield return null;  // for child arrows without camera

        camera.transform.parent = null;
        yield return new WaitForSeconds(3f);
        Destroy(camera);
    }

    // Controls the rotation of a projectile using the notch pointing direction
    private void RemoteControl() 
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, notch.transform.rotation, _timeCount * interpolationSpeed); // rotates the gameobject
        rb.velocity = transform.forward * rb.velocity.magnitude;  // rotates the rigidbody
        _timeCount = _timeCount + Time.deltaTime;
        interpolationSpeed -= 0.00015f;      // control worsens over time
    }
    // The multiply modifier that creates two additional arrows on the left and right side of this one
    public void Multiply()
    {
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);   // resets the rotation of the arrow on its body
                                                                                                                // so that it splits always horizontally
        interpolationSpeed = 0;

        // spawn the other two arrows
        _rightSplitArrow = Instantiate(childArrowPrefab, transform.position,  transform.rotation * Quaternion.AngleAxis(20, Vector3.up)).GetComponent<ChildArrow>();
        _leftSplitArrow = Instantiate(childArrowPrefab, transform.position, transform.rotation * Quaternion.AngleAxis(-20, Vector3.up)).GetComponent<ChildArrow>();

        // add force to them so they also go forward
        _rightSplitArrow.force = force;
        _leftSplitArrow.force = force;
    }
}
