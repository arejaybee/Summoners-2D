using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
	private int m_playerNum;
	private bool m_turn;
	private ArrayList m_characters;
	private Summoner m_summoner;
	private Gamepad m_gamepad;
	private bool m_lost;
	public Player()
	{

	}
	public Player(int num)
	{
		m_playerNum = num;
		m_gamepad = new Gamepad("keyboard");
		m_characters = new ArrayList();
		m_lost = false;
	}
	public Player(int num, string controller)
	{
		m_playerNum = num;
		m_gamepad = new Gamepad("controller",num);
		m_characters = new ArrayList();
		m_lost = false;
	}

	public void addCharacter(Character c)
	{
		m_characters.Add(c);
	}

	//getters
	public bool isTurn()
	{
		return m_turn;
	}
	public bool hasLost()
	{
		return m_lost;
	}
	public int GetPlayerNum()
	{
		return m_playerNum;
	}
	public Gamepad getGamepad()
	{
		return m_gamepad;
	}
	public Summoner getSummoner()
	{
		return m_summoner;
	}
	public ArrayList getCharacters()
	{
		return m_characters;
	}

	//setters
	public void setTurn(bool turn)
	{
		m_turn = turn;
	}
	public void setGamePad(Gamepad g)
	{
		m_gamepad = g;
	}
	public void setSummoner(Summoner s)
	{
		m_summoner = s;
	}
	public void setLost(bool flag)
	{
		m_lost = flag;
	}
}
