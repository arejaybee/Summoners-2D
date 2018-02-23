using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Character
{
	protected override void Start()
	{
		name = "Palidin";
		faction = (int)FACTIONS.LIGHT;
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 10;
		attkRange = 10;
		attk = 10;
		defense = 10;
		cost = 10;
		zeal = 10;
		canMove = true;
		description = "Defensive Bois";
		topBarDescription = "Devout followers of the holy light.\n Few can get past their armor and shileds";//(need to implement different costs for tile movements)
		

	}
}
