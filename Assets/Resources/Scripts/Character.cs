using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Main class for all Summoned objects, and the Summoners themselves
 * Is the parent class for each summoned object
 * Deals with stats, statuses, materials of objects, and stuff you would see on each summon
 */ 
public class Character : AbstractScript
{
	public enum FACTIONS { LIGHT=0, DARK=1, EARTH=2, WEATHER=3};

	public float maxHp;
    public float hp;
    public int attkRange;
    public int attk;
    public int defense;
    public int move;
	public int zeal;
	public int playerNumber;
    public new string name;
    public string description;
	public string topBarDescription;
    public bool canMove = true;
	public bool canUseZeal = false;
    public int stun = 0;
    public Material theMaterial;
    public int cost;
	protected bool piercing;
	public string iconPath;
	public int faction;
	public MapTile onMapTile = null;

	//literally just to size/space based on cursor size
	private float spacer;

	// Use this for initialization
	protected virtual void Start ()
    {
        if(maxHp == 0)
        {
            maxHp = 1;
            hp = 1;
        }
		zeal = 0;
        hp = maxHp;
		canMove = true;
		spacer = HUB.SPACER;
		iconPath = "Icons/" + name + "Icon";
	}

    // Update is called once per frame
    protected virtual void Update ()
    {
	}
	public void runStart()
	{
		Start();
	}

	public void CreateCharacter()
	{
		Start();
		playerNumber = Turns.getCurrentPlayerTurn();
	}

	public void loseHP(int amt)
	{
		hp -= amt;
		if(hp < 0f)
		{
			if (name == "Summoner")
			{
				getPlayer(playerNumber).setLost(true);
			}
			else
			{
				getPlayer(playerNumber).getSummoner().addMana(cost / 2);
			}
			Destroy(this.gameObject);
		}
	}

	/*
     * Precondition: There are two Characters
     * Postcondition: Character2 will lose some health (minimum of 1)
     * Description: This character deals damage to another character. Called whenever this character moves on the board.
     */
	public virtual void fight(Character char2)
	{
		//this character hits the other one
		battle(this, char2);

		//if that character survives it hits back
		if (char2.hp > 0)
		{
			//if char2 has the range for it
			if (char2.attkRange >= attkRange)
			{
				battle(char2, this);
			}
		}
	}

	/*
	 * This function is basically an extension of fight.
	 * I use this so I can just tell char1 to hit char2 and char2 to hit char1 by calling a single function twice.
	 */
	public void battle(Character char1, Character char2)
	{
		if (char2.playerNumber != 0 && char2.playerNumber != char1.playerNumber)
		{
			if (char1.piercing)//with piecing, characters can simply ignore an opponents defence
			{
				char2.loseHP(char1.attk);
			}
			else
			{
				char2.loseHP(Mathf.Max(char1.attk - char2.defense, 0)); //dont do negative damage, but try to attack!
			}
		}
	}



	public void speak(Character c2)
	{
		if (c2.playerNumber != playerNumber)
		{
			c2.zeal -= (int)(zeal / 5);
		}
		else
		{
			c2.zeal += (int)(zeal / 5);
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

	//gives the grid place position of a unit given their world position
	public int getIntX()
	{
		return realRound(transform.position.x / spacer);
	}
	public int getIntY()
	{
		return realRound(transform.position.y / spacer);
	}

	public bool isAllyTo(Character c)
	{
		if(c.playerNumber == playerNumber)
		{
			return true;
		}
		return false;
	}

	//if this comes up, returns the color of a unit (This is from an early build, but may still be useful)
	public Color getPlayerColor()
	{
		if(playerNumber == 1)
		{
			return Color.red;
		}
		else if(playerNumber == 2)
		{
			return Color.blue;
		}
		return Color.white;
	}

	public int CompareTo(Character b)
	{
		if (b == null)
		{
			return 1;
		}
		return this.name.CompareTo(b.name);
	}


}
