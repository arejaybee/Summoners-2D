﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	private int m_playerNum;
	private bool m_turn;
	private Gamepad m_gamepad;
	public Player(int num)
	{
		m_playerNum = num;
		m_gamepad = new Gamepad();
	}

	//getters
	public bool isTurn()
	{
		return m_turn;
	}
	public int GetPlayerNum()
	{
		return m_playerNum;
	}
	//setters
	public void setTurn(bool turn)
	{
		m_turn = turn;
	}
}
