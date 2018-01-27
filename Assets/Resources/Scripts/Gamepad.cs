using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Gamepad constructor class which can be adjusted for keyboard or controller used
//note I used "c" instead of "enter"

public class Gamepad{
	
	private string m_type;

	//default constructor
	public Gamepad()
	{
		
	}
	/*
	 * Real constructor
	 * @type - the type of controller to make ("keyboard" or "controller")
	 */
	public Gamepad(string type)
	{

	}

	/*
	 * This function will be used to grab inputs for each player's controller.
	 * @button - the button input to check was pressed
	 * 
	 * return - true if the button was pressed, false otherwise.
	 */ 
	public bool isPressed(string button)
	{
		//add checks for each of the possible buttons in each type (start with keyboard)
		return false;
	}
}

