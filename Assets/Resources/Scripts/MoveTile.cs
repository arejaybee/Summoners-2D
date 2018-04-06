using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : AbstractScript
{
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(hub.findCharacterAt(transform.position) == hub.CURSOR.selectedCharacter)
		{
			GameObject.DestroyObject(this);
		}
	}
}
