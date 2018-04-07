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
	public ManaCounters manaCounter;
	// Use this for initialization
	protected override void Start ()
	{
		
		name = "Summoner";
		base.Start();
		zeal = 20;
		maxHp = 25;
		hp = maxHp;
		move = 3;
		attkRange = 3;
		summonRange = 3;
		attk = 5;
		defense = 1;
		cost = 0;
		canMove = true;
		canUseZeal = true;
		armySize = 10;
		numUnits = 0;

		ManaCounters[] mc = FindObjectsOfType<ManaCounters>();
		foreach (ManaCounters m in mc)
		{
			if (m.playerNumber == playerNumber)
			{
				manaCounter = m;
			}
		}

		if (playerNumber == 1)
		{
			addMana(5);
		}
		else
		{
			mana = 0;
		}
		topBarDescription = "These masters of magic summon armies to fight for them.\nIf you lose this unit, you lose!";
	}
	public void addMana(int manaAmt)
	{
		mana += manaAmt;
		mana = Mathf.Max(mana, 0);
		manaCounter.setMana(mana);
	}
	public void spendMana(int cost)
	{
		if(mana > cost)
			mana -= cost;
		mana = Mathf.Max(mana, 0);
		manaCounter.setMana(mana);
	}
	
}
