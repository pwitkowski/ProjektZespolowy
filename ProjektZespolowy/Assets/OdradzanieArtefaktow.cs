using UnityEngine;
using System.Collections;

public class OdradzanieArtefaktow : MonoBehaviour {
	
	public GameObject bateria;
	public GameObject bomba;
	public GameObject zegar;
	public GameObject apteczka;
	public float czasOdrodzenia = 10f; //ustawiany w unity
	
	void Start ()
	{
		//odpala funkcje odradzanie po upływie czasuOdrodzenia
		InvokeRepeating ("Odradzanie", czasOdrodzenia, czasOdrodzenia);
	}
	
	
	void Odradzanie ()
	{	
		//jeśli artefakt nie jest aktywny to aktywuje go
		if (bateria.active == false) bateria.active = true;
		if (bomba.active == false) bomba.active = true;
		if (zegar.active == false) zegar.active = true;
		if (apteczka.active == false) apteczka.active = true;
	}
}
