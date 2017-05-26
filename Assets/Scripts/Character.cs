using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Main class for all Summoned objects, and the Summoners themselves
 * Is the parent class for each summoned object
 * Deals with stats, statuses, materials of objects, and stuff you would see on each summon
 */ 
public class Character : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float attkRange;
    public float attk;
    public float defense;
    public float move;
    public float playerNumber;
    public new string name;
    public string description;
    public bool canMove = true;
    public int stun = 0;
    public Material theMaterial;
    public int cost;
    public string extraDescription = "";
	protected bool piercing;
	public bool counterAttack;
	// Use this for initialization
	void Start ()
    {
        if(maxHp == 0)
        {
            maxHp = 1;
            hp = 1;
        }
        hp = maxHp;
        description = name + "\n HP: " + hp + "/" + maxHp + "\n Attk: " + attk + " Def: " + defense + "\n Attk Range: " + attkRange + "\n Move: "+move+extraDescription;
        canMove = true;
		counterAttack = false;
		piercing = false;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        if (hp <= 0f)
        {
            Destroy(this.gameObject);
        }
        description = name + "\n HP: " + hp + "/" + maxHp + "\n Attk: " + attk + " Def: " + defense + "\n Attk Range: " + attkRange + "\n Move: " + move+extraDescription;
        if(stun > 0)
        {
            description = description + "\nStun for: " + Mathf.CeilToInt((float)(stun-1) / 2) + " rounds";
        }
    }

    /*
     * Precondition: There are two Characters
     * Postcondition: Character2 will lose some health (minimum of 1)
     * Description: This character deals damage to another character. Called whenever this character moves on the board.
     */
    public virtual void fight(Character char2)
    {
        if (char2.playerNumber != 0 && char2.playerNumber != playerNumber)
        {
			if (piercing)
			{
				char2.hp -= attk;
			}
			else
			{
				char2.hp = char2.hp - Mathf.Max(attk - char2.defense, 0); //dont do negative damage, but try to attack!
			}
            //When a Gorgan is hit by a creature, that creature is stunned for 1 round
            if(char2.name == "Gorgon" && name != "Gorgon")
            {
                stun = 3;
				canMove = false;
            }

			//players get some mana back if their unit dies
			if (char2.hp <= 0)
			{
				GameObject.Find("Summoner" + char2.playerNumber).GetComponent<Summoner>().mana += (int)(char2.cost / 2);
			}
			if(char2.hp > 0 && char2.counterAttack)
			{
				char2.counter(this);
			}
		}

    }

	/*
	 * This is speciffically for units that may attack back
	 */ 
	public virtual void counter(Character char2)
	{
		if (char2.playerNumber != 0)
		{
			//char2.hp = char2.hp - Mathf.Max(attk - char2.defense, 1); //original intention is to always do a min of 1 damage
			char2.hp = char2.hp - Mathf.Max(attk - char2.defense, 0); //dont do negative damage, but try to attack!

			//When a Gorgan is hit by a creature, that creature is stunned for 1 round
			if (char2.name == "Gorgon" && name != "Gorgon")
			{
				stun = 3;
				canMove = false;
			}
		}
		//players get some mana back if their unit dies
		if (char2.hp <= 0)
		{
			GameObject.Find("Summoner" + char2.playerNumber).GetComponent<Summoner>().mana += (int)(char2.cost / 2);
		}
	}

	/*
     * A function to set the playerNum value to either 1 or 2
     * Could, in the future, be used to steal other player's units (A vampire summon?)
     */
	public void setPlayerNum(int num)
    {
        playerNumber = num;
    }

    /*
     * A function to set teh canMove value to true or false
     */
    public void setCanMove(bool canmove)
    {
        canMove = canmove;
    }

    /*
     * A functional way to get the cost of a character
     */ 
     public int getCost()
    {
        return cost;
    }

    /*
     * Some characters will do special things at the end turn (for now just dragons and fairies)
     */
   public virtual void EndTurn()
    {
        //this is for units with abilities
    }
}
