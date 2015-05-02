using System;
using UnityEngine;
using System.Collections;

public class InterakcjaObiektyWskazniki : MonoBehaviour
{
	private GameObject cialoRobota;
	private GameObject poleWidzenia;
	private float uszkodzeniaBaterii = 10f;
	private float uszkodzeniaNaprawy = 50f;
	private float doladowanieCzasu = 200f;

	//potrzebne do zapisywania wskaźników przed i po podjęciu decyzji
	private Wskazniki wskaznikiPrzed;
	//private Wskazniki wskaznikiPo = new Wskazniki();

	void Awake ()
	{
		cialoRobota =  GameObject.FindGameObjectWithTag(DoneTags.cialoRobota);
		poleWidzenia = GameObject.FindGameObjectWithTag(DoneTags.poleWidzenia);
	}
		
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject == cialoRobota) {
			//print ("Ciało robota dotyka: " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
				case "lasery":
					wskaznikiPrzed = Gra.wskazniki;
					Gra.wskazniki.naprawa -= uszkodzeniaNaprawy;
					Gra.wskazniki.bateria -= uszkodzeniaBaterii;
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Bateria: -" + uszkodzeniaBaterii + ", Naprawa -" + uszkodzeniaNaprawy);
					//TODO dodać do navmesh jako przeszkoda
					break;
				case "bateria":
					if (Gra.wskazniki.bateria < 100) {
						float doladowanie  =(100 - Gra.wskazniki.bateria);
						wskaznikiPrzed = Gra.wskazniki;
						//print ("wskaznikiPrzed= "+wskaznikiPrzed.ToString());
						Gra.wskazniki.bateria += doladowanie;

						dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

						Gra.WyswietlKomunikatWChmurze("Bateria: +" + doladowanie);
						gameObject.active = false;

						//zdejmuje z kolejki podładowanie baterii
						Gra.kolejkaPriorytetowa.Dequeue();
					}
					break;
				case "bomba":
					wskaznikiPrzed = Gra.wskazniki;
 					Gra.wskazniki.naprawa -= (uszkodzeniaNaprawy + 40);
					Gra.wskazniki.bateria -= (uszkodzeniaBaterii + 40);
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Bateria: -" + uszkodzeniaBaterii + ", Naprawa -" + uszkodzeniaNaprawy);
					gameObject.active = false;
					break;
				case "zegar":
					wskaznikiPrzed = Gra.wskazniki;
					Gra.wskazniki.czas += doladowanieCzasu;

					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Czas +" + doladowanieCzasu);
					gameObject.active = false;

					//zdejmuje z kolejki zdobycie troche czasu
					Gra.kolejkaPriorytetowa.Dequeue();
					break;
				case "apteczka":
					if (Gra.wskazniki.naprawa < 100) {
						float doladowanieNaprawy  =(100 - Gra.wskazniki.naprawa);
						wskaznikiPrzed = Gra.wskazniki;
						Gra.wskazniki.naprawa += doladowanieNaprawy;
						
						dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

						Gra.WyswietlKomunikatWChmurze("Naprawa: +" + doladowanieNaprawy);
						gameObject.active = false;

						//zdejmuje z kolejki podreperowanie się
						Gra.kolejkaPriorytetowa.Dequeue();
					}			
					break;
				case "punkt":
					//zapisuje punkt jako odwiedzony
					if(!Gra.listaPunktowOdwiedzonych.Contains(collider.name)){
						//print("Odwiedziłem już: "+collider.name);
						Gra.listaPunktowOdwiedzonych.Add(collider.name);
					}
					break;
				case "kluczDoWindy":
					wskaznikiPrzed = Gra.wskazniki;
					Gra.wskazniki.kluczDoWindy = true;
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);
					
					Gra.WyswietlKomunikatWChmurze("Znalazlem klucz do windy");
					gameObject.active = false;
					break;
			}
		}

		if (other.gameObject == poleWidzenia) {
			//print ("W polu widzenia jest: " + collider.name+ " tag: "+collider.gameObject.tag);
			switch (colliderNazwaZawiera (collider.name)) {
				case "punkt":
					//dodaje waypointa do listy punktow
					if(!Gra.tablicaPunktow.ContainsKey(collider.name)){
						//print("Dodaje do listy punktów: "+collider.name);
						Gra.tablicaPunktow.Add(collider.name, collider.transform.position);
					}
					break;
			}

			if(collider.gameObject.CompareTag(DoneTags.artefakt)){
				//dodaje do listy wszystkie obiekty z tagiem artefakt
				dodajPozycjeArtefaktuDoListy(collider.transform.position);
			}
		}
	}

	private void dodajWspomnieniePoUzyciuArtefaktuDoTablicy(Collider collider, Wskazniki wskaznikiPrzedParam){
		//dodaje rekordy do tablicaArtefaktBol jak zmieniły się wskaźniki po użyciu danego artefaktu
		//print ("wskaznikiPrzed= " + wskaznikiPrzedParam.ToString()); //TODO wtf czemu wartosci sa inne ???
		//print ("wskaznikiPo= " + Gra.wskazniki.ToString());

		Wskazniki wskaznikiBulu = new Wskazniki();
		wskaznikiBulu.czas = Gra.wskazniki.czas - wskaznikiPrzedParam.czas;
		wskaznikiBulu.bateria = Gra.wskazniki.bateria - wskaznikiPrzedParam.bateria;
		wskaznikiBulu.naprawa = Gra.wskazniki.naprawa - wskaznikiPrzedParam.naprawa;

		//print ("Wskazniki bulu: " + wskaznikiBulu.ToString ());
		//TODO sprawdzić czy wskaźniki przeliczają sie dobrze

		if (!Gra.tablicaArtefaktBol.ContainsKey (collider.name)) {
			//print("Dodaje do tablicy artefatk ból jak zmieniły się wskaźniki po użyciu artefaktu: "+collider.name+" wskaźniki: "+wskaznikiBulu.ToString());
			Gra.tablicaArtefaktBol.Add(collider.name, wskaznikiBulu);
		}

		usunPozycjeArtefaktuZListy (collider.transform.position);

	}

	private void dodajPozycjeArtefaktuDoListy(Vector3 punktArtefaktu){
		//dodaje nowy punkt nieznanego artefaktu do listy znalezionych artefaktów
		if(!Gra.listaPozycjiZnalezionychArtefaktow.Contains(punktArtefaktu)){
			//print("Dodaje nowy punkt do listy znalezionych artefaktów: "+punktArtefaktu);
			Gra.listaPozycjiZnalezionychArtefaktow.Add(collider.transform.position);
		}
	}

	private void usunPozycjeArtefaktuZListy(Vector3 punktArtefaktu){
		//usuwa punkt znanego artefaktu z listy znalezionych artefaktów
		if(!Gra.listaPozycjiZnalezionychArtefaktow.Contains(punktArtefaktu)){
			//print("Usuwam punkt z listy znalezionych artefaktów: "+punktArtefaktu);
			Gra.listaPozycjiZnalezionychArtefaktow.Remove(collider.transform.position);
		}
	}

	private string colliderNazwaZawiera (string name){
		if (name.Contains("fx_laserFence")) return "lasery"; 
		if (name.Contains("wayPoint")) return "punkt"; 
		return name;
	}
}