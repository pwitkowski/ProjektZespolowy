﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Wskazniki : MonoBehaviour {
	
	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public Image kolorSuwakaCzasu;
	public Slider suwakBateri;
	public Image kolorSuwakaBaterii;
	public Slider suwakNaprawy;
	public Image kolorSuwakaNaprawy;
	
	public float health = 100f;							// How much health the player has left.
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
		napisIloscIteracji.text = ""+Gra.iloscIteracji;
		suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
		suwakBateri.value = Gra.bateriaPoczatkowa;
		suwakNaprawy.value = Gra.naprawaPoczatkowa;

		Gra.czas = Gra.czasPoczatkowy;
		Gra.bateria = Gra.bateriaPoczatkowa;
		Gra.naprawa = Gra.naprawaPoczatkowa;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Gra.pauza) {
			Gra.czas -= Time.deltaTime;
			Gra.bateria -= Gra.szybkoscRozladowaniaBaterii;

			if (Gra.czas >= 0) {
				suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
			}

			if (Gra.bateria >= 0) {
				suwakBateri.value = Gra.bateria; 
			}

			if (Gra.naprawa >= 0) {
				suwakNaprawy.value = Gra.naprawa;
			}	

			UstawKolorySuwakow ();
			
			if (Gra.czas <= 0 || Gra.bateria <= 0 || Gra.naprawa <= 0) {
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

	float DajIloscCzasuWPrzeliczeniuNaProcent(){
		return (float) Math.Round((100 * Gra.czas) / Gra.iloscCzasuWSekundach,1);
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
