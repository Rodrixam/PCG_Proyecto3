using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Animator anim;
    Rigidbody2D body;

    [SerializeField] float maxHspeed;
    [SerializeField] float acceleration;

    [SerializeField] float jumpForce;

    bool grounded = true;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = false;
        //Izquierda
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.localScale = new Vector3(-1,1);
            isMoving = true;
        }
        //Derecha
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.localScale = new Vector3(1, 1);
            isMoving = true;
        }

        #region Movimiento horizontal
        if (isMoving)
        {
            anim.SetBool("Walking", true);

            //Acelerar
            body.velocity += new Vector2(body.velocity.x + acceleration * transform.localScale.x, body.velocity.y);
            //Limite a la velocidad
            if (body.velocity.x < maxHspeed * -1 || body.velocity.x > maxHspeed)
            {
                body.velocity = new Vector2(maxHspeed * transform.localScale.x, body.velocity.y);
            }
        }
        else if (body.velocity.x == 0)
        {
            anim.SetBool("Walking", false);
        }
        #endregion

        #region Saltar
        if (Input.GetKeyDown(KeyCode.Z) && grounded)
        {
            grounded = false;
            anim.SetBool("Jump", true);

            body.AddForce(Vector2.up * jumpForce);
        }
        #endregion
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && collision.gameObject.transform.position.y < transform.position.y && body.velocity.y <= 0)
        {
            anim.SetBool("Jump", false);
            grounded = true;
        }
    }
}
