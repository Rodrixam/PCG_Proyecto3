using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionScript : MonoBehaviour
{
    Animator anim;
    bool hit = false;

    [SerializeField] GameObject prize;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit()
    {
        if (!anim.GetBool("Hit"))
        {
            anim.SetBool("Hit", true);
            hit = true;
        }
    }
}
