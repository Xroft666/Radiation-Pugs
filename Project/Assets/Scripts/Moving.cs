using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour 
{
    public PlayerEnum playerID;

	public bool isController;
	public float speed;

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
    }
}
