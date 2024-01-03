using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : MonoBehaviour
{
    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f) {
            timer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer > 0f) { return; }

        if (other.gameObject.tag == "Arrow") {
            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            arrow.Multiply();
            timer = 5f;         // cooldown after applying the modifier
        }
    }
}
