using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour {
	
	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public Slider suwakBateri;
	public Slider suwakNaprawy;

	public float licznikCzasu = 50f;
	public float bateria = 100;
	public float naprawa =100;
	public float szybkoscUtratyBateri = 0.02f;

	private float czas;

	// Use this for initialization
	void Start () {
		czas = licznikCzasu;
		napisIloscIteracji.text = ""+Gra.iloscIteracji;
		suwakCzasu.value = licznikCzasu;
		suwakBateri.value = bateria;
		suwakNaprawy.value = naprawa;
	}
	
	// Update is called once per frame
	void Update () {
		licznikCzasu -= Time.deltaTime;
		bateria -= szybkoscUtratyBateri;

		if (licznikCzasu > 0) {
			suwakCzasu.value = (100 * licznikCzasu) / czas;
		}

		if(bateria > 0){
			suwakBateri.value = bateria; 
		}

		if(licznikCzasu <= 0 || bateria <= 0){
			Gra.iloscIteracji++;
			Application.LoadLevel (0);
		}
	}
}
