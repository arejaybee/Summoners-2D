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
		GameObject sum = GameObject.Find("Summoner" + playerNumber+"(Clone)");
		if(sum != null)
		{
			manaVal = sum.GetComponent<Summoner>().mana;
		}
		else
		{
			manaVal = 0;
		}
		mana = "Mana\n\n" + manaVal.ToString("D4");
		transform.FindChild("Mana").GetComponent<TextMesh>().text = mana;
	}
}
