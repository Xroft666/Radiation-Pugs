using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {

	void Update()
	{
		if(Input.GetKey(KeyCode.Z) || Input.GetButton("Joy1" + " Pee") || Input.GetButton("Joy2" + " Pee") || Input.GetButton("Joy3" + " Pee") || Input.GetButton("Joy4" + " Pee"))
		{
			LoadStage();
		}
	}

	public void LoadStage()  {
		Application.LoadLevel (1);
		States.won = false;
		States.isStart = true;
	}

}
