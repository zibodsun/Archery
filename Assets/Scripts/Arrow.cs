using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Arrow rightSplitArrow, leftSplitArrow;
    public bool shooting = false;
    public float speed;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Debug.Log("Created Parent Arrow");
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting) {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
    public void Shoot(float s) {
        shooting = true;
        speed = s;
    }
    
    public virtual void Multiply() {
        rightSplitArrow = Instantiate(this, transform.position, Quaternion.AngleAxis(20,Vector3.up));
        leftSplitArrow = Instantiate(this, transform.position, Quaternion.AngleAxis(-20,Vector3.up));

        rightSplitArrow.Shoot(speed);
        leftSplitArrow.Shoot(speed);
    } 
}
