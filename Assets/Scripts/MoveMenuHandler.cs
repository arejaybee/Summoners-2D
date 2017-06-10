using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMenuHandler : MonoBehaviour
{
	ArrayList menuItems;//all of the menu items
	private int selectedItem;//the current menu item that is highlighted
	HUB hub;


	// Use this for initialization
	void Start ()
	{
		hub = GameObject.FindObjectOfType<HUB>();
		menuItems = new ArrayList();
		selectedItem = 0;
	}
	void Update()
	{
		if (menuItems.Count > 0)
		{
			((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;
			//go down
			if (Input.GetKey(KeyCode.DownArrow) && Time.time - hub.lastTimeDown >= 0.1f)
			{
				hub.lastTimeDown = Time.time;
				if (selectedItem < menuItems.Count-1 && menuItems.Count > 1)
				{
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.white;
					selectedItem++;
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;
				}

			}
			//go up
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				if (selectedItem > 0)
				{
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.white;
					selectedItem--;
					((GameObject)menuItems[selectedItem]).GetComponent<SpriteRenderer>().color = Color.green;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Z))
			{
				passOption(((GameObject)menuItems[selectedItem]).transform.FindChild("Text").GetComponent<TextMesh>().text.ToString());
				hub.cursor.confirmFromMoveMenu();

			}
			else if (Input.GetKeyDown(KeyCode.X) && Time.time - hub.lastTimeX >= 0.5f)
			{
				hub.lastTimeX = Time.time;
				//find a way to cancel and go back
				removeMenu();
				hub.cursor.cursorCanMove = true;
			}
		}
	}


	public void passOption(string option)
	{
		switch(option)
		{
			case ("Attack"):
				removeMenu();
				break;
			case ("Speak"):
				removeMenu();
				break;
			case ("Summon"):
				removeMenu();
				//move camera to the summon menu (also make a summon menu lol)
				break;
			case ("Stop"):
				removeMenu();
				hub.cursor.cursorCanMove = true; //give player control over their cursor again
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
			mItem.transform.FindChild("Text").GetComponent<TextMesh>().text = options[i].ToString();
			mItem.transform.position = new Vector3(position.x, position.y - (i * 2.5f), position.z);
			menuItems.Add(mItem);
			selectedItem = i;
		}
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
}
