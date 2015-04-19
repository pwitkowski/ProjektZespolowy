using UnityEngine;
using System.Collections;

public class OdradzanieArtefaktow : MonoBehaviour {
	
	public GameObject bateria;
	public float czasOdrodzenia = 5f; //ustawiany w unity
	
	void Start ()
	{
		//odpala funkcje odradzanie po upływie czasuOdrodzenia
		InvokeRepeating ("Odradzanie", czasOdrodzenia, czasOdrodzenia);
	}
	
	
	void Odradzanie ()
	{	
		//jeśli artefakt nie jest aktywny to odradzam go
		if (bateria.active == false) bateria.active = true;
	}
}
