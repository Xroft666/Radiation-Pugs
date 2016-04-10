using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class winTable : MonoBehaviour {



	// Use this for initialization
	void Start () {
		foreach(Transform child in transform)
		{
			child.GetComponent<RectTransform>().sizeDelta = new Vector2(child.GetComponent<RectTransform>().sizeDelta.x,0);
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(KeyValuePair<PlayerEnum, float> cellHeight in LevelGrid.pointsCounter)
		{
			float procentage = ((cellHeight.Value / (float)(LevelGrid.gridResolution))*2)*10;
			Transform Bar = transform.GetChild((int)cellHeight.Key);

			if(Bar.GetComponent<RectTransform>().sizeDelta.y < procentage)
			{
				Bar.GetComponent<RectTransform>().sizeDelta = new Vector2(Bar.GetComponent<RectTransform>().sizeDelta.x, Bar.GetComponent<RectTransform>().sizeDelta.y + 10f * Time.deltaTime);
			}
			else
			{
				Bar.GetComponent<RectTransform>().sizeDelta = new Vector2(Bar.GetComponent<RectTransform>().sizeDelta.x, procentage);
			}
		}
	}
}
