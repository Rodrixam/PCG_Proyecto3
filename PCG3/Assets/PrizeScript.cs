using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeScript : MonoBehaviour
{
    /*CapsuleCollider2D bCollider;

    bool spawning = false;
    int counter = 0;

    [SerializeField] bool isMushroom = false;//*/

    [SerializeField] int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        //bCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (spawning)
        {
            transform.position += Vector3.up * 0.2f;
            counter++;
            if(counter >= 4)
            {
                spawning = false;
                bCollider.enabled = true;
            }
        }//*/
    }

    public void SpawnPrize()
    {
        transform.position += Vector3.up * 0.2f;
        /*bCollider.enabled = false;
        spawning = true;
        counter = 0;

        if (isMushroom)
        {
            GetComponent<GoombaMovement>().SetVelocity(4);
            gameObject.transform.localScale = new Vector2(1, 1);
        }//*/
    }

    public int GetPoints()
    {
        return points;
    }
}
