using UnityEngine;
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

			if (Gra.czas > 0) {
				suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
			}

			if (Gra.bateria > 0) {
				suwakBateri.value = Gra.bateria; 
			}

			if (Gra.naprawa > 0) {
				suwakNaprawy.value = Gra.naprawa;
			}	

			UstawKolorySuwakow ();
			
			if (Gra.czas <= 0 || Gra.bateria <= 0 || Gra.naprawa <= 0) {
				Gra.RestartGame();
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
}
