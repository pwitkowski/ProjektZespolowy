using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Chmura : MonoBehaviour {

	public Transform kamera;
	public GameObject canvasChmurka;
	public Text textChmura;
	public float czasWyswietlaniaChmury = 2f; //ustawiane w unity

	private float stoper = 0f;
	
	void Start () {
		//ukrywam chmurke na starcie
		canvasChmurka.SetActive (false);
	}
	
	// Update is called once per frame
	void Update() {
		string komunikat = null;
		try{
			//biore pierwszy komunikat z kolejki
			komunikat = Gra.kolejkaKomunikatow.Peek();
			//print ("Komunikat: " + komunikat);
		}catch(InvalidOperationException e){
			//print("Kolejka komunikatów jest pusta");
		}


		if (komunikat != null && komunikat.Length > 0) {

			//jeśli gra nie jest zastopowana to zwiekszam stoper
			if(!Gra.pauza) stoper += Time.deltaTime;

			//jeśli czas wyswietlania komuikatu minął
			if(stoper >= czasWyswietlaniaChmury){
				//zeruje stoper
				stoper = 0f;

				//usuwam z kolejki komunikat
				Gra.kolejkaKomunikatow.Dequeue();

				//ukrywam chmurke
				canvasChmurka.SetActive(false);
			}else{
				//ustawiam tekst w chmurce
				textChmura.text = komunikat;

				//pokazuje chmurke
				canvasChmurka.SetActive(true);

				//ustawiam rotacje wzgledem kamery
				transform.rotation = kamera.rotation;
			}
		}else{
			//ukrywam chmurke
			canvasChmurka.SetActive(false);
		}
	}
}
