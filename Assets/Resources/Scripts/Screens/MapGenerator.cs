﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : AbstractScript
{
	public string[,] map;
	public MapTile[,] newMapTiles;
	public int boundsX;
	public int boundsY;
	private ArrayList summoners;
	private GameObject sum1;
	private GameObject sum2;
	// Use this for initialization
	void Start ()
	{
		sum1 = hub.players[0].getSummoner().gameObject;
		sum2 = hub.players[1].getSummoner().gameObject;

		//Eventually, I want to be able to pass in a string and have that string be built. So that I can have several pre-built maps that are set to this variable.
		string tempMap = "MMMGGGGGGGGGGGGGGGGGG MMMMGGGGGGGGGGGGGGGGG MMMMMGGGGGGGGGGGGGGGG MMMMGGGGGGGGGGGGGGGGG MMMGGGGGGGGGGGGGGGGGG GGGGGGGGGGGGGGGGGGGGG GGGGGGGWWGGGGGGGGGGGG GGGGGGGGWWGGGGGGGGGGG GGGGGGGGGWWGGGGGGGGGG GGGGGGGGGGWWWWGGGGGGG GGGGGGGGGWWWWWGGGGGGG GGGGGGGGGGGGGWGGGGGGG GGGGGGGGGGGGGGWGGGGGG GGGGGGGGGGGGGGGGGGGGG GGGGGGGGGGGGGGGGGGGGG GGGGGGGGGGGGGGGGGGGGG GGGGGGGGGGGGGGGGGGGGG GGGGGGGGGGWWWWWWGGGGG GGGGGGGGGGWWWWWWGGGGG GGGGGGGGGGWWWWWWGGGGG";
		string[] n = tempMap.Split(" "[0]);
		boundsY = n.Length;
		boundsX = n[0].Length;
		MakeMap(n);
		SpawnItems(n);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//builds a 2d grid based on the string "tempMap" written above
	void MakeMap(string[] n)
	{
		map = new string[n.Length, n[0].Length];
		newMapTiles = new MapTile[n.Length, n[0].Length];
		for(int i = 0; i < n.Length; i++)
		{
			for(int j = 0; j < n[i].Length; j++)
			{
				map[i, j] = n[i].Substring(j, 1);
				//print("I: " + i + " J:" + j);
			}
		}
	}

	//I have no clue why I went with "Items" here..
	//So here, I am spawning mapTiles based on the string given above.
	//It then places the cursor in the center, and  makes 2 summoners (I may make the 2 summoners thing a part of the map text)
	void SpawnItems(string[] n)
	{
		for (int i = 0; i < n.Length; i++)
		{
			for (int j = 0; j < n[0].Length; j++)
			{
				//make map tiles
				GameObject m = null;
				if (map[i, j] == "M")
				{
					m = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/Mountain"));
					m.transform.position = new Vector3(HUB.SPACER * j, HUB.SPACER * i, 0);
				}
				else if (map[i, j] == "G")
				{
					 m = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/Grass"));
					m.transform.position = new Vector3(HUB.SPACER * j, HUB.SPACER * i, 0);
				}
				else if (map[i, j] == "W")
				{
					 m = (GameObject)Instantiate(Resources.Load("Prefab/Tiles/Water"));
					m.transform.position = new Vector3(HUB.SPACER * j, HUB.SPACER * i, 0);
				}
				newMapTiles[i, j] = m.GetComponent<MapTile>();
			}
		}
		//add summoners
		sum1.transform.position = new Vector3(0, HUB.SPACER * (int)(n.Length / 2), 0);
		hub.CURSOR.MoveTo(worldToGrid(sum1.transform.position));
		sum2.transform.position = new Vector3(HUB.SPACER * (int)(n[0].Length - 1), HUB.SPACER * (int)(n.Length / 2), 0);
	}
}
