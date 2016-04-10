﻿using UnityEngine;
using System.Collections;

public class PugController : MonoBehaviour 
{
    public PlayerEnum playerID;

	public bool isController;
	public float speed = 0.85f;

    public float amountOfPee = 100;
    public float peeDepletionRate = 20f;
    public float peeAdditionRate = 50f;
	public float peePoints = 2;

    [HideInInspector]
    public bool drinking = false;
    [HideInInspector]
    public bool isHavingShoe = false;

    public Transform shoePivot;

	public int score = 0;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private GameObject shoeAvailable;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		if(!States.isStart || States.won)
            return;
		
		float x = isController ? Input.GetAxis("Joy" + ((int)playerID + 1) + " Axis1") : Input.GetAxis("Horizontal");
		float y = isController ? Input.GetAxis("Joy" + ((int)playerID + 1) + " Axis2") : Input.GetAxis("Vertical");
		

        Vector3 velocity = (new Vector3(x, y)).normalized;
        
        animator.SetFloat("speed", velocity.magnitude);
        
        GridCell currentCell = LevelGrid.Instance.GetCell(transform.position.x, transform.position.y);

        float modifier = 1f;
        if(currentCell.owner == PlayerEnum.None)
        {
            
        }
        else
        if(currentCell.owner == playerID)
        {
            modifier = 1.4f;
        }
        else
        {
            modifier = 0.9f;
        }

        transform.position += velocity * modifier * speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(shoeAvailable != null && !isHavingShoe)
            {
                shoeAvailable.transform.parent = shoePivot;
                shoePivot.transform.localPosition = Vector3.zero;

                isHavingShoe = true;
            }
            else
            if(isHavingShoe)
            {
                shoePivot.GetChild(0).transform.parent = null;

                isHavingShoe = false;
            }
        }



        Vector3 shoeLocalPos = shoePivot.transform.localPosition;
        if(x != 0f)
        {
            spriteRenderer.flipX = x < 0f ? false : true;
            shoeLocalPos.x = x < 0f ? -0.04f : 0.15f;

        }
        
        if(y != 0f)
        {
            animator.SetBool("frontSide", y < 0f);
            shoeLocalPos.y = y < 0f ? -0.05f : 0.05f;
            shoeLocalPos.z = y < 0f ? -1f : 1f;
        }

        shoePivot.transform.localPosition = shoeLocalPos;

        bool isPeeing = isController ? Input.GetButton("Joy" + ((int)playerID + 1) + " Pee") : Input.GetKey(KeyCode.Z);
        animator.SetBool("peeing", isPeeing);

        if(isPeeing && amountOfPee > 0)
        {
            amountOfPee -= peeDepletionRate * Time.deltaTime;

            LevelGrid.Instance.SetGridOwner(transform.position.x, transform.position.y, (PlayerEnum) playerID);
			score += (int)peePoints;
        }
        else if(isPeeing)
        {
            amountOfPee = 0;
        }

        if(drinking && amountOfPee < 100)
        {
			amountOfPee += peeAdditionRate * Time.deltaTime;
        }
        else if(drinking)
        {
            amountOfPee = 100;
        }

    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        switch(other.transform.tag)
        {
            case "water":
                drinking = true;
                break;
            case "shoe":
                shoeAvailable = other.gameObject;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        switch(other.transform.tag)
        {
            case "water":
                drinking = false;
                break;
            case "shoe":
                shoeAvailable = null;
                break;
        }
    }
}
