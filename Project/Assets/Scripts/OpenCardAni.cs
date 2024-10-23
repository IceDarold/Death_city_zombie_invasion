using System;
using UnityEngine;

public class OpenCardAni : MonoBehaviour
{
	public void OpenCard()
	{
		this.curCard.OpenCard();
	}

	public CardItem curCard;
}
