using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Character {
	public int armySize;
	public int numUnits;
	public int summonRange;
	public int mana;
	// Use this for initialization
	void Start ()
	{
		name = "Summoner";
		maxHp = 25;
		hp = 25;
		move = 2;
		attkRange = 3;
		summonRange = 1;
		attk = 5;
		defense = 1;
		cost = 0;
		canMove = true;
		armySize = 10;
		numUnits = 0;
		mana = 5;
	}
	protected override void Update()
	{
		base.Update();
		mana = Mathf.Max(mana, 0);
	}
	
}
