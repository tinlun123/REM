﻿using UnityEngine;
using System.Collections;

public class BunnyScript : MonoBehaviour {
    
    // TODO: jumping
    //public float jumpStrength;
    //public float jumpCooldown;  // Cooldown before bunny can jump again, in seconds.
    //private float jumpTimer = 0;  // Used to time jumping.

    // Walking time variables.
    // Bunny will walk in a cycle, walking a bit, then waiting a bit, etc...
    public float minWalkSpeed;
    public float maxWalkSpeed;
    private float walkSpeed;
    public float minWalkTime;  // All time variables will be in seconds.
    public float maxWalkTime;
    public float minWalkWaitTime;
    public float maxWalkWaitTime;
    private bool isWalking = false;
    private float walkTimer = 0;
    private int walkDirection = 1;

    [System.NonSerialized]
    public bool isAggro = false;
    private FlowerScript flowerScript;

    // Aggro bunny variables
    private bool aggroSwitch = false;  // Used to do things when switching to aggro only once.
    private float jumpTimer = 0;
    public float jumpTime;
    public float maxJumpForce;
    public float jumpAngle;  // In degrees

    public Transform groundChecker;  // A point on the bunny, used to check for ground.
    public LayerMask groundLayerMask;  // Only check for ground using ground, and nothing else.
    private float groundCheckerRadius;  // How large of a circle the checker is.
    private bool grounded;



    private Rigidbody2D rigidbody2D;
    
	// Use this for initialization
	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        flowerScript = GameObject.FindWithTag("Flower").GetComponent<FlowerScript>();
        groundCheckerRadius = transform.GetChild(0).GetComponent<CircleCollider2D>().radius;
        //print(Mathf.Cos(Mathf.Deg2Rad * 60));
	}
	
	// Update is called once per frame
	void Update () {
        
        if (flowerScript.saturationPoints > 0) {  // If flower has stuffs in it.
            if (!isAggro) {
                isAggro = true;  // Bunny becomes aggro.
                aggroSwitch = true;
            }
        } else {
            isAggro = false;
        }
        
        if (!isAggro) {
            // Passive mode
            walkTimer -= Time.deltaTime;  // Decrement timer
            if (walkTimer < 0) {
                // Timer reset
                isWalking = !isWalking;  // Switch walking/waiting state.
                print(isWalking);
                if (isWalking) {
                    walkTimer = Random.Range(minWalkTime, maxWalkTime);
                    walkDirection = (Random.value < 0.5f) ? -1 : 1;  // Randomize walk direction (-1 = left, 1 = right)
                    walkSpeed = Random.Range(minWalkSpeed, maxWalkSpeed);

                } else {
                    walkTimer = Random.Range(minWalkWaitTime, maxWalkWaitTime);
                }
            }
            if (!isWalking) {
                // Waiting
                rigidbody2D.velocity = new Vector3(0, rigidbody2D.velocity.y, 0);
            } else {
                rigidbody2D.velocity = new Vector3(walkSpeed * walkDirection, rigidbody2D.velocity.y, 0);
            }
        } else { 
            // Aggro
            grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayerMask);
            if (aggroSwitch) {
                aggroSwitch = false;
                rigidbody2D.velocity = Vector3.zero;
                print("aggro");
            }
            jumpTimer -= Time.deltaTime;
            if (grounded) {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
            if (jumpTimer < 0) {
                print("jump");
                jumpTimer = jumpTime;
                //rigidbody2D.velocity = new Vector3(maxJumpForce * Mathf.Cos(Mathf.Deg2Rad * jumpAngle), maxJumpForce * Mathf.Sin(Mathf.Deg2Rad * jumpAngle), 0);
                rigidbody2D.AddForce(new Vector2(maxJumpForce * Mathf.Cos(Mathf.Deg2Rad * jumpAngle), maxJumpForce * Mathf.Sin(Mathf.Deg2Rad * jumpAngle)));
            }
            
        }
	}
}
