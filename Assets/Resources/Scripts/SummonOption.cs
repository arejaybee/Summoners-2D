using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOption : MonoBehaviour
{
	public Character c;
	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (c != null)
		{
			transform.FindChild("Text").GetComponent<TextMesh>().text = c.name;
		}
	}

	public int CompareTo(SummonOption b)
	{
		if(b == null)
		{
			return 1;
		}
		return this.c.CompareTo(b.c);
	}
}
