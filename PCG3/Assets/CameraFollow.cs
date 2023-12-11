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
    }
}
