using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	private float sizeX;
	private float sizeY;
	private Character selectedCharacter;
	private bool select;//if this is true, we can pick up a unit
	// Use this for initialization
	void Start ()
	{
		sizeX = transform.localScale.x;
		sizeY = transform.localScale.y;

		//we do not start with a character started
		selectedCharacter = null;
		select = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		DetectMovement();
		MouseMovement();
		MobileTouchMovement();
		if(selectedCharacter != null)
		{
			selectedCharacter.gameObject.transform.position = transform.position;
		}
	}

	//used to move the cursor
	void DetectMovement()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.position += new Vector3(0, sizeY / 1.5f, 0);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			transform.position -= new Vector3(0, sizeY / 1.5f, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.position += new Vector3(sizeX / 1.5f, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position -= new Vector3(sizeX / 1.5f, 0, 0);
		}

		if(Input.GetKeyDown(KeyCode.Z))
		{
			if (select)
			{
				DetectSelect();
				select = false;
			}
			else
			{
				selectedCharacter = null;
				select = true;
			}
		}
	}

	/*
	 * Detects a click of the mouse
	 * Sends the cursor to the square closest to wherever was clicked
	 */ 
	void MouseMovement()
	{
		if (Input.GetMouseButtonDown(0))
		{
			print("Button down");
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//clicking gives screen coordinates, so just convert that to world space
			Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePosition);

			//see next function header for why we do this 
			transform.position = RoundPosition(clickPos);
			if (selectedCharacter != null)
			{
				selectedCharacter.gameObject.transform.position = transform.position;
			}
			if (select)
			{
				DetectSelect();
				select = false;
			}
			else
			{
				selectedCharacter = null;
				select = true;
			}
		}
	}
	
	/*
	 * Just incase this game DOES get adapted to mobile, 
	 * This should mimic the mouse movement function, but with a touch request
	 */ 
	void MobileTouchMovement()
	{
		try
		{
			Vector2 fingerPos = Input.GetTouch(0).position;
			transform.position = RoundPosition(fingerPos);
		}
		catch(Exception e)
		{
			e.ToString();
			//GetTouch seems to be out of bounds everytime we arent clicking,
			//This isnt needed, but it makes the false error go away
		}
	}

	/*
	 * This function will see if there is a character to select
	 * It is implied that when you get to this call you have tried selecting something
	 * */
	void DetectSelect()
	{
		Character[] chars = FindObjectsOfType<Character>();
		for(int i = 0; i < chars.Length; i++)
		{
			if(RoundPosition(chars[i].gameObject.transform.position) == RoundPosition(transform.position))
			{
				print("Char found!");
				selectedCharacter = chars[i];
			}
		}
	}







	/*
	 * When clicking to move, the position doesnt necessarily stay grid like
	 * This function converts a clicked location into a grid location.
	 */
	Vector3 RoundPosition(Vector3 pos)
	{
		float xPos = 0;
		float yPos = 0;
		//find the first position that is greater than the one clicked
		while(xPos < Mathf.Abs(pos.x))
		{
			xPos += sizeX / 1.5f;
		}
		while (yPos < Mathf.Abs(pos.y))
		{
			yPos += sizeY / 1.5f;
		}
		//look at the last position for both
		float lxPos = xPos - sizeX / 1.5f;
		float lyPos = yPos - sizeY / 1.5f;

		//set xPos to the closer position to wherever they clicked
		if (Mathf.Abs(lxPos - Mathf.Abs(pos.x)) < Mathf.Abs(xPos - Mathf.Abs(pos.x)))
		{
			xPos = lxPos;
		}
		//same for y
		if (Mathf.Abs(lyPos - Mathf.Abs(pos.y)) < Mathf.Abs(yPos - Mathf.Abs(pos.y)))
		{
			yPos = lyPos;
		}

		//the algorithm I used here only really works for finding the best positive numbers,
		//so I just made everything positive, then multiply by -1 if it was negative (y)
		if(pos.x < 0)
		{
			xPos *= -1;
		}
		if(pos.y < 0)
		{
			yPos *= -1;
		}
		Vector2 newPos = new Vector2(xPos, yPos);

		return newPos;
	}
}
