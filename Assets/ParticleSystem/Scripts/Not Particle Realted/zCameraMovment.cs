using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zCameraMovment : MonoBehaviour
{
    [SerializeField] float movementSpeed;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 position = transform.position;
        position += transform.right * x * movementSpeed * Time.deltaTime;
        position += transform.forward * y * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            position.y += movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            position.y -= movementSpeed * Time.deltaTime;
        }

        transform.position = position;
    }
}
