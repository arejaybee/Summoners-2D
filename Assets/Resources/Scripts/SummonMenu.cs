using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SummonMenu : AbstractScript
{
	public Character selectedChar;
	public Object[] characterObjects;
	public Character[] summonableCharacters;
	public List<SummonOption> summonOptions;
	//this will make things easier for the player
	public List<SummonOption> canAfford;
	public List<SummonOption> cannotAfford;
	public bool canMove;

	private int index;
	private int startList;
	private int endList;
	// Use this for initialization
	void Start ()
	{
		canMove = false;
		characterObjects = Resources.LoadAll("Prefab/Characters/Units");

		summonableCharacters = new Character[characterObjects.Length];//set the character array to the same size as the number of objects
		for(int i = 0; i < characterObjects.Length; i++)
		{
			GameObject obj = GameObject.Instantiate((GameObject)characterObjects[i]);
			summonableCharacters[i] = obj.GetComponent<Character>();
			summonableCharacters[i].runStart();//to get values specified in the "start" function
			GameObject.Destroy(obj);
		}
		index = 0;
		startList = 0;
		endList = 5;
		summonOptions = new List<SummonOption>();
		for(int i = 0; i < summonableCharacters.Length; i++)
		{
			//summonOptions.Add(new SummonOption());
			summonOptions.Add(((GameObject)(GameObject.Instantiate(Resources.Load("Prefab/SummonMenu/SummonMenuOption")))).GetComponent<SummonOption>());
			summonOptions[i].c = summonableCharacters[i];// ((GameObject)characters[i]).GetComponent<Character>();
			summonOptions[i].Start();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canMove)//dont worry about any of this unless a player is on this menu
		{
			selectedChar = summonOptions[index].c;
			fillStatPortion();

			separateLists();//separate your list of characters into 2 lists
			sortByCost();//sort those two lists
			combineLists();//bring those lists back together
			getInput();//see if the player wanted to move up/down the list
		}
	}

	/*
	 * Take the stats from the selected character, and display them
	 */ 
	void fillStatPortion()
	{
		GameObject.Find("SelectedStats").GetComponent<TextMesh>().text = "  ATK: "+summonOptions[index].c.attk+"\t\tRNG: "+ summonOptions[index].c.attkRange+"\n  DEF: "+ summonOptions[index].c .defense+ "\t\tMOV:"+ summonOptions[index].c.move+ "\n\t\t\tZEAL: "+summonOptions[index].c.zeal+ "\n"+summonOptions[index].c.description+"\n\t\t\tCost: "+ summonOptions[index].c.cost+ "\n\t\t\tMana: "+Turns.getCurrentSummoner().mana;
		GameObject.Find("SelectedName").GetComponent<TextMesh>().text = selectedChar.name;
		GameObject.Find("StatDisplay").transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>(selectedChar.iconPath);
	}

	/*
	 * I feel like reading this list of summonable things will be easier if they are set up so you see the things you CAN summon first, and then you see the things
	 * that you cannot afford. Each of these lists will also be alphabetized!
	 */ 
	void separateLists()
	{
		canAfford.Clear();
		cannotAfford.Clear();
		for(int i = 0; i < summonOptions.Count; i++)
		{
			if(summonOptions[i].c.cost > Turns.getCurrentSummoner().mana)
			{
				summonOptions[i].GetComponent<SpriteRenderer>().color = Color.black;
				cannotAfford.Add(summonOptions[i]);
			}
			else
			{
				summonOptions[i].GetComponent<SpriteRenderer>().color = Color.white;
				canAfford.Add(summonOptions[i]);
			}
		}
	}

	//you take those two lists and put them back together. CanAfford is added first because you want to see those first.
	void combineLists()
	{
		//we add all of the options we can afford, first then the ones we cannot. Set them all to some far off location, then bring the ones that need to be on screen
		//onto the screen.
		summonOptions.Clear();
		for(int i = 0; i < canAfford.Count; i++)
		{
			summonOptions.Add(canAfford[i]);
		}
		for (int i = 0; i < cannotAfford.Count; i++)
		{
			summonOptions.Add(cannotAfford[i]);
		}
		for(int i = 0; i < summonOptions.Count; i++)
		{
			summonOptions[i].transform.position = new Vector3(-999, -999, -999);
		}

		//these 2 while loops will cycle the summon options as the index moves up and down
		while(index > endList-1)
		{
			startList += 1;
			endList += 1;
		}
		while(index  < startList)
		{
			startList -= 1;
			endList -= 1;
		}
		
		//NOW WE FINALLY MAKE THESE STUPID OPTION OBJECTS
		for(int i = startList; i < endList; i++)
		{
			//for now these are hard coded, because their positions are pretty much statically based on the position of this first one, which is pretty much statically
			//based on the position of the menu, WHICH Im pretty sure is just gonna stay where it is.
			summonOptions[i].transform.position = new Vector3(-513.4f,(6.6f - 3.84f*(i-startList)),-1f);
			summonOptions[i].transform.localScale = new Vector3(41.72f, 6, 1);
		}
	}

	//get some input from the user
	void getInput()
	{
		summonOptions[index].GetComponent<SpriteRenderer>().color = Color.green;
		//move down the list
		if(Turns.getCurrentPlayer().getGamepad().isPressed("down") && Time.time - hub.LAST_TIME_DOWN >= 0.1f)
		{
			hub.LAST_TIME_DOWN = Time.time;
			if(index+1 < summonOptions.Count)
			{
				summonOptions[index].GetComponent<SpriteRenderer>().color = Color.white;
				index++;
				summonOptions[index].GetComponent<SpriteRenderer>().color = Color.green;
			}
		}
		//move up the list
		else if(Turns.getCurrentPlayer().getGamepad().isPressed("up") && Time.time - hub.LAST_TIME_UP >= 0.1f)
		{
			hub.LAST_TIME_UP = Time.time;
			if(index-1 > -1)
			{
				summonOptions[index].GetComponent<SpriteRenderer>().color = Color.white;
				index--;
				summonOptions[index].GetComponent<SpriteRenderer>().color = Color.green;
			}
		}
		//select a character
		else if(Turns.getCurrentPlayer().getGamepad().isPressed("confirm"))
		{
			hub.LAST_TIME_CONFIRM = Time.time;
			summonOptions[index].GetComponent<SpriteRenderer>().color = Color.white;
			if (selectedChar.cost <= Turns.getCurrentSummoner().mana)
			{
				SummonCharacter(selectedChar);
			}
		}
		//go back to the map
		else if(Turns.getCurrentPlayer().getGamepad().isPressed("cancel"))
		{
			hub.LAST_TIME_CANCEL = Time.time;
			summonOptions[index].GetComponent<SpriteRenderer>().color = Color.white;
			canMove = false;
			hub.CAMERA_CONTROLLER.moveCamera(hub.CURSOR.transform.position);
			hub.CAMERA_CONTROLLER.toggleChildren();
			hub.MOVE_MENU_CONTROLLER.canMove = true;
		}
	}

	void SummonCharacter(Character c)
	{
		//set this menu's canMove flag.
		canMove = false;
		//set the other can move flags.
		hub.MOVE_MENU_CONTROLLER.canMove = false;

		//make an instance of this character
		GameObject createdCharacter = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Characters/Units/" + c.name));
		createdCharacter.GetComponent<Character>().CreateCharacter();
		Turns.getCurrentSummoner().mana -= c.cost;
		

		//make tiles showing where they can summon
		hub.MakeTiles("SummonTile");

		//put that character onto the cursor
		hub.CURSOR.MoveTo((Vector2)hub.summonPositions[0]);
		createdCharacter.transform.position = hub.CURSOR.transform.position;

		//tell the cursor to stop caring about the summoner (if it did)
		if (hub.CURSOR.selectedCharacter != null)
		{
			hub.CURSOR.selectedCharacter.canMove = false;
			hub.CURSOR.selectedCharacter = null;
		}
		//assign the cursor to the character
		hub.CURSOR.canSelect = false;
		hub.CURSOR.summoning = true;
		hub.CURSOR.selectedCharacter = createdCharacter.GetComponent<Character>();
		//let Confirm button place the character and delete the tiles(done in cursor)

		//move the camera
		hub.CAMERA_CONTROLLER.moveCamera(hub.CURSOR.transform.position);
		hub.CAMERA_CONTROLLER.toggleChildren(true);
		hub.CURSOR.confirmFromMoveMenu();
		hub.MOVE_MENU_CONTROLLER.removeMenu();
		

	}

	//sorts both the 'canAfford' and 'cannotAfford' lists in alpha order
	void sortByName()
	{
		canAfford = canAfford.OrderBy(g => g.c.name).ToList();
		cannotAfford = cannotAfford.OrderBy(g => g.c.name).ToList();
	}

	//Below this line, I hope to fill a bunch of sort options (currently only going to sort by name)
	void sortByCost()
	{
		canAfford = canAfford.OrderBy(g => g.c.cost).ToList();
		cannotAfford = cannotAfford.OrderBy(g => g.c.cost).ToList();
	}
}
