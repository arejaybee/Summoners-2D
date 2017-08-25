using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai : Character {

	// Use this for initialization
	protected override void Start()
	{
		name = "Samurai";
		base.Start();
		maxHp = 15;
		hp = 15;
		move = 3;
		attkRange = 1;
		attk = 8;
		defense = 0;
		cost = 30;
		zeal = 999;
		canMove = true;
		description = "Do not swap sides.";
		topBarDescription = "Master of the sword that would rather die than join the enemy.";
	}
	protected override void Update()
	{
		//at 0hp, they die!
		if (hp <= 0f)
		{
			Destroy(this.gameObject);
		}
		if(zeal != 999)
		{
			zeal = 999;
		}
	}
}
