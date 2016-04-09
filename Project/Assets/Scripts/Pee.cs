using UnityEngine;
using System.Collections;

public class Pee : MonoBehaviour {

	public GameObject prefab;
	public int playerNumber;
	public float amountOfPee;
	public float peeDepletionRate = 0.3f;
	public float peeAdditionRate = 0.3f;

	public bool drinking = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButton("Joy" + playerNumber + " Pee") && amountOfPee > 0)
		{
			Instantiate(prefab, transform.position, Quaternion.identity);
			amountOfPee -= peeDepletionRate * Time.deltaTime;

            LevelGrid.Instance.SetGridOwner(transform.position.x, transform.position.y, PlayerEnum(playerNumber - 1));
		}
		else if(Input.GetButton("Joy" + playerNumber + " Pee"))
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
