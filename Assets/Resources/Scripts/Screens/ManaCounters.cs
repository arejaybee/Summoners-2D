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

	public void setMana(int theMana)
	{
		manaVal = theMana;
		mana = "Mana\n\n" + manaVal.ToString("D4");//D4 means the number will have 4 digits eg: 2 -> 0002
		transform.Find("Mana").GetComponent<TextMesh>().text = mana;
	}
}
