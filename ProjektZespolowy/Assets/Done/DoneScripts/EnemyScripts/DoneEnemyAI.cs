using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DoneEnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;							// The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;							// The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;						// The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;						// The amount of time to wait when the patrol way point is reached.
	//public Transform[] patrolWayPoints;						// An array of transforms for the patrol route.

	private DoneEnemySight enemySight;						// Reference to the EnemySight script.
	private NavMeshAgent nav;								// Reference to the nav mesh agent.
	private Transform player;								// Reference to the player's transform.
	private DonePlayerHealth playerHealth;					// Reference to the PlayerHealth script.
	private DoneLastPlayerSighting lastPlayerSighting;		// Reference to the last global sighting of the player.
	private float chaseTimer;								// A timer for the chaseWaitTime.
	private float patrolTimer;								// A timer for the patrolWaitTime.
	private int wayPointIndex;								// A counter for the way point array.
	private Vector3 punkt;
	private List<string> listaPunktowDoWyboru = new List<string>();
	private Vector3 wyjscie;
	private String komunikat = "";
	private Animator playerAnim;
	private DoneHashIDs hash;

	void Awake (){
		// Setting up the references.
		enemySight = GetComponent<DoneEnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag(DoneTags.player).transform;
		playerHealth = player.GetComponent<DonePlayerHealth>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
		punkt = new Vector3();
		wyjscie = GameObject.FindGameObjectWithTag(DoneTags.wyjscie).transform.position;
		playerAnim = GameObject.FindGameObjectWithTag(DoneTags.player).GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
	}
	
	void Update (){
		//jeśli dojdzie do kolejnego punktu na mapie 
		//lub jeśli robot się zatrzymał to podejmuje kolejną decyzję
		if (nav.destination == punkt 
		    || nav.remainingDistance < nav.stoppingDistance
		    || playerAnim.GetFloat(hash.speedFloat) == 0f){

			//ustawiam szybkosc
			nav.speed = patrolSpeed;

			//czyszczę komunikat podjętej decyzji
			komunikat = "";

			//podejmowanie decyzji
			stanWydostanieSieZWiezienia ();
			
			if(czyPoprawicWskazniki()) stanPoprawaWskaznikow ();

			//wyświetlam komunikat podjętej decyzji
			if(komunikat != null && komunikat.Length > 0) Gra.WyswietlKomunikatWChmurze(komunikat);
		}
		
		//ide do punktu na mapie
		nav.destination = punkt;
	}

	bool czyPoprawicWskazniki (){
		//zakładam, że wskaźniki muszą utrymywać sie na poziomie 50%
		if (ObslugaWskaznikow.DajIloscCzasuWPrzeliczeniuNaProcent() <= Gra.wartoscKrytyczna
		    || Gra.wskazniki.bateria <=  Gra.wartoscKrytyczna
		    || Gra.wskazniki.naprawa <=  Gra.wartoscKrytyczna
		    || Gra.czyPotrzebujeKluczaDoWindy ) {
			return true;
		} else {
			return false;
		}
	}

	void stanWydostanieSieZWiezienia (){
		print("Stan wydostanieSieZWiezienia");

		//jeśli znam wyjście i mam klucz do windy to uciekam z więzienia
		if (Gra.czyZnalazlemWyjscie && Gra.czyZnalazlemKluczDoWindy) {
			komunikat = "Uciekam z wiezienia !";
			nav.speed = chaseSpeed;
			punkt = wyjscie;
			return;
		}

		//biore pierwszy nieodwiedzony punkt z tablicy
		listaPunktowDoWyboru = new List<string> ();
		foreach (DictionaryEntry p in Gra.tablicaPunktow) {
			if (!Gra.listaPunktowOdwiedzonych.Contains (p.Key.ToString ())) {
				listaPunktowDoWyboru.Add (p.Key.ToString ());
				//print("Idę do punktu: "+p.Key.ToString());
				//punkt = (Vector3) p.Value;
				//break;
			}
		}
		
		//wybieram losowy punkt
		if (listaPunktowDoWyboru.Count > 0) {
			System.Random r = new System.Random();
			int index = r.Next (0, listaPunktowDoWyboru.Count);
			string nazwaPunktu = listaPunktowDoWyboru [index];
			//print ("Idę do punktu: " + nazwaPunktu);
			komunikat = "Ide do punktu: " + nazwaPunktu;
			punkt = (Vector3)Gra.tablicaPunktow [nazwaPunktu];
		}
	}	

	void stanPoprawaWskaznikow (){
		print ("Stan poprawa wskaznikow");

		//jeśli znam wyjście i mam klucz do windy to nie poprawiam wskaźników tylko uciekam
		if (Gra.czyZnalazlemWyjscie && Gra.czyZnalazlemKluczDoWindy) return;

		if (czyZnamArtefaktKtoryZaspokoiMojePotrzeby()) {
			//jeśli znam artefakt który zaspokoi moje potrzeby to ide do niego
			punkt = dajPozycjeArtefaktoKtoryZaspokoiMojePotrzeby(dajNazweArtefaktuKtoryNajbardziejZaspokajaMojaPotrzebe(dajAktualnaPorzebeDoZaspokojenia()));
		} else {
			//w przeciwnym razie zmieniam stan na poznawanie artefaktów
			poznawanieArtefaktow();
		}
	}

	bool czyZnamArtefaktKtoryZaspokoiMojePotrzeby (){
		//zwraca true jeśli znam pozycję artefaktu jaki może zaspokoić moją potrzebę

		//sprawdzam jaką potrzebę muszę zaspokoić
		string potrzeba = dajAktualnaPorzebeDoZaspokojenia ();

		//sprawdzam jaki artefakt najbardziej zaspakaja moją potrzebę 
		string artefakt = dajNazweArtefaktuKtoryNajbardziejZaspokajaMojaPotrzebe(potrzeba);

		foreach(DictionaryEntry entry in Gra.tablicaPozycjiRozpoznanychArtefaktow){
			print("Rozpoznane artefakty: "+entry.Key);
		}

		if (Gra.tablicaPozycjiRozpoznanychArtefaktow.Contains (artefakt)) {
			komunikat = "Ide zaspokoic potrzebe "+potrzeba;
			return true;
		} else {
			komunikat = "Nie wiem jak zaspokoic potrzebe "+potrzeba;
			return false;
		}
	}

	string dajAktualnaPorzebeDoZaspokojenia (){
		//wyszukuje największy ból na wskaźniku
		var dict = new Dictionary<string, float> {
			 { "czas", Gra.wskaznikiBolu.czas }
			,{ "bateria", Gra.wskaznikiBolu.bateria }
			,{ "naprawa", Gra.wskaznikiBolu.naprawa }
		};

		if (Gra.czyPotrzebujeKluczaDoWindy) dict.Add ("kluczDoWindy", 50f/Gra.slownikPriorytetow["kluczDoWindy"]);

		var max = dict.Values.Max();
		var relevantKey = dict
			.Where (x => max.Equals (x.Value))
				.Select (x => x.Key)
//				.First ();
				.FirstOrDefault ();

		print ("Aktualna potrzeba do zaspokojenia: " + relevantKey +" wartość: "+max);
		return relevantKey;
	}

	string dajNazweArtefaktuKtoryNajbardziejZaspokajaMojaPotrzebe (string potrzeba){
		Dictionary<string, float> dict = new Dictionary<string, float>();
		string artefakt = "";
		float max = 0f;
		switch (potrzeba) {
			case "czas":
				foreach (DictionaryEntry entry in Gra.tablicaArtefaktBol){
					string klucz = (string) entry.Key;
					Wskazniki wartosc = (Wskazniki) entry.Value;
					dict.Add(klucz,wartosc.czas);
				}
				break;
			case "bateria":
				foreach (DictionaryEntry entry in Gra.tablicaArtefaktBol){
					string klucz = (string) entry.Key;
					Wskazniki wartosc = (Wskazniki) entry.Value;
					dict.Add(klucz,wartosc.bateria);
				}
				break;
			case "naprawa":
				foreach (DictionaryEntry entry in Gra.tablicaArtefaktBol){
					string klucz = (string) entry.Key;
					Wskazniki wartosc = (Wskazniki) entry.Value;
					dict.Add(klucz,wartosc.naprawa);
				}
				break;
			case "kluczDoWindy":
				foreach (DictionaryEntry entry in Gra.tablicaArtefaktBol){
					string klucz = (string) entry.Key;
					Wskazniki wartosc = (Wskazniki) entry.Value;
					dict.Add(klucz,wartosc.kluczDoWindy ? 1f : 0f);
				}
				break;
			default:
				print ("Nie poznałem jeszcze żadnych artefaktów");
				break;
		}

		foreach (KeyValuePair<string, float> entry in dict) {
			print("Potrzeba: "+potrzeba+" artefakt: "+entry.Key+" wartość: "+entry.Value);
		}

		if (dict.Count > 0) {
			max = dict.Values.Max ();
			artefakt = dict
			.Where (x => max.Equals (x.Value))
				.Select (x => x.Key)
//				.First ();
				.FirstOrDefault ();
		}

		if (dict.Count == 0 || max <= 0f || artefakt == null){
			print("Nie znalazłem artefaktu który mógłby zaspokoić moją potrzebę");
			return "";
		}
			
		print ("Artefakt który najbardziej zaspokaja potrzebę: "+potrzeba+" to: " + artefakt +" wartość jego to: "+max);
		return artefakt;
	}

	Vector3 dajPozycjeArtefaktoKtoryZaspokoiMojePotrzeby (string artefakt){
		//zwraca pozycje artefaktu który najbardziej zaspokoja moja potrzebe
		return (Vector3) Gra.tablicaPozycjiRozpoznanychArtefaktow[artefakt];
	}

	void poznawanieArtefaktow (){
		print ("Stan poznawanie artefaktow");
		if (Gra.listaPozycjiZnalezionychArtefaktow.Count > 0) {
			//jeśli znaleziono jakieś artefakty to ide do losowego z nich
			System.Random r = new System.Random();
			int index = r.Next(0, Gra.listaPozycjiZnalezionychArtefaktow.Count);
			punkt = Gra.listaPozycjiZnalezionychArtefaktow [index];
		}else {
			//w przeciwnym razie zmieniam stan na przechodzenie waypointow
			stanWydostanieSieZWiezienia();
		}
	}
}
