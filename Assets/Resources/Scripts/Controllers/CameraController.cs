using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : AbstractScript {
	private int maxX,minX;
	private int maxY,minY;
	private bool childFlag;

	// Use this for initialization
	void Start ()
	{
		childFlag = true;
	}

	/*
	 * Used to move the camera.
	 * There are bounds on the edges of the map to keep the camera from showing things out of the map
	 * "pos" here is in World space
	 */
	public void moveCamera(Vector3 pos)
	{
		GetComponent<Camera>().orthographicSize = 19; //idk why this was hardcoded, but it can never be changed ;-;
		maxX = realRound(hub.MAX_X / HUB.SPACER) - 10;
		maxY = realRound(hub.MAX_Y / HUB.SPACER) - 3;
		minX = 0 + 10;
		minY = 0 + 3;
		float tempX = pos.x;
		float tempY = pos.y;
		if (realRound(tempX / HUB.SPACER) > maxX)
		{
			tempX = maxX * HUB.SPACER;
		}
		else if (realRound(tempX / HUB.SPACER) < minX)
		{
			tempX = minX * HUB.SPACER;
		}

		if (realRound(tempY / HUB.SPACER) > maxY)
		{
			tempY = maxY * HUB.SPACER;
		}
		else if (realRound(tempY / HUB.SPACER) < minY)
		{
			tempY = minY * HUB.SPACER;
		}
		transform.position = new Vector3(tempX, tempY, transform.position.z);

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
