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

		Gra.ResumeGame();
		Gra.RestartGame();
	}
}
