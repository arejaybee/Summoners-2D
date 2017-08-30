using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
	HUB hub;
	// Use this for initialization
	void Start ()
	{
		hub = FindObjectOfType<HUB>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(hub.findCharacterAt(transform.position) == hub.cursor.selectedCharacter)
		{
			GameObject.DestroyObject(this);
		}
	}
}
