using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractScript : MonoBehaviour {
	public HUB hub;
	void Awake()
	{
		hub = FindObjectOfType<HUB>();
	}


	public Player getPlayer(int num)
	{
		return hub.players[num - 1];
	}

	//change grid coordinates to world coordinates
	protected Vector2 gridToWorld(Vector2 gridCoord)
	{
		return roundPosition(new Vector2(gridCoord.x * HUB.SPACER, gridCoord.y * HUB.SPACER));
	}

	//change world coordinates to grid coordinates
	protected Vector2 worldToGrid(Vector2 worldCoord)
	{
		int x = realRound(worldCoord.x / HUB.SPACER);
		int y = realRound(worldCoord.y / HUB.SPACER);
		return new Vector2(x, y);
	}

	/*
	 * When clicking to move, the position doesnt necessarily stay grid like
	 * This function converts a clicked location into a grid location.
	 * The function is currently being used to convert gridspace coordinates to world space.
	 * "pos" here is in world space
	 */
	protected Vector2 roundPosition(Vector2 pos)
	{
		float xPos = 0;
		float yPos = 0;

		//find the first position that is greater than the one clicked
		while (xPos < Mathf.Abs(pos.x))
		{
			xPos += HUB.SPACER;
		}
		while (yPos < Mathf.Abs(pos.y))
		{
			yPos += HUB.SPACER;
		}
		//look at the last position for both
		float lxPos = xPos - HUB.SPACER;
		float lyPos = yPos - HUB.SPACER;

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
		if (pos.x < 0)
		{
			xPos *= -1;
		}
		if (pos.y < 0)
		{
			yPos *= -1;
		}
		Vector2 newPos = new Vector2(xPos, yPos);

		return newPos;
	}


	protected int realRound(float f)
	{
		float tempF = f;
		tempF -= (int)f;
		tempF *= 10;
		if ((int)tempF > 4)
		{
			return (int)f + 1;
		}
		return (int)f;
	}
}
