using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	void Awake ()
	{
		// Setting up the references.
		enemySight = GetComponent<DoneEnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag(DoneTags.player).transform;
		playerHealth = player.GetComponent<DonePlayerHealth>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
		punkt = new Vector3();
	}
	
	
	void Update ()
	{
		// If the player is in sight and is alive...
		if(enemySight.playerInSight && playerHealth.health > 0f)
			// ... shoot.
			Shooting();
		
		// If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
			// ... chase.
			Chasing();
		
		// Otherwise...
		else
			// ... patrol.
			Patrolling();
	}
	
	
	void Shooting ()
	{
		// Stop the enemy where it is.
		nav.Stop();
	}
	
	
	void Chasing ()
	{
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;
		
		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
	}

	
	void Patrolling ()
	{
		//ustawiam szybkosc
		nav.speed = patrolSpeed;
		if (nav.destination == punkt || nav.remainingDistance < nav.stoppingDistance) {
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
				int index = Random.Range (0, listaPunktowDoWyboru.Count);
				string nazwaPunktu = listaPunktowDoWyboru [index];
				//print ("Idę do punktu: " + nazwaPunktu);
				Gra.WyswietlKomunikatWChmurze("Ide do punktu: " + nazwaPunktu);
				punkt = (Vector3)Gra.tablicaPunktow [nazwaPunktu];
			}

			//TODO dodać priorytetyzacje potrzeb
			if (Gra.bateria <= 30f) punkt = dajPozycjeBaterii (punkt);
		}

		//ide do punktu na mapie
		nav.destination = punkt;
	}

	Vector3 dajPozycjeBaterii(Vector3 punktDoKtoregoIde){
		if (Gra.tablicaArtefaktow.Contains ("bateria")) {
			print ("Ide podladowac baterie");
			Gra.WyswietlKomunikatWChmurze ("Ide podladowac baterie");
			//zwracam pozycje baterii jeśli ją znam
			return (Vector3) Gra.tablicaArtefaktow["bateria"];
		} else {
			print("Nie wiem gdzie jest bateria. Szukam dalej ...");
			Gra.WyswietlKomunikatWChmurze ("Podladowalbym baterie ale nie wiem gdzie jest ...");
			return punktDoKtoregoIde;
		}
	}

	Vector3 dajPozycjeZegara(Vector3 punktDoKtoregoIde){
		if (Gra.tablicaArtefaktow.Contains ("zegar")) {
			print ("Ide zyskac troche czasu");
			Gra.WyswietlKomunikatWChmurze ("Ide zyskac troche czasu");
			//zwracam pozycje zegara jeśli ją znam
			return (Vector3) Gra.tablicaArtefaktow["zegar"];
		} else {
			print("Nie wiem gdzie jest zegar. Szukam dalej ...");
			Gra.WyswietlKomunikatWChmurze ("Zdobylbym troche wiecej czasu ale nie wiem jak ...");
			return punktDoKtoregoIde;
		}
	}

	Vector3 dajPozycjeApteczki(Vector3 punktDoKtoregoIde){
		if (Gra.tablicaArtefaktow.Contains ("apteczka")) {
			print ("Ide podreperowac uklady scalone");
			Gra.WyswietlKomunikatWChmurze ("Ide podladowac uklady scalone");
			//zwracam pozycje apteczki jeśli ją znam
			return (Vector3) Gra.tablicaArtefaktow["apteczka"];
		} else {
			print("Nie wiem gdzie jest apteczka. Szukam dalej ...");
			Gra.WyswietlKomunikatWChmurze ("Podreperowalbym sie ale nie wiem jak ...");
			return punktDoKtoregoIde;
		}
	}
}
