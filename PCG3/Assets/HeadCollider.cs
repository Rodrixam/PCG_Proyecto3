using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Question" && player.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            collision.gameObject.GetComponent<QuestionScript>().Hit();
        }
    }
}
