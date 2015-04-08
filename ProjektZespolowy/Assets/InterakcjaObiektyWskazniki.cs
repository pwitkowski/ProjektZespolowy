using System;
using UnityEngine;
using System.Collections;

public class InterakcjaObiektyWskazniki : MonoBehaviour
{
	private GameObject player;
	private GameObject cialoRobota;

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag(DoneTags.player);
		cialoRobota =  GameObject.FindGameObjectWithTag(DoneTags.cialoRobota);
	}
		
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == cialoRobota) {
			print ("Obiekt " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
			case "lasery":
				Gra.naprawa = 0;
				Gra.bateria = 0;
				break;
				
			case "LadowanieBaterii":
				if (Gra.bateria < 100) {
					Gra.bateria += (100 - Gra.bateria);
					Destroy(gameObject);
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