using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    public Arrow arrow;
    public bool shooting;
    public float speed;

    private Vector3 startPos;

    float timeCount = 0.0f;
    public float interpolationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startPos = arrow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        arrow.transform.rotation = transform.rotation;

        if (shooting) {
            arrow.Shoot(speed);
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            arrow.transform.position = startPos;
        }

        // player-controlled trajectory
        arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, transform.rotation, timeCount * interpolationSpeed);
        timeCount = timeCount + Time.deltaTime;

    }
}
