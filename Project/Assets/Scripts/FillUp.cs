using UnityEngine;
using System.Collections;

public class FillUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.transform.tag == "pug")
		{
			other.GetComponent<PugController>().drinking = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.transform.tag == "pug")
		{
            other.GetComponent<PugController>().drinking = false;
		}
	}
}
