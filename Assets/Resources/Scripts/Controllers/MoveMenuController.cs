using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenuController : AbstractScript
{
	ArrayList menuItems;//all of the menu items
	private int selectedItem;//the current menu item that is highlighted
	public static bool canMove;


	// Use this for initialization
	void Start ()
	{
		menuItems = new ArrayList();
		selectedItem = 0;
		canMove = false;
	}
	void Update()
	{
		//If there is need of a menu
		if (menuItems.Count > 0 && canMove)
		{
			((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;

			//go down
			if (Turns.getCurrentPlayer().getGamepad().isPressed("down") && Time.time - hub.LAST_TIME_DOWN >= 0.1f)
			{
				hub.LAST_TIME_DOWN = Time.time;
				if (selectedItem < menuItems.Count-1 && menuItems.Count > 1)
				{
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.white;
					selectedItem++;
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;
				}

			}

			//go up
			else if (Turns.getCurrentPlayer().getGamepad().isPressed("up") && Time.time - hub.LAST_TIME_UP >= 0.1f)
			{
				hub.LAST_TIME_UP = Time.time;
				if (selectedItem > 0)
				{
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.white;
					selectedItem--;
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;
				}
			}

			//pick the green item
			else if (Turns.getCurrentPlayer().getGamepad().isPressed("confirm"))
			{
				hub.LAST_TIME_CONFIRM = Time.time;
				passOption(((GameObject)menuItems[selectedItem]).transform.Find("Text").GetComponent<TextMesh>().text.ToString());

			}

			//cancel (return to moving the character)
			else if (Turns.getCurrentPlayer().getGamepad().isPressed("cancel") && Time.time - hub.LAST_TIME_CANCEL >= 0.5f)
			{
				hub.LAST_TIME_CANCEL = Time.time;

				//find a way to cancel and go back
				removeMenu();
				Cursor.cursorCanMove = true;
			}
		}
	}

	/*
	 * Funnels the option a player selected to a corresponding function
	 */ 
	public void passOption(string option)
	{
		switch(option)
		{
			case ("Attack"):
				beginAttacking();
				//hub.cursor.confirmFromMoveMenu();
				//removeMenu();
				break;
			case ("Speak"):
				beginSpeaking();
				break;
			case ("Summon"):
				//removeMenu();
				//move camera to the summon menu (also make a summon menu lol)
				beginSummoning();
				break;
			case ("Stop"):
				removeMenu();
				hub.CURSOR.confirmFromMoveMenu();
				break;
			default:
				print("This should not happpen. We got the option: " + option);
				break;
		}
	}



	//This function is called exclusively by external functions.
	//Options should be the strings you wish to display
	//position is where the top of the list should be
	public void MakeMoveMenu(ArrayList options, Vector3 position)
	{
		for(int i = 0; i < options.Count; i++)
		{
			GameObject mItem = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/MenuItem"));
			mItem.transform.Find("Text").GetComponent<TextMesh>().text = options[i].ToString();
			mItem.transform.position = new Vector3(position.x, position.y - (i * 2.5f), position.z);
			menuItems.Add(mItem);
			selectedItem = i;
		}
		canMove = true;
	}

	//clears out the menu. Should always be called when the menu is done with.
	public void removeMenu()
	{
		//destroy all spawned menu items
		for(int i = 0; i < menuItems.Count; i++)
		{
			Destroy((GameObject)menuItems[i]);
		}

		//just in case there is some left over data here
		menuItems.Clear();
	}

	/*
	 * This function should only be called if a player selected "Summon" as their option
	 * Should move the camera over to the summoning menu, and allow the controls to only operate that menu.
	 */ 
	public void beginSummoning()
	{
		canMove = false;
		SummonMenu.canMove = true;
		hub.CAMERA_CONTROLLER.toggleChildren();
		hub.CAMERA_CONTROLLER.goToSummonMenu();
	}

	public void beginAttacking()
	{
		//create enemy tiles
		hub.MakeTiles("EnemyTile");
		//set cursor flag to know it needs to attack
		hub.CURSOR.attacking = true;
	    Cursor.cursorCanMove = true;
		removeMenu();
	}
	public void beginSpeaking()
	{
		for(int i = 0; i < hub.charsInRange.Count; i++)
		{
			if(((Character)hub.charsInRange[i]).name != "Summoner")
			{
				hub.CURSOR.selectedCharacter.speak((Character)hub.charsInRange[i]);
			}
		}
		Cursor.cursorCanMove = true;
		hub.CURSOR.confirmFromMoveMenu();
		removeMenu();
	}
}
