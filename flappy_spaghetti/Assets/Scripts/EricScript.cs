using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;
    private Animator anim;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        myRigidbody.velocity = Vector2.up * flapStrength * 1.6f;
        logic.theme.Play();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myRigidbody.velocity = Vector2.up * flapStrength;
            anim.SetTrigger("Flap");
        }
        if (myRigidbody.position.y < -17 || myRigidbody.position.y > 17)
        {
            anim.SetTrigger("Die");
            logic.gameOver();
            
        }
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        anim.SetTrigger("Die");
        logic.gameOver();
        
        birdIsAlive = false;
    }
}