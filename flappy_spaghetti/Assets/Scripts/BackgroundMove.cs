using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float deadZone = -60;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < deadZone)
        {
            transform.position += (Vector3.right * 159.9f);
        }
        transform.position += (Vector3.left * moveSpeed) * Time.deltaTime;
    }
}
