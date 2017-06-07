using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour {

	public bool isSelected;
	// Use this for initialization
	void Start ()
	{
		isSelected = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(isSelected)
		{
			GetComponent<SpriteRenderer>().color = Color.green;
		}
		else
		{
			GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
