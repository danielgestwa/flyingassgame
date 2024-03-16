using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{

    public int speed = 10;
    public GameObject fart;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private float attackTimePassed;
    private bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float tx = Input.GetAxis("Horizontal") * speed;
        float ty = Input.GetAxis("Vertical") * speed;
        rb.velocity = new Vector2(tx, ty);

        if(rb.velocity.x > 0) {
            sr.flipX = true;
        }
        else if(rb.velocity.x < 0) {
            sr.flipX = false;
        }

        if (Input.GetKeyDown("space") && canAttack) {
            canAttack = false;
            attackTimePassed = 0;
            animator.SetBool("attack", true);

            // Instantiate(fart);
            var created = Instantiate(fart);
            var direction = sr.flipX ? 1 : -1;
            created.transform.position = new Vector2(this.transform.position.x + (1 * direction), this.transform.position.y);
            var fartRb = created.GetComponent<Rigidbody2D>();
            fartRb.velocity = new Vector2((speed + 2) * direction, 0f);
            var fartSr = created.GetComponent<SpriteRenderer>();
            fartSr.flipX = !sr.flipX;
        }

        //Change animation after attack one is finished
        if(!canAttack && attackTimePassed > .4f && animator.GetBool("attack")) {
            animator.SetBool("attack", false);
        }

        //Let player attack after 1s
        if(!canAttack && attackTimePassed > .5f) {
            canAttack = true;
        }

        if(!canAttack) {
            attackTimePassed += Time.deltaTime;
        }
    }
}
