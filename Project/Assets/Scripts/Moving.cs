using UnityEngine;
using System.Collections;

public class Moving : MonoBehaviour 
{

    void Update()
    {
		float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

		/*
        transform.position += new Vector3(x, y) * Time.deltaTime;
*/
		if(x != 0 || y != 0)
		{
			float direction = Mathf.Rad2Deg * Mathf.Atan2(-x,y);

			transform.eulerAngles = new Vector3(0,0,direction);
		}

		transform.position += transform.up * Time.deltaTime;
    }
}
