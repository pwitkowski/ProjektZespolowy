using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Wskazniki : MonoBehaviour {

	public Text napisIloscIteracji;

	// Use this for initialization
	void Start () {
		napisIloscIteracji.text = ""+Gra.iloscIteracji;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
