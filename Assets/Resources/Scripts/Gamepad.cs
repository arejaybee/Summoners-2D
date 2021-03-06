using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Gamepad constructor class which can be adjusted for keyboard or controller used
//note I used "c" instead of "enter"

public class Gamepad{
	
	public string m_type;
	public int m_padNum;
	// generalized inputs so they can be defined for the specific gamepad
	//possibly add a controls menu to give preferences to controls?
	public KeyCode m_up;
	private KeyCode m_down;
	private KeyCode m_left;
	private KeyCode m_right;
	private KeyCode m_cancel;
	public KeyCode m_confirm;
	private KeyCode m_start;
	private KeyCode m_end;
	public float m_xAxis;
	public float m_yAxis;

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
		m_type = type;
		// keyboard initial key bindings.
		if (type == "keyboard") {
			m_up = KeyCode.UpArrow;
			m_down = KeyCode.DownArrow;
			m_left = KeyCode.LeftArrow;
			m_right = KeyCode.RightArrow;
			m_cancel = KeyCode.X;
			m_confirm = KeyCode.Z;
			m_start = KeyCode.Return;
			m_end = KeyCode.E;
		}
	}
	public Gamepad(string type, int padNum)
	{
		m_type = type;
		m_padNum = padNum;
		setControllerVars();
	}

	/*
	 * This function will be used to grab inputs for each player's controller.
	 * @button - the button input to check was pressed
	 * 
	 * return - true if the button was pressed, false otherwise.
	 */
	public bool isPressed(string button)
	{
		if (m_type != "keyboard") { 
			m_xAxis = Input.GetAxis("JoyStick" + m_padNum + "Horizontal");
			m_yAxis = Input.GetAxis("JoyStick" + m_padNum + "Vertical");
			//they didnt use the analog stick, check the dpad
			if (m_xAxis == 0) {
				m_xAxis = Input.GetAxis("JoyStick" + m_padNum + "DHorizontal");
			}
			if (m_yAxis == 0) {
				m_yAxis = -1 * Input.GetAxis("JoyStick" + m_padNum + "DVertical");
			}
		}
		//add checks for each of the possible buttons in each type (start with keyboard)
		//returns true if the assignment for the generalized input matches button and is being pressed.
		    if (button == "up" && (Input.GetKey(m_up) || m_yAxis < -0.5f))
				return true;
			else if (button == "down" && (Input.GetKey(m_down) || m_yAxis > 0.5f))
				return true;
			else if (button == "left" && (Input.GetKey(m_left) || m_xAxis < -0.5f))
				return true;
			else if (button == "right" && (Input.GetKey(m_right) || m_xAxis > 0.5f))
				return true;
			if (button == "confirm" && Input.GetKeyDown(m_confirm))
				return true;
			else if (button == "cancel" && Input.GetKeyDown(m_cancel))
				return true;
			else if (button == "start" && Input.GetKeyDown(m_start))
				return true;
			else if (button == "end" && Input.GetKeyDown(m_end))
				return true;
		// if no valid key is entered
		return false;
	}
	private void setControllerVars()
	{
		switch (m_padNum)
		{
			case 1:
				m_cancel = KeyCode.Joystick1Button1;
				m_confirm = KeyCode.Joystick1Button0;
				m_start = KeyCode.Joystick1Button8;
				m_end = KeyCode.Joystick1Button7;
				break;
			case 2:
				m_cancel = KeyCode.Joystick2Button1;
				m_confirm = KeyCode.Joystick2Button0;
				m_start = KeyCode.Joystick2Button8;
				m_end = KeyCode.Joystick2Button7;
				break;
			case 3:
				m_cancel = KeyCode.Joystick3Button1;
				m_confirm = KeyCode.Joystick3Button0;
				m_start = KeyCode.Joystick3Button8;
				m_end = KeyCode.Joystick3Button7;
				break;
			case 4:
				m_cancel = KeyCode.Joystick4Button1;
				m_confirm = KeyCode.Joystick4Button0;
				m_start = KeyCode.Joystick4Button8;
				m_end = KeyCode.Joystick4Button7;
				break;
			default:
				break;
		}
	}
}


