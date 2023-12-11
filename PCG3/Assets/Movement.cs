using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Movement : MonoBehaviour
{
    [SerializeField] GameObject levelController;
    LevelList levelList;

    Animator anim;
    Rigidbody2D body;

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
        levelList = levelController.GetComponent<LevelList>();
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

        if(transform.position.y < -5)
        {
            levelList.LoadCurentLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            levelList.LoadCurentLevel();
        }

        if (collision.gameObject.tag == "Finish")
        {
            levelList.GoNextLevel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "item")
        {
            collision.gameObject.SetActive(false);
        }
    }

    public void SetGroundedState(bool status)
    {
        grounded = status;
        anim.SetBool("Jump", !status);
    }

    public void SetObstacleState(bool status)
    {
        obstacle = status;
    }


}
