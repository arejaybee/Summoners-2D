using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Character {
    public const int FAIRY_MANA = 3;
	// Use this for initialization
	protected override void Start ()
    {
		name = "Fairy";
		base.Start();
		zeal = 99;
        maxHp = 1;
        hp = 1;
        move = 2;
        attkRange = 1;
        attk = 1;
        defense = 0;
        cost = 2;
        description = "+3 mana per turn";
		topBarDescription = "These magical creatures will aid their summoner with 3 mana every turn.";
        canMove = true;
    }

    //Fairies add 3 to the player's mana pool each turn
    public override void EndTurn()
    {
		hub.getPlayer(playerNumber).getSummoner().mana += 3;
    }
}
