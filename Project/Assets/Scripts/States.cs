using UnityEngine;
using System.Collections;

public class States : MonoBehaviour {

	public static bool isStart = true;
	public static bool won = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(won)
		{
			Application.LoadLevel (2);
		}
	}
}
