using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//These are only here to be used.
//The move cost and names are assigned as these get built.
public class MapTile : MonoBehaviour
{
	public int moveCost;
	public string theName;
	public MapTile northTile = null;
	public MapTile southTile = null;
	public MapTile eastTile = null;
	public MapTile westTile = null;
	private Character characterOnTile = null;
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public MapTile getNorthTile()
	{
		return northTile;
	}
	public void setNorthTile(MapTile mt)
	{
		northTile = mt;
	}

	public MapTile getSouthTile()
	{
		return southTile;
	}
	public void setSouthTile(MapTile mt)
	{
		southTile = mt;
	}

	public MapTile getEastTile()
	{
		return eastTile;
	}
	public void setEastTile(MapTile mt)
	{
		eastTile = mt;
	}

	public MapTile getWestTile()
	{
		return westTile;
	}
	public void setWestTile(MapTile mt)
	{
		westTile = mt;
	}

	public Character getCharacterOnTile()
	{
		return characterOnTile;
	}
	public void setCharacterOnTile(Character c)
	{
		characterOnTile = c; 
	}



}
