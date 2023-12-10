using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Movement : MonoBehaviour
{
    Animator anim;
    Rigidbody2D body;
    CapsuleCollider2D bCollider;

    public LevelGenerator levelGenerator;

    [SerializeField] float maxHspeed;
    [SerializeField] float maxRunSpeed;
    [SerializeField] float acceleration;

    [SerializeField] float jumpSpeed;

    bool grounded = false;
    bool obstacle = false;

    KeyCode left = KeyCode.LeftArrow;
    KeyCode right = KeyCode.RightArrow;
    KeyCode jump = KeyCode.Z;
    KeyCode run = KeyCode.X;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        bCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Inputs
        bool isMoving = false;
        //Izquierda
        if (Input.GetKey(left))
        {
            transform.localScale = new Vector3(-1,1);
            isMoving = true;
        }
        //Derecha
        else if (Input.GetKey(right))
        {
            transform.localScale = new Vector3(1, 1);
            isMoving = true;
        }

        //Correr
        float speedLimit = maxHspeed;
        float accSpeed = acceleration / 2;
        if (grounded)
        {
            if (Input.GetKey(run))
            {
                speedLimit = maxRunSpeed;
            }
            accSpeed = acceleration;
        }
        #endregion

        #region Movimiento horizontal
        if (isMoving && !obstacle)
        {
            anim.SetBool("Walking", true);
            
            //Limite a la velocidad
            if ((transform.localScale.x < 0 && body.velocity.x - accSpeed > -1 * speedLimit) || (transform.localScale.x > 0 && body.velocity.x + accSpeed < speedLimit))
            {
                //Acelerar
                body.velocity += new Vector2(accSpeed * transform.localScale.x, 0);
            }
        }
        else if (body.velocity.x == 0)
        {
            anim.SetBool("Walking", false);
        }
        #endregion

        #region Saltar
        if (Input.GetKeyDown(jump) && grounded)
        {
            grounded = false;
            anim.SetBool("Jump", true);

            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
        #endregion

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Golpea bloque question por debajo
        if (collision.gameObject.tag == "Question")
        {
            BoxCollider2D otherCol = collision.gameObject.GetComponent<BoxCollider2D>();

            bool underBlock = transform.position.y + bCollider.size.y / 2 - 0.1f < collision.gameObject.transform.position.y - otherCol.size.y / 2;
            bool leftBlockBorder = transform.position.x + bCollider.size.x / 2 > collision.gameObject.transform.position.x - otherCol.size.x / 2;
            bool rightBlockBorder = transform.position.x - bCollider.size.x / 2 < collision.gameObject.transform.position.x + otherCol.size.x / 2;

            //Debug.Log((transform.position.y + bCollider.size.y / 2 - 0.1f) + " " + (collision.gameObject.transform.position.y - otherCol.size.y / 2));

            if (underBlock && leftBlockBorder && rightBlockBorder)
            {
                collision.gameObject.GetComponent<QuestionScript>().Hit();
            }
        }
        /*
        //Colisionar con bloque por el lado
        List<string> groundTags = new List<string>() { "Ground", "Question" };
        if (groundTags.Contains(collision.gameObject.tag) && body.velocity.x != 0)
        {
            BoxCollider2D otherCol = collision.gameObject.GetComponent<BoxCollider2D>();

            bool overBlock = transform.position.y - bCollider.size.y / 2 + 0.1f >= collision.gameObject.transform.position.y + otherCol.size.y / 2;
            bool underBlock = transform.position.y + bCollider.size.y / 2 - 0.1f <= collision.gameObject.transform.position.y - otherCol.size.y / 2;

            if(!overBlock && !underBlock)
            {
                Debug.Log("Hey");
                transform.position = new Vector3(collision.gameObject.transform.position.x + (otherCol.size.x / 2 + bCollider.size.x / 2) * transform.localScale.x * -1, transform.position.y);
                body.velocity = new Vector2(0, body.velocity.y);
            }
        }//*/
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        List<string> groundTags = new List<string>() { "Ground", "Question" };

        //Parado sobre piso
        if(groundTags.Contains(collision.gameObject.tag))
        {
            if (collision.gameObject.transform.position.y + collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2 <= transform.position.y - bCollider.size.y / 2 && body.velocity.y <= 0)
            {
                anim.SetBool("Jump", false);
                grounded = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //levelGenerator.GenerateLevel();   ESTO GENERA UN NIVEL SOBRE EL NIVEL Y POS, NO

            SceneManager.LoadScene("SampleScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "item")
        {
            Destroy(collision.gameObject);
        }


        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        List<string> groundTags = new List<string>() { "Ground", "Question" };

        //Dejar el piso
        if (groundTags.Contains(collision.gameObject.tag))
        {
            grounded = false;   
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        List<string> groundTags = new List<string>() { "Ground", "Question" };

        //Obstaculo al frente
        if (groundTags.Contains(collision.gameObject.tag))
        {
            obstacle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        List<string> groundTags = new List<string>() { "Ground", "Question" };

        //Obstaculo al frente
        if (groundTags.Contains(collision.gameObject.tag))
        {
            obstacle = false;
        }
    }

}
