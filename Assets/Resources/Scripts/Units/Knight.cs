using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{

	// Use this for initialization
	protected override void Start ()
	{
		base.Start();
		name = "Knight";
		maxHp = 10;
		hp = 10;
		zeal = 50;
		move = 3;
		attkRange = 1;
		attk = 4;
		defense = 2;
		cost = 5;
		canMove = true;
		counterAttack = true;
		description = "Strong and sturdy\nArmored units.";
		topBarDescription = "Durable masters of the sword. They are as loyal as they are expendable.";
	}
}
