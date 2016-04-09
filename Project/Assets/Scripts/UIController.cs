using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public Text[] playersCounters;
	
    public void Start()
    {
        LevelGrid.Instance.OnCounterChanged += OnAreaChanged;
    }

    private void OnAreaChanged(PlayerEnum id, int num)
    {
		playersCounters[(int)id].text = (num / (float)(LevelGrid.gridResolution)*2).ToString("0.0") + "%";
       
    }
}
