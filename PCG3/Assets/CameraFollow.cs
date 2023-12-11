using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject followObjective;

    float xPosition;

    // Start is called before the first frame update
    void Start()
    {
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(followObjective.transform.position.x > xPosition)
        {
            transform.position = new Vector3(followObjective.transform.position.x, transform.position.y, transform.position.z);
            xPosition = transform.position.x;
        }

        if (followObjective.transform.position.y > 5.5)
        {
            transform.position = new Vector3(transform.position.x, followObjective.transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, 5.5f, transform.position.z);
        }
    }

    public void ResetCamera()
    {
        transform.position = new Vector3(8.5f, 5.5f, -5);
        xPosition = 8.5f;
    }
}
