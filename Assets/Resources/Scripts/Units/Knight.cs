using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{

	// Use this for initialization
	protected override void Start ()
	{
		name = "Knight";
		base.Start();
		maxHp = 10;
		hp = 10;
		zeal = 25;
		move = 3;
		attkRange = 1;
		attk = 4;
		defense = 1;
		cost = 5;
		canMove = true;
		canUseZeal = true;
		description = "Strong and sturdy\nAnd able to use zeal";
		topBarDescription = "These durable masters of the sword are as loyal.\nThese units can use their words as often as their swords";
	}
}
