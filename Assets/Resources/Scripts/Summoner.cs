using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A summoner is a type of unit that can summon other units. 
 */
public class Summoner : Character {
	public int armySize;
	public int numUnits;
	public int summonRange;
	public int mana;
	// Use this for initialization
	protected override void Start ()
	{
		
		name = "Summoner";
		zeal = 100;
		maxHp = 25;
		hp = maxHp;
		move = 3;
		attkRange = 3;
		summonRange = 3;
		attk = 5;
		defense = 1;
		cost = 0;
		canMove = true;
		armySize = 10;
		numUnits = 0;
		mana = 5;
		base.Start();
		topBarDescription = "These masters of magic summon armies to fight for them.\nIf you lose this unit, you lose!";
	}
	protected override void Update()
	{
		base.Update();
		mana = Mathf.Max(mana, 0);
	}
	
}
