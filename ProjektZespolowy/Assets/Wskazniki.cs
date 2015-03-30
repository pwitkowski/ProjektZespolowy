using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour {

	public Text napisIloscIteracji;
	public Slider suwakCzasu;
	public float licznikCzasu = 5f;
	public static bool restartGry = false;

	private float czas;

	// Use this for initialization
	void Start () {
		czas = licznikCzasu;
		napisIloscIteracji.text = ""+Gra.iloscIteracji;
	}
	
	// Update is called once per frame
	void Update () {
		licznikCzasu -= Time.deltaTime;
		if (licznikCzasu > 0) {
			suwakCzasu.value = (100 * licznikCzasu) / czas;
		}else{
			Gra.iloscIteracji++;
			Application.LoadLevel (0);
		}
	}
}
