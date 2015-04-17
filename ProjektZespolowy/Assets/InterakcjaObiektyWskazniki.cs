using System;
using UnityEngine;
using System.Collections;

public class InterakcjaObiektyWskazniki : MonoBehaviour
{
	//private GameObject player;
	private GameObject cialoRobota;
	private GameObject poleWidzenia;

	void Awake ()
	{
		//player = GameObject.FindGameObjectWithTag(DoneTags.player);
		cialoRobota =  GameObject.FindGameObjectWithTag(DoneTags.cialoRobota);
		poleWidzenia = GameObject.FindGameObjectWithTag(DoneTags.poleWidzenia);
	}
		
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == cialoRobota) {
			print ("Ciało robota dotyka: " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
				case "lasery":
					Gra.naprawa -= 50;
					Gra.bateria -= 10;
					//TODO dodać do navmesh jako przeszkoda
					break;
				case "LadowanieBaterii":
					if (Gra.bateria < 100) {
						Gra.bateria += (100 - Gra.bateria);
						Destroy(gameObject);
					}
					break;
			case "punkt":
				//zapisuje punkt jako odwiedzony
				if(!Gra.listaPunktowOdwiedzonych.Contains(collider.name)){
					print("Odwiedziłem już: "+collider.name);
					Gra.listaPunktowOdwiedzonych.Add(collider.name);
				}
				break;
			}
		}

		if (other.gameObject == poleWidzenia) {
			print ("W polu widzenia jest: " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
				case "punkt":
					//dodaje waypointa do listy punktow
					if(!Gra.tablicaPunktow.ContainsKey(collider.name)){
						print("Dodaje do listy punktów: "+collider.name);
						Gra.tablicaPunktow.Add(collider.name, collider.transform.position);
					}
					break;
			}
		}
	}

	private string colliderNazwaZawiera (string name)
	{
		if (name.Contains("fx_laserFence")) return "lasery"; 
		if (name.Contains("wayPoint")) return "punkt"; 
		return name;
	}
}