using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ustawienia : MonoBehaviour {
	
	public GameObject panelUstawienia;

	public InputField iloscCzasuWsekundach;
	public InputField szybkoscRozladowaniaBaterii;
	public Text napisCzas;
	public Text napisBateria;
	public Text napisNaprawa;
	
	public Slider suwakCzas;
	public Slider suwakBateria;
	public Slider suwakNaprawa;

	public Button restart;
	
	private bool pokazUstawienia = false;

	//--------------------------------
	public float health = 100f;							// How much health the player has left.
	public float resetAfterDeathTime = 5f;				// How much time from the player dying to the level reseting.
	public AudioClip deathClip;							// The sound effect of the player dying.
	
	private Animator anim;								// Reference to the animator component.
	private DonePlayerMovement playerMovement;			// Reference to the player movement script.
	private DoneHashIDs hash;							// Reference to the HashIDs.
	private DoneSceneFadeInOut sceneFadeInOut;			// Reference to the SceneFadeInOut script.
	private DoneLastPlayerSighting lastPlayerSighting;	// Reference to the LastPlayerSighting script.
	private float timer;								// A timer for counting to the reset of the level once the player is dead.
	private bool playerDead;							// A bool to show if the player is dead or not.
	//--------------------------------

	void Start(){
		panelUstawienia.SetActive(pokazUstawienia);

		iloscCzasuWsekundach.text = ""+Gra.iloscCzasuWSekundach;
		szybkoscRozladowaniaBaterii.text = ""+Gra.szybkoscRozladowaniaBaterii;
	
		restart.onClick.AddListener (delegate { WczytajNoweUstawienia(); });
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			pokazUstawienia = !pokazUstawienia;
			panelUstawienia.SetActive(pokazUstawienia);
			if(pokazUstawienia) Gra.PauseGame();
			else Gra.ResumeGame();
		}

		if (pokazUstawienia) {
			napisCzas.text = suwakCzas.value.ToString ();
			napisBateria.text = suwakBateria.value.ToString ();
			napisNaprawa.text = suwakNaprawa.value.ToString ();
		}
	}

	public void WczytajNoweUstawienia(){
		Gra.iloscIteracji = 0;
		Gra.iloscCzasuWSekundach = int.Parse(iloscCzasuWsekundach.text.ToString ());
		Gra.szybkoscRozladowaniaBaterii = float.Parse(szybkoscRozladowaniaBaterii.text.ToString());
		Gra.czas = Gra.iloscCzasuWSekundach * (suwakCzas.value / 100); 
		Gra.bateria = suwakBateria.value;
		Gra.naprawa = suwakNaprawa.value;

		Gra.czasPoczatkowy = Gra.czas;
		Gra.bateriaPoczatkowa = Gra.bateria;
		Gra.naprawaPoczatkowa = Gra.naprawa;

		Gra.ResumeGame();
		Gra.RestartGame();
	}
}
