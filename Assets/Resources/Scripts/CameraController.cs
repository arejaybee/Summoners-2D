using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Cursor cursor;
	private int maxX,minX;
	private int maxY,minY;
	private bool childFlag;
	// Use this for initialization
	void Start ()
	{
		cursor = FindObjectOfType<Cursor>();
		childFlag = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		
	}

	/*
	 * Used to move the camera.
	 * There are bounds on the edges of the map to keep the camera from showing things out of the map
	 */
	public void moveCamera(Vector3 pos)
	{
		GetComponent<Camera>().orthographicSize = 19;
		maxX = realRound(cursor.maxX / cursor.spacer) - 10;
		maxY = realRound(cursor.maxY / cursor.spacer) - 3;
		minX = 0 + 10;
		minY = 0 + 3;
		float tempX = pos.x;
		float tempY = pos.y;
		if (realRound(tempX / cursor.spacer) > maxX)
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
		else if (realRound(tempY / cursor.spacer) < minY)
		{
			tempY = minY * cursor.spacer;
		}
		transform.position = new Vector3(tempX, tempY, transform.position.z);

	}

	//Rounders numbers instead of truncating
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

	public void toggleChildren()
	{
		childFlag = !childFlag;
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(childFlag);
		}
	}
	public void toggleChildren(bool flag)
	{
		childFlag = flag;
		foreach (Transform child in transform)
		{
			child.gameObject.SetActive(childFlag);
		}
	}
	public void goToSummonMenu()
	{
		transform.position = new Vector3(-500, 0, transform.position.z);
		GetComponent<Camera>().orthographicSize = 15;
	}

}
