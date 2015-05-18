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
		Gra.wskazniki.iloscIteracji = 0;
		Gra.iloscCzasuWSekundach = int.Parse(iloscCzasuWsekundach.text.ToString ());
		Gra.szybkoscRozladowaniaBaterii = float.Parse(szybkoscRozladowaniaBaterii.text.ToString());
		Gra.wskazniki.czas = Gra.iloscCzasuWSekundach * (suwakCzas.value / 100); 
		Gra.wskazniki.bateria = suwakBateria.value;
		Gra.wskazniki.naprawa = suwakNaprawa.value;

		//TODO dodać do ustawień checkboxy czy ma klucz do windy itp.
		Gra.wskazniki.kluczDoWindy = false; 
		Gra.czyPotrzebujeKluczaDoWindy = false;
		Gra.czyZnalazlemKluczDoWindy = false;
		Gra.czyZnalazlemWyjscie = false;

		Gra.wskaznikiPoczatkowe.czas = Gra.wskazniki.czas;
		Gra.wskaznikiPoczatkowe.bateria = Gra.wskazniki.bateria;
		Gra.wskaznikiPoczatkowe.naprawa = Gra.wskazniki.naprawa;
		Gra.wskaznikiPoczatkowe.kluczDoWindy = Gra.wskazniki.kluczDoWindy;// czyli false

		Gra.ResumeGame();
		Gra.RestartGame();
	}
}
