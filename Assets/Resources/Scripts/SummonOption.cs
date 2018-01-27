using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonOption : MonoBehaviour
{
	public Character c;

	// Use this for initialization
	public void Start ()
	{
		if (c != null)
		{
			transform.Find("Text").GetComponent<TextMesh>().text = c.name;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
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
