using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollider : MonoBehaviour
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
        //Parado sobre piso
        if (groundTags.Contains(collision.gameObject.tag))
        {
            playerScrpt.SetGroundedState(true);
        }

        //Enemigos
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Dejar el piso
        if (groundTags.Contains(collision.gameObject.tag))
        {
            playerScrpt.SetGroundedState(false);
        }
    }
}
