using UnityEngine;
using System.Collections;

public class loadLevel : MonoBehaviour {

	public string levelToLoad;

	public void LoadStage()  {
		Application.LoadLevel (levelToLoad);
	}

}
