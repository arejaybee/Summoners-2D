using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Cursor cursor;
	private int maxX,minX;
	private int maxY,minY;

	// Use this for initialization
	void Start ()
	{
		cursor = FindObjectOfType<Cursor>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		maxX = realRound(cursor.maxX / cursor.spacer) - 10;
		maxY = realRound(cursor.maxY / cursor.spacer) - 5;
		minX = 0+10;
		minY = 0 + 5;
		float tempX = cursor.transform.position.x;
		float tempY = cursor.transform.position.y;
		if(realRound(tempX / cursor.spacer) > maxX)
		{
			tempX = maxX * cursor.spacer;
		}
		else if (realRound(tempX / cursor.spacer) < minX)
		{
			tempX = minX * cursor.spacer;
		}

		if (realRound(tempY / cursor.spacer) > maxY)
		{
			tempY = maxY * cursor.spacer;
		}
		else if(realRound(tempY / cursor.spacer) < minY)
		{
			tempY = minY * cursor.spacer;
		}
		
		transform.position = new Vector3(tempX, tempY, transform.position.z);
	}

	int realRound(float f)
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
