using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaFrontCollider : MonoBehaviour
{
    [SerializeField] GameObject goomba;

    List<string> groundTags = new List<string>() { "Ground", "Question" };

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
        //Obstaculo al frente
        if (groundTags.Contains(collision.gameObject.tag))
        {
            goomba.transform.localScale = new Vector2(goomba.transform.localScale.x * -1, 1);
        }
    }

}
