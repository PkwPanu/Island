using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    float x, z;
    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        movePlayer(new Vector3(x, 0, z));
    }

    void movePlayer(Vector3 direction)
    {
        transform.position = (transform.position + (direction * 10)); ;
    }
}
