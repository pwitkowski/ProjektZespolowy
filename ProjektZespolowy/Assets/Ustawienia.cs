using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ustawienia : MonoBehaviour {
	
	public GameObject panelUstawienia;

	public Text iloscCzasuWsekundach;
	public Text szybkoscRozladowaniaBaterii;
	public Text napisCzas;
	public Text napisBateria;
	public Text napisNaprawa;
	
	public Slider suwakCzas;
	public Slider suwakBateria;
	public Slider suwakNaprawa;
	
	private bool pokazUstawienia = false;

	void Start(){
		panelUstawienia.SetActive(pokazUstawienia);

		iloscCzasuWsekundach.text = ""+Gra.iloscCzasuWSekundach;
		szybkoscRozladowaniaBaterii.text = ""+Gra.szybkoscRozladowaniaBaterii;
	}
	
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			pokazUstawienia = !pokazUstawienia;
			panelUstawienia.SetActive(pokazUstawienia);
		}

		if (pokazUstawienia) {
			iloscCzasuWsekundach.text = ""+Gra.iloscCzasuWSekundach;
			szybkoscRozladowaniaBaterii.text = ""+Gra.szybkoscRozladowaniaBaterii;
			napisCzas.text = suwakCzas.value.ToString ();
			napisBateria.text = suwakBateria.value.ToString ();
			napisNaprawa.text = suwakNaprawa.value.ToString ();
		}
	}
}
