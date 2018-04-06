using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCounters : MonoBehaviour
{
	private string mana;
	private int manaVal;
	public int playerNumber;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		//find the summoner corresponding to this manacounter,and set the counter to their mana total
		Summoner sum = Turns.getCurrentSummoner();
		if(sum != null)
		{
			manaVal = sum.mana;
		}
		else
		{
			manaVal = 0;
		}
		mana = "Mana\n\n" + manaVal.ToString("D4");//D4 means the number will have 4 digits eg: 2 -> 0002
		if (sum.playerNumber == playerNumber)
		{
			transform.Find("Mana").GetComponent<TextMesh>().text = mana;
		}
	}
}
