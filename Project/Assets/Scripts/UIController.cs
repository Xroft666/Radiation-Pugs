using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
    public Text[] playersCounters;
	public PugController[] players;
	
    public void Start()
    {
        LevelGrid.Instance.OnCounterChanged += OnAreaChanged;
    }

    private void OnAreaChanged(PlayerEnum id, float num)
    {
		playersCounters[(int)id].text = "" + players[(int)id].score;
       
    }
}
