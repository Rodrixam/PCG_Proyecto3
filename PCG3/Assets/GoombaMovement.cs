using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : MonoBehaviour
{
    Rigidbody2D enemyBody;

    [SerializeField] float velocity;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        enemyBody.velocity = new Vector2(velocity * transform.localScale.x * -1, enemyBody.velocity.y);
    }

    public void SetVelocity(float newVel)
    {
        velocity = newVel;
    }

}
