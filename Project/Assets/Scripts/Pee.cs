using UnityEngine;
using System.Collections;

public class Pee : MonoBehaviour {

	public GameObject prefab;
	public int playerNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButton("Joy" + playerNumber + " Pee"))
		{
			//Instantiate(prefab, transform.position, Quaternion.identity);
		}
		else
		{
			Instantiate(prefab, transform.position, Quaternion.identity);
		}
	}
}
