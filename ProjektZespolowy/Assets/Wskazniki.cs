using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour{ 
	 
	public Text napisIloscIteracji;
	public Slider suwakCzas;
	public Slider suwakBateria;
	public Slider suwakNaprawa;

	public void ustawSuwakBateri(int wartosc){
		suwakBateria.value = wartosc;
	}
}
