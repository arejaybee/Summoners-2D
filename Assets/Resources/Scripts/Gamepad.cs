using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the Gamepad constructor class which can be adjusted for keyboard or controller used
//note I used "c" instead of "enter"

public class Gamepad {

	public:
		Gamepad();
		Gamepad(string controller);
		~Gamepad;
		
		// pressed
		
		bool isPressed(string input, string control);
		
		// sets
		
		void set_Up(string input);
		void set_Down(string input);
		void set_Left(string input);
		void set_Right(string input);
		void set_Confirm(string input);
		void set_Cancel(string input);
		void set_Start(string input);
		
		//gets
		
		string get_Up();
		string get_Down();
		string get_Left();
		string get_Right();
		string get_Confirm();
		string get_Cancel();
		string get_Start();
		
		
	private:
		string up;
		string down;
		string left;
		string right;
		string confirm;
		string cancel;
		string start;
};

// default class structure

Gamepad::Gamepad(){
	up = "up";
	down = "down";
	left = "left";
	right = "right";
	confirm = "z";
	cancel = "x";
	start = "c";
}

// empty class

Gamepad::Gamepad(string controller){
	
}

// Press functions below returns true if a valid input was given.
// note: I used "c" instead of enter for start

bool Gamepad::isPressed(string input){
	if (input == "up")
		return true;
	if (input == "down")
		return true;
	if (input == "left")
		return true;
	if (input == "right")
		return true;
	if (input == "z")
		return true;
	if (input == "x")
		return true;
	if (input == "c")
		return true;
}

// Below are the setters for the class variables

void Gamepad::set_Up(string input){
	up = input;
}

void Gamepad::set_Down(string input){
	down = input;
}

void Gamepad::set_Left(string input){
	left = input;
}

void Gamepad::set_Right(string input){
	right = input;
}

void Gamepad::set_Confirm(string input){
	 confirm= input;
}

void Gamepad::set_Cancel(string input){
	cancel = input;
}

void Gamepad::set_Start(string input){
	start = input;
}

// Below are the getters for the class variables

string Gamepad::get_Up(){
	return up;
}

string Gamepad::get_Down(){
	return down;
}

string Gamepad::get_Left(){
	return left;
}

string Gamepad::get_Right(){
	return right;
}

string Gamepad::get_Confirm(){
	return confirm;
}

string Gamepad::get_Cancel(){
	return cancel;
}

string Gamepad::get_Start(){
	return start;
}

