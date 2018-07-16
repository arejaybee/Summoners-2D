using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : AbstractScript {
	public static bool cursorCanMove;
	public bool canSelect;//if this is true, we can pick up a unit
	public bool summoning;
	public bool attacking;
	public Character selectedCharacter;
	private MapTile charOrgMapTile;
	private Character unselectableSummoner;
	private Character fightingCharacter;
	public MapTile onMapTile;

	// Use this for initialization
	void Start()
	{
		transform.localScale.Set(HUB.SIZE, HUB.SIZE, 1);
		//we do not start with a character started
		selectedCharacter = null;
		canSelect = true;
		cursorCanMove = true;
		summoning = false;
		attacking = false;
	}
	// Update is called once per frame
	void Update ()
	{
		//if the cursor can move, see if it is being moved
		if (cursorCanMove && !PauseMenuController.isPaused)
		{
			DetectMovement();
		}
		//MouseMovement();
		//MobileTouchMovement();

		//when the cursor switches to attack mode, we need to know which 
		//character is attacking, but also dont want to move the character.
		if (attacking)
		{
			if (selectedCharacter != null)
			{
				selectedCharacter.canMove = false;
				fightingCharacter = selectedCharacter;
				selectedCharacter = null;
			}
		}
	}

	private bool isOnCharacter()
	{
		if (!cursorCanMove)
		{
			hub.TOP_BAR.setTopBarActive(false, null);
			return false;
		}
		if(onMapTile.getCharacterOnTile() != null)
		{
			hub.TOP_BAR.setTopBarActive(true, hub.CURSOR.getOnMapTile().getCharacterOnTile());
			return true;
		}
		hub.TOP_BAR.setTopBarActive(false, null);
		return false;
	}

	//used to move the cursor
	void DetectMovement()
	{
		//mv shows where the cursor has moved to. This is so I can move the camera with the cursor
		Vector3 mv = transform.position;

		//Each key only gives input every tenth of a second. This keeps the cursor
		//moving smoothley

		//up
		if (Turns.getCurrentPlayer().getGamepad().isPressed("up") && onMapTile.getNorthTile() != null && Time.time - hub.LAST_TIME_UP >= 0.1f)
		{
			hub.LAST_TIME_UP = Time.time;
			mv = onMapTile.getNorthTile().transform.position;
			setOnMapTile(onMapTile.getNorthTile());
		}
		//down
		if (Turns.getCurrentPlayer().getGamepad().isPressed("down") && onMapTile.getSouthTile() != null && Time.time - hub.LAST_TIME_DOWN >= 0.1f)
		{
			hub.LAST_TIME_DOWN = Time.time;
			mv = onMapTile.getSouthTile().transform.position;
			setOnMapTile(onMapTile.getSouthTile());
		}
		//right
		if (Turns.getCurrentPlayer().getGamepad().isPressed("right") && onMapTile.getEastTile() != null && Time.time - hub.LAST_TIME_RIGHT >= 0.1f)
		{
			hub.LAST_TIME_RIGHT = Time.time;
			mv = onMapTile.getEastTile().transform.position;
			setOnMapTile(onMapTile.getEastTile());
		}
		//left
		if (Turns.getCurrentPlayer().getGamepad().isPressed("left") && onMapTile.getWestTile() !=null && Time.time - hub.LAST_TIME_LEFT >= 0.1f)
		{
			hub.LAST_TIME_LEFT = Time.time;
			mv = onMapTile.getWestTile().transform.position;
			setOnMapTile(onMapTile = onMapTile.getWestTile());
		}
		
		//for confirm and cancel, I wait a bit longer between inputs to help stop the game
		//from canceling and confirming between menus. Sometimes this can be noticable, but hardly..

		//on confirm
		if(Turns.getCurrentPlayer().getGamepad().isPressed("confirm") && Time.time - hub.LAST_TIME_CONFIRM >= 0.25f)
		{
			hub.LAST_TIME_CONFIRM = Time.time;

			//if you have not selected a character, do so
			if (canSelect && !summoning && !attacking)
			{
				DetectSelect();
			}

			//if the character hits confirm, check if there is a summon tile, if so, place the unit there
			else if(summoning)
			{
				if(isOnTile("SummonTile"))
				{
					canSelect = true;
					summoning = false;
					hub.RemoveTiles("SummonTile");
					selectedCharacter = null;
				}
			}
			//if the player is currently trying to select an attack target
			else if (attacking)
			{
				if(isOnTile("EnemyTile") && onMapTile.getCharacterOnTile() != null)
				{
					canSelect = true;
					attacking = false;
					hub.RemoveTiles("EnemyTile");
					//print(fightingCharacter.name);
					fightingCharacter.fight(onMapTile.getCharacterOnTile());
					confirmFromMoveMenu();
				}
			}
			//if you have a character selected, give them options from this point
			else if(isOnTile("MoveTile"))
			{
				GoToMoveMenu();
			}
		}

		//on cancel
		if(Turns.getCurrentPlayer().getGamepad().isPressed("cancel") && Time.time - hub.LAST_TIME_CANCEL >= 0.25f && !summoning)
		{
			hub.LAST_TIME_CANCEL = Time.time;
			if (!canSelect)
			{
				hub.moveCharacter(selectedCharacter, charOrgMapTile);
			}
			if (selectedCharacter != null)
			{
				selectedCharacter = null;
			}
			canSelect = true;

			//hub.Remove the extra tiles
			hub.RemoveTiles("MoveTile");
			hub.RemoveTiles("EnemyTile");
			hub.RemoveTiles("SummonTile");
		}

		hub.CAMERA_CONTROLLER.moveCamera(mv);
	}

	bool isOnTile(string name)
	{
		bool ret = false;
		switch(name)
		{
			case ("SummonTile"):
				if(hub.summonPositions.Contains(onMapTile))
				{
					ret = true;
				}
				break;
			case ("MoveTile"):
				if (hub.moveTilePositions.Contains(onMapTile))
				{
					ret = true;
				}
				break;
			case ("EnemyTile"):
				if(hub.enemyPositions.Contains(onMapTile))
				{
					ret = true;
				}
				break;
			default:
				return false;
		}
		return ret;
	}

	//Enters the MoveMenu script.
	//Gives players options after they have moved
	void GoToMoveMenu()
	{
		ArrayList list = new ArrayList();
		//if you are a summoner and can summon, you get the summon option
		if (selectedCharacter.name.Contains("Summoner") && hub.currentSummonerCanSummon())
		{
			list.Add("Summon");
		}
		//if there are enemies within your attack range, you get the attack option
		if(hub.enemyInRange(selectedCharacter))
		{
			list.Add("Attack");
		}
		//add Heal

		//you can only stop if there are no characters on the spot you are on.
		if (onMapTile.getCharacterOnTile() != null)
		{
			list.Add("Stop");
		}

		//It may be that no options were added, in which case, pretend we never hit this function
		if (list.Count != 0)
		{
			cursorCanMove = false;
			Vector3 pos = transform.position + new Vector3(2 * HUB.SPACER, HUB.SPACER, 0);
			hub.MOVE_MENU_CONTROLLER.MakeMoveMenu(list, pos);
		}
	}

	//this is set up to be called from the moveMenu so that this stuff is only set IF they confirm.
	//on cancel we just move around again
	public void confirmFromMoveMenu()
	{
		if (selectedCharacter != null && !summoning)
		{
			selectedCharacter.canMove = false;
			selectedCharacter = null;
		}
		hub.enemyPositions.Clear();
		canSelect = true;
		cursorCanMove = true;
		//hub.Remove the move tiles
		hub.RemoveTiles("MoveTile");
	}
	/*
	 * This function will see if there is a character to select
	 * It is implied that when you get to this call you have tried selecting something
	 * */
	void DetectSelect()
	{
		Character c = onMapTile.getCharacterOnTile();

		//if you hit a character, and you can select something
		if (c != null && canSelect)
		{
			//if its that characters turn
			if (c.playerNumber == Turns.getCurrentPlayerTurn())
			{
				//if they can still move
				if (c.canMove)
				{
					//select them with the cursor
					selectedCharacter = c;
					canSelect = false;
					charOrgMapTile = onMapTile;
				}
				//if theyre a summoner, give them the chance to summon even if they cannot move
				else if (c.name == "Summoner" && hub.currentSummonerCanSummon())
				{
					cursorCanMove = false;
					ArrayList list = new ArrayList();
					list.Add("Summon");
					Vector3 pos = transform.position + new Vector3(2 * HUB.SPACER, HUB.SPACER, 0);
					hub.MOVE_MENU_CONTROLLER.MakeMoveMenu(list, pos);
				}
			}
		}
		if (!canSelect) 
		{
			//displays all of the possible spaces that character can move to
			hub.FindMoveTiles(selectedCharacter.move, onMapTile,selectedCharacter);
			hub.MakeTiles("MoveTile");
		}
	}

	//returns the in-grid x,y cooridnate of the cursor
	public int getIntX()
	{

		return realRound(transform.position.x/HUB.SPACER);
	}
	public int getIntY()
	{
		return realRound(transform.position.y / HUB.SPACER);
	}
	public void setOnMapTile(MapTile mt)
	{
		onMapTile = mt;
		if (selectedCharacter != null)
		{
			if (selectedCharacter.onMapTile != null)
			{ 
				selectedCharacter.onMapTile.setCharacterOnTile(null);
			}
			selectedCharacter.onMapTile = mt;
			mt.setCharacterOnTile(selectedCharacter);
			selectedCharacter.transform.position = mt.transform.position;
		}
		transform.position = mt.transform.position;
		isOnCharacter();
	}
	public MapTile getOnMapTile()
	{
		return onMapTile;
	}
}
