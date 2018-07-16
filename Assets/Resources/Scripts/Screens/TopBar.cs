using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour
{
	public Character selectedChar;
	private GameObject topBar;
	// Use this for initialization
	void Awake ()
	{
		topBar = GameObject.Find("TopBar"); //cannot get the gameobject by using "this" for some reason...
	}

	private void Update()
	{
		if (selectedChar != null)
		{
			FillTopInfo();
		}
	}

	public void setTopBarActive(bool flag, Character c)
	{
		if (c == null)
		{
			flag = false;
		}
		topBar.SetActive(flag);
		selectedChar = c;
		if (flag)
		{
			FillTopInfo();
		}
	}

	//display information about the character that the cursor is on.
	void FillTopInfo()
	{
		GameObject hp = topBar.transform.Find("HP").gameObject;
		GameObject hPBar = topBar.transform.Find("HPBar").gameObject;
		GameObject name = topBar.transform.Find("Name").gameObject;
		GameObject stats = topBar.transform.Find("Stats").gameObject;
		GameObject playerNum = topBar.transform.Find("PlayerNum").gameObject;
		GameObject description = topBar.transform.Find("Description").gameObject;
		GameObject icon = topBar.transform.Find("Icon").gameObject;

		topBar.GetComponent<SpriteRenderer>().color = selectedChar.getPlayerColor();
		//set HP
		hp.GetComponent<TextMesh>().text = "HP: " + selectedChar.hp.ToString() + "/" + selectedChar.maxHp.ToString();
		//name
		name.GetComponent<TextMesh>().text = selectedChar.name;
		//stats
		stats.GetComponent<TextMesh>().text = "   ATK: " + selectedChar.attk + "\tRNG: " + selectedChar.attkRange + "\n   DEF: " + selectedChar.defense + "\tMOV: " + selectedChar.move + "\n\t\tZEAL: " + selectedChar.zeal;
		//playerNumber
		playerNum.GetComponent<TextMesh>().text = "Player " + selectedChar.playerNumber;
		//description
		description.GetComponent<TextMesh>().text = selectedChar.topBarDescription;

		//icon
		icon.GetComponent<SpriteRenderer>().sprite = Resources.Load<UnityEngine.Sprite>(selectedChar.iconPath);

		//math for HPbar
		//So as far as I can tell, -5 should be a constant as the length/position of the health bar. This may change if I can find out where the number really comes from.
		//if you're not me and this is interesting/you want to know why I used -5 to start with, ask me. Lets start a dialogue!
		hPBar.transform.localPosition = new Vector3(3 * (selectedChar.hp / selectedChar.maxHp)-4, 0.5f, 1);
	}
}
