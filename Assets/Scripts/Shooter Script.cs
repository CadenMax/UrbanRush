using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShooterScript : MonoBehaviour
{

    public float min=2f;
    public float max=3f;
    // Start is called before the first frame update
    void Start()
    {
        min = transform.position.z;
        max = transform.position.z + 20;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * 2, max - min) + min);
    }
}
