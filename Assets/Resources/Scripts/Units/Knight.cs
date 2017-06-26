using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{

	// Use this for initialization
	protected override void Start ()
	{
		name = "Knight";
		maxHp = 10;
		hp = 10;
		move = 4;
		attkRange = 1;
		attk = 3;
		defense = 1;
		cost = 20;
		canMove = true;
		counterAttack = true;
		extraDescription = "\nAttacks back when\nAttacked.\nCounter deals double.";
	}
}
