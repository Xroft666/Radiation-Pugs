using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Joy1" + " Pee") || Input.GetButtonDown("Joy2" + " Pee") || Input.GetButtonDown("Joy3" + " Pee") || Input.GetButtonDown("Joy4" + " Pee"))
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
