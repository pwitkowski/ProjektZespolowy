using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour {
	
	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public Slider suwakBateri;
	public Slider suwakNaprawy;

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
		suwakNaprawy.value = naprawa;
	}
	
	// Update is called once per frame
	void Update () {
		czas -= Time.deltaTime;
		bateria -= szybkoscUtratyBateri;

		if (czas > 0) {
			suwakCzasu.value = DajIloscCzasuWPrzeliczeniuNaProcent();
		}

		if(bateria > 0){
			suwakBateri.value = bateria; 
		}

		if(czas <= 0 || bateria <= 0){
			Gra.iloscIteracji++;
			Application.LoadLevel (0);
		}
	}

	float DajIloscCzasuWPrzeliczeniuNaProcent(){
		return (100 * czas) / czasPoczatkowy;
	}
}
