using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Counter : MonoBehaviour {

	public float time;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(States.isStart)
		{
			if(time > 0)
			{
				time -= Time.deltaTime;
			}
			else
			{
				time = 0;
				States.won = true;
			}

			int minutes = (int)time / 60;
			int seconds = (int)time % 60;
			int fraction = (int)((time * 100) % 100);

			string counterText = string.Format ("{0:00}:{1:00}", minutes, seconds); 

			transform.GetComponent<Text>().text = counterText;
		}
	}
}
