using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	private float sizeX;
	private float sizeY;
	// Use this for initialization
	void Start ()
	{
		sizeX = transform.localScale.x;
		print(sizeX);
		sizeY = transform.localScale.y;
	}
	
	// Update is called once per frame
	void Update ()
	{
		DetectMovement();
		MouseMovement();
		MobileTouchMovement();
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
	}

	/*
	 * Detects a click of the mouse
	 * Sends the cursor to the square closest to wherever was clicked
	 */ 
	void MouseMovement()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			//clicking gives screen coordinates, so just convert that to world space
			Vector3 clickPos = Camera.main.ScreenToWorldPoint(mousePosition);

			//see next function header for why we do this 
			transform.position = RoundPosition(clickPos);
		}
	}
	
	/*
	 * Just incase this game DOES get adapted to mobile, 
	 * This should mimic the mouse movement function, but with a touch request
	 */ 
	void MobileTouchMovement()
	{
		Vector2 fingerPos = Input.GetTouch(0).position;
		transform.position = RoundPosition(fingerPos);
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
