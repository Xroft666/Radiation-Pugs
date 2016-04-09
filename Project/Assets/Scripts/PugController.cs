using UnityEngine;
using System.Collections;

public class PugController : MonoBehaviour 
{
    public PlayerEnum playerID;

	public bool isController;
	public float speed = 0.85f;

    public float amountOfPee = 100;
    public float peeDepletionRate = 20f;
    public float peeAdditionRate = 50f;

    [HideInInspector]
    public bool drinking = false;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		if(States.isStart && !States.won)
		{
			float x = isController ? Input.GetAxis("Joy" + ((int)playerID + 1) + " Axis1") : Input.GetAxis("Horizontal");
			float y = isController ? Input.GetAxis("Joy" + ((int)playerID + 1) + " Axis2") : Input.GetAxis("Vertical");
		

            Vector3 velocity = (new Vector3(x, y)).normalized;
            
            animator.SetFloat("speed", velocity.magnitude);
            
            
            transform.position += velocity * speed * Time.deltaTime;
            
            if(x != 0f)
                spriteRenderer.flipX = x < 0f ? false : true;
            
            if(y != 0f)
                animator.SetBool("frontSide", y < 0f);

		}

        if(States.isStart && !States.won)
        {
            bool isPeeing = isController ? Input.GetButton("Joy" + ((int)playerID + 1) + " Pee") : Input.GetKey(KeyCode.Z);
            animator.SetBool("peeing", isPeeing);

            if(isPeeing && amountOfPee > 0)
            {
                amountOfPee -= peeDepletionRate * Time.deltaTime;

                LevelGrid.Instance.SetGridOwner(transform.position.x, transform.position.y, (PlayerEnum) playerID);
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
    }
}
