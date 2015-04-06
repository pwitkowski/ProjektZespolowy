using System;
using UnityEngine;
using System.Collections;

public class InterakcjaObiektyWskazniki : MonoBehaviour
{
	private GameObject player;

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag(DoneTags.player);
	}
		
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == player) {
			print ("Obiekt " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
			case "lasery":
				Gra.naprawa = 0;
				Gra.bateria = 0;
				break;
				
			case "LadowanieBaterii":
				if (Gra.bateria < 100) {
					Gra.bateria += (100 - Gra.bateria);
				}
				break;
			}
		}

	}

	private string colliderNazwaZawiera (string name)
	{
		if (name.Contains("fx_laserFence")) {
			return "lasery";
		}
		return name;
	}
}