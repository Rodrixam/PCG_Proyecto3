using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCollider : MonoBehaviour
{
    [SerializeField] GameObject player;
    Movement playerScrpt;

    List<string> groundTags = new List<string>() { "Ground", "Question" };

    // Start is called before the first frame update
    void Start()
    {
        playerScrpt = player.GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Obstaculo al frente
        if (groundTags.Contains(collision.gameObject.tag))
        {
            playerScrpt.SetObstacleState(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Salir del obstaculo
        if (groundTags.Contains(collision.gameObject.tag))
        {
            playerScrpt.SetObstacleState(false);
        }
    }
}
