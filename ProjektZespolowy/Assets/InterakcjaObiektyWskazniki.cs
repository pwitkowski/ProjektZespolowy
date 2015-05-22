using System;
using UnityEngine;
using System.Collections;

public class InterakcjaObiektyWskazniki : MonoBehaviour
{
	private GameObject cialoRobota;
	private GameObject poleWidzenia;
	private NavMeshAgent nawAgent;
	private Animator playerAnim;
	private DoneHashIDs hash;
	private float uszkodzeniaBaterii = 10f;
	private float uszkodzeniaNaprawy = 30f;
	private float doladowanieCzasu = 200f;

	//potrzebne do zapisywania wskaźników przed i po podjęciu decyzji
	private Wskazniki wskaznikiPrzed = new Wskazniki();

	void Awake (){
		cialoRobota =  GameObject.FindGameObjectWithTag(DoneTags.cialoRobota);
		poleWidzenia = GameObject.FindGameObjectWithTag(DoneTags.poleWidzenia);
		nawAgent = GameObject.FindGameObjectWithTag(DoneTags.player).GetComponent<NavMeshAgent>();
		playerAnim = GameObject.FindGameObjectWithTag(DoneTags.player).GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
	}
		
	void OnTriggerEnter (Collider other){
		if (other.gameObject == cialoRobota) {
			//print ("Ciało robota dotyka: " + collider.name);
			switch (colliderNazwaZawiera (collider.name)) {
				case "lasery":
					wskaznikiPrzed = new Wskazniki(Gra.wskazniki);
					Gra.wskazniki.naprawa -= uszkodzeniaNaprawy;
					Gra.wskazniki.bateria -= uszkodzeniaBaterii;
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Bateria: -" + zaokraglij(uszkodzeniaBaterii) + ", Naprawa -" + zaokraglij(uszkodzeniaNaprawy));
					break;
				case "bateria":
					if (Gra.wskazniki.bateria < 100) {
						float doladowanie  =(100 - Gra.wskazniki.bateria);
						wskaznikiPrzed = new Wskazniki(Gra.wskazniki);
						//print ("wskaznikiPrzed= "+wskaznikiPrzed.ToString());
						Gra.wskazniki.bateria += doladowanie;

						dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

						Gra.WyswietlKomunikatWChmurze("Bateria: +" + zaokraglij(doladowanie));
						gameObject.active = false;
					}
					break;
				case "bomba":
					wskaznikiPrzed = new Wskazniki(Gra.wskazniki);
 					Gra.wskazniki.naprawa -= (uszkodzeniaNaprawy + 40);
					Gra.wskazniki.bateria -= (uszkodzeniaBaterii + 40);
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Bateria: -" + zaokraglij(uszkodzeniaBaterii) + ", Naprawa -" + zaokraglij(uszkodzeniaNaprawy));
					gameObject.active = false;
					break;
				case "zegar":
					wskaznikiPrzed = new Wskazniki(Gra.wskazniki);
					Gra.wskazniki.czas += doladowanieCzasu;

					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

					Gra.WyswietlKomunikatWChmurze("Czas +" + zaokraglij(doladowanieCzasu));
					gameObject.active = false;
					break;
				case "apteczka":
					if (Gra.wskazniki.naprawa < 100) {
						float doladowanieNaprawy  =(100 - Gra.wskazniki.naprawa);
						wskaznikiPrzed = new Wskazniki(Gra.wskazniki);;
						Gra.wskazniki.naprawa += doladowanieNaprawy;
						
						dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);

						Gra.WyswietlKomunikatWChmurze("Naprawa: +" + zaokraglij(doladowanieNaprawy));
						gameObject.active = false;
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
					wskaznikiPrzed = new Wskazniki(Gra.wskazniki);
					Gra.wskazniki.kluczDoWindy = true;
					
					dodajWspomnieniePoUzyciuArtefaktuDoTablicy(collider, wskaznikiPrzed);
					
					Gra.WyswietlKomunikatWChmurze("Znalazlem klucz do windy");
					Gra.czyZnalazlemKluczDoWindy = true;
					gameObject.active = false;
					break;
				case "door_exit_outer":
					if(!Gra.czyZnalazlemKluczDoWindy){
						Gra.WyswietlKomunikatWChmurze("Potrzebuje klucza do windy");
						
						//zatrzymuje robota
						nawAgent.Stop(true);
						playerAnim.SetFloat(hash.speedFloat,0f);

						Gra.czyPotrzebujeKluczaDoWindy = true;
						Gra.czyZnalazlemWyjscie = true;
					}
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
		Wskazniki wskaznikiBulu = new Wskazniki();
		wskaznikiBulu.czas = Gra.wskazniki.czas - wskaznikiPrzedParam.czas;
		wskaznikiBulu.bateria = Gra.wskazniki.bateria - wskaznikiPrzedParam.bateria;
		wskaznikiBulu.naprawa = Gra.wskazniki.naprawa - wskaznikiPrzedParam.naprawa;
		//jeśli w poprzednich wskaźnikach nie było klucza a teraz jest to ustawiam na true
		wskaznikiBulu.kluczDoWindy = Gra.wskazniki.kluczDoWindy && !wskaznikiPrzed.kluczDoWindy;

		print ("Wskazniki bulu dla : "+collider.name+" to:"+ wskaznikiBulu.ToString ());

		if (!Gra.tablicaArtefaktBol.ContainsKey (collider.name)) {
			print("Dodaje do tablicy artefatk ból jak zmieniły się wskaźniki po użyciu artefaktu: "+collider.name+" wskaźniki: "+wskaznikiBulu.ToString());
			Gra.tablicaArtefaktBol.Add(collider.name, wskaznikiBulu);
		}

		dodajDoTablicyPozycjiRozpoznanychArtefaktow (collider);
		usunPozycjeArtefaktuZListy (collider.transform.position);
	}

	void dodajDoTablicyPozycjiRozpoznanychArtefaktow (Collider collider){
		//dodaje nazwę artefaktu i pozycje do tablicy rozpoznanyh artefaktów
		if (collider != null && collider.name != null && collider.name.Length > 0) {
			if (!Gra.tablicaPozycjiRozpoznanychArtefaktow.Contains (collider.name)) {
				print ("Dodaje rozpoznany artefakt: " + collider.name + " na pozycji: " + collider.transform.position);
				Gra.tablicaPozycjiRozpoznanychArtefaktow.Add (collider.name, collider.transform.position);
			}
		}
	}

	private void dodajPozycjeArtefaktuDoListy(Vector3 punktArtefaktu){
		//dodaje nowy punkt nieznanego artefaktu do listy znalezionych artefaktów
		if(!Gra.listaPozycjiZnalezionychArtefaktow.Contains(punktArtefaktu)
		   	&& !Gra.tablicaPozycjiRozpoznanychArtefaktow.ContainsValue(punktArtefaktu)){
			print("Dodaje nowy punkt do listy znalezionych artefaktów: "+punktArtefaktu);
			Gra.listaPozycjiZnalezionychArtefaktow.Add(collider.transform.position);
		}
	}

	private void usunPozycjeArtefaktuZListy(Vector3 punktArtefaktu){
		//usuwa punkt znanego artefaktu z listy znalezionych artefaktów
		if(Gra.listaPozycjiZnalezionychArtefaktow.Contains(punktArtefaktu)){
			print("Usuwam punkt z listy znalezionych artefaktów: "+punktArtefaktu);
			Gra.listaPozycjiZnalezionychArtefaktow.Remove(collider.transform.position);
		}
	}

	private string colliderNazwaZawiera (string name){
		if (name.Contains("fx_laserFence")) return "lasery"; 
		if (name.Contains("wayPoint")) return "punkt"; 
		return name;
	}

	private float zaokraglij(float wartosc){
		return (float) Math.Round (wartosc);
	}

	private double zaokraglij(double wartosc){
		return (double) Math.Round (wartosc);
	}
}