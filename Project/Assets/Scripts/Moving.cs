using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour 
{
	public int playerNumber;
	public bool isController;
	public float speed;

    void Update()
    {
		float x = isController ? Input.GetAxis("Joy" + playerNumber + " Axis1") : Input.GetAxis("Horizontal");
		float y = isController ? Input.GetAxis("Joy" + playerNumber + " Axis2") : Input.GetAxis("Vertical");

		/*
        transform.position += new Vector3(x, y) * Time.deltaTime;
*/
		if(Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f)
		{
			float direction = Mathf.Rad2Deg * Mathf.Atan2(-x,y);

			transform.eulerAngles = new Vector3(0,0,direction);
		}


		transform.position += transform.up * Time.deltaTime * speed;
    }
}
