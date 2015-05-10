using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ObslugaWskaznikow: MonoBehaviour {
	
	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public Image kolorSuwakaCzasu;
	public Slider suwakBateri;
	public Image kolorSuwakaBaterii;
	public Slider suwakNaprawy;
	public Image kolorSuwakaNaprawy;
	public Image kluczDoWindy;
	
	//public float health = 100f;							// How much health the player has left.
	public float resetAfterDeathTime = 5f;				// How much time from the player dying to the level reseting.
	//public AudioClip deathClip;							// The sound effect of the player dying.
	
	private Animator anim;								// Reference to the animator component.
	//private DonePlayerMovement playerMovement;			// Reference to the player movement script.
	private DoneHashIDs hash;							// Reference to the HashIDs.
	private DoneSceneFadeInOut sceneFadeInOut;			// Reference to the SceneFadeInOut script.
	private DoneLastPlayerSighting lastPlayerSighting;	// Reference to the LastPlayerSighting script.
	private float timer;								// A timer for counting to the reset of the level once the player is dead.
	private bool playerDead;							// A bool to show if the player is dead or not.
	private static int iloscIteracji;

	void Awake ()
	{
		// Setting up the references.
		anim = GameObject.FindGameObjectWithTag(DoneTags.player).GetComponent<Animator>();
		//playerMovement = GetComponent<DonePlayerMovement>();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
		sceneFadeInOut = GameObject.FindGameObjectWithTag(DoneTags.fader).GetComponent<DoneSceneFadeInOut>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneLastPlayerSighting>();
	}

	// Use this for initialization
	void Start () {
		//ustawiam napisy i suwaki
		napisIloscIteracji.text = ""+Gra.wskazniki.iloscIteracji;
		suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
		suwakBateri.value = Gra.wskaznikiPoczatkowe.bateria;
		suwakNaprawy.value = Gra.wskaznikiPoczatkowe.naprawa;

		//ustawiam wskaźniki początkowe
		Gra.wskazniki.czas = Gra.wskaznikiPoczatkowe.czas;
		Gra.wskazniki.bateria = Gra.wskaznikiPoczatkowe.bateria;
		Gra.wskazniki.naprawa = Gra.wskaznikiPoczatkowe.naprawa;
		Gra.wskazniki.kluczDoWindy = Gra.wskaznikiPoczatkowe.kluczDoWindy;

		//czyszcze dane tj. kolejkaKomunikatów itp.
		Gra.kolejkaKomunikatow = new System.Collections.Generic.Queue<string>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Gra.pauza) {
			Gra.wskazniki.czas -= Time.deltaTime;
			Gra.wskazniki.bateria -= Gra.szybkoscRozladowaniaBaterii;

			if (Gra.wskazniki.czas >= 0) {
				suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
			}

			if (Gra.wskazniki.bateria >= 0) {
				suwakBateri.value = Gra.wskazniki.bateria; 
			}

			if (Gra.wskazniki.naprawa >= 0) {
				suwakNaprawy.value = Gra.wskazniki.naprawa;
			}	

			kluczDoWindy.active = Gra.wskazniki.kluczDoWindy;

			PrzeliczWskaznikiBolu();

			UstawKolorySuwakow ();
			
			if (Gra.wskazniki.czas <= 0 || Gra.wskazniki.bateria <= 0 || Gra.wskazniki.naprawa <= 0) {
				//Gra.RestartGame();
				// ... and if the player is not yet dead...
				if(!playerDead)
					// ... call the PlayerDying function.
					PlayerDying();
				else
				{
					// Otherwise, if the player is dead, call the PlayerDead and LevelReset functions.
					PlayerDead();
					LevelReset();
				}
			}
		}
	}

	void PrzeliczWskaznikiBolu (){
		//przelicza wskaźniki bólu według wzoru 100 - wskaźnik / priorytet
		Gra.wskaznikiBolu.czas = (100f-DajIloscCzasuWPrzeliczeniuNaProcent())/Gra.slownikPriorytetow["czas"];
		Gra.wskaznikiBolu.bateria = (100f-Gra.wskazniki.bateria)/Gra.slownikPriorytetow["bateria"];
		Gra.wskaznikiBolu.naprawa = (100f-Gra.wskazniki.naprawa)/Gra.slownikPriorytetow["naprawa"];
//		Gra.wskaznikiBolu.kluczDoWindy = Gra.wskazniki.kluczDoWindy ? false : true; //jeśli nie mam klucza to ustawiam 1 w przeciwnym razie 0

		//print ("Wskaźniki bólu: "+Gra.wskaznikiBolu.ToString());
	}

	void UstawKolorySuwakow(){
		kolorSuwakaCzasu.color = DajKolorSukawa(suwakCzasu);
		kolorSuwakaBaterii.color = DajKolorSukawa(suwakBateri);
		kolorSuwakaNaprawy.color = DajKolorSukawa(suwakNaprawy);
	}

	Color DajKolorSukawa(Slider suwak){
		if (suwak.value <= 33) {
			return Color.red;
		} else if (suwak.value >= 33 && suwak.value <= 66) {
			return Color.yellow;
		} else {
			return Color.green;
		}
	}

	public static float DajIloscCzasuWPrzeliczeniuNaProcent(){
		return (float) Math.Round((100 * Gra.wskazniki.czas) / Gra.iloscCzasuWSekundach,1);
	}
	
	void PlayerDying ()
	{
		// The player is now dead.
		playerDead = true;
		
		// Set the animator's dead parameter to true also.
		anim.SetBool(hash.deadBool, playerDead);
		
		// Play the dying sound effect at the player's location.
		//AudioSource.PlayClipAtPoint(deathClip, transform.position);
	}
	
	
	void PlayerDead ()
	{
		// If the player is in the dying state then reset the dead parameter.
		if(anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.dyingState)
			anim.SetBool(hash.deadBool, false);
		
		// Disable the movement.
		anim.SetFloat(hash.speedFloat, 0f);
		//playerMovement.enabled = false;
		
		// Reset the player sighting to turn off the alarms.
		lastPlayerSighting.position = lastPlayerSighting.resetPosition;
		
		// Stop the footsteps playing.
		//audio.Stop();
	}
	
	
	void LevelReset ()
	{
		// Increment the timer.
		timer += Time.deltaTime;
		
		//If the timer is greater than or equal to the time before the level resets...
		if (timer >= resetAfterDeathTime) {
			// ... reset the level.
			sceneFadeInOut.EndScene ();
		}
	}
}
