using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : Character {
	public int armySize;
	public int numUnits;
	public int summonRange;
	public int mana;
	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		name = "Summoner";
		maxHp = 25;
		hp = maxHp;
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
		extraDescription = "These masters of magic summon armies to fight for them.\nIf you lose this unit, you lose!";
	}
	protected override void Update()
	{
		base.Update();
		mana = Mathf.Max(mana, 0);
	}
	
}
