using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {

	void Update()
	{
		if(Input.GetKey(KeyCode.Z))
		{
			LoadStage();
		}
	}

	public void LoadStage()  {
		Application.LoadLevel (1);
	}

}
