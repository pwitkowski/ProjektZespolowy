﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour {
	
	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public Image kolorSuwakaCzasu;
	public Slider suwakBateri;
	public Image kolorSuwakaBaterii;
	public Slider suwakNaprawy;
	public Image kolorSuwakaNaprawy;

	public float czas; //podajemy w sekundach
	public float bateria;
	public float naprawa;
	public float szybkoscUtratyBateri;

	private float czasPoczatkowy; //zapamiętujemy żeby przeliczyć na %

	// Use this for initialization
	void Start () {
		czasPoczatkowy = czas;
		napisIloscIteracji.text = ""+Gra.iloscIteracji;
		suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
		suwakBateri.value = bateria;
		Gra.naprawa = naprawa;
		suwakNaprawy.value = Gra.naprawa;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Gra.pauza) {
			czas -= Time.deltaTime;
			bateria -= szybkoscUtratyBateri;

			if (czas > 0) {
				suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent ();
			}

			if (bateria > 0) {
				suwakBateri.value = bateria; 
			}

			if (Gra.naprawa > 0) {
				suwakNaprawy.value = Gra.naprawa;
			}	

			if (czas <= 0 || bateria <= 0 || Gra.naprawa <= 0) {
				Gra.RestartGame();
			}

			UstawKolorySuwakow ();
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
		return (100 * czas) / czasPoczatkowy;
	}
}
