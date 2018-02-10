using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Gamepad constructor class which can be adjusted for keyboard or controller used
//note I used "c" instead of "enter"

public class Gamepad{
	
	private string m_type;
	// generalized inputs so they can be defined for the specific gamepad
	//possibly add a controls menu to give preferences to controls?
	private string m_up;
	private string m_down;
	private string m_left;
	private string m_right;
	private string m_cancel;
	private string m_confirm;
	private string m_start;
	private string m_end;


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
		// keyboard initial key bindings.
		if (type == "keyboard") {
			m_up = "up";
			m_down = "down";
			m_left = "left";
			m_right = "right";
			m_cancel = "x";
			m_confirm = "z";
			m_start = "enter";
			m_end = "e";
		
		}

		// controller use d-pad or analogs?

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
		//returns true if the assignment for the generalized input matches button and is being pressed.
		if (button == "up" && Input.GetKeyDown (m_up))
			return true;
		if (button == "down" && Input.GetKeyDown (m_down))
			return true;
		if (button == "left" && Input.GetKeyDown (m_left))
			return true;
		if (button == "right" && Input.GetKeyDown (m_right))
			return true;
		if (button == "confirm" && Input.GetKeyDown (m_confirm))
			return true;
		if (button == "cancel" && Input.GetKeyDown (m_cancel))
			return true;
		if (button == "start" && Input.GetKeyDown (m_start))
			return true;
		if (button == "end" && Input.GetKeyDown (m_end))
			return true;
		// if no valid key is entered
		return false;
	}
}


