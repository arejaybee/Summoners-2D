using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard : Character
{
	// Use this for initialization
	protected override void Start()
	{
		name = "Bard";
		base.Start();
		maxHp = 10;
		hp = 10;
		move = 3;
		attkRange = 1;
		attk = 2;
		defense = 0;
		cost = 20;
		zeal = 50;
		canMove = true;
		canUseZeal = true;
		description = "Can convert opponents.";
		topBarDescription = "With the power of the pen, these units can convince others' sword\nto join your cause.";
	}
}
