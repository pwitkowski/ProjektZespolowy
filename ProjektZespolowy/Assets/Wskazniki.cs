using UnityEngine;
using System.Collections;

public class Wskazniki {
	public int iloscIteracji;
	public float czas;
	public float bateria;
	public float naprawa;
	public bool kluczDoWindy;

	public Wskazniki (int iloscIteracji, float czas, float bateria, float naprawa, bool kluczDoWindy)
	{
		this.iloscIteracji = iloscIteracji;
		this.czas = czas;
		this.bateria = bateria;
		this.naprawa = naprawa;
		this.kluczDoWindy = kluczDoWindy;
	}

	public Wskazniki (int iloscIteracji, float czas, float bateria, float naprawa)
	{
		this.iloscIteracji = iloscIteracji;
		this.czas = czas;
		this.bateria = bateria;
		this.naprawa = naprawa;
	}

	public Wskazniki (float czas, float bateria, float naprawa)
	{
		this.czas = czas;
		this.bateria = bateria;
		this.naprawa = naprawa;
	}

	public Wskazniki (Wskazniki w)
	{
		iloscIteracji = w.iloscIteracji;
		czas = w.czas;
		bateria = w.bateria;
		naprawa = w.naprawa;
		kluczDoWindy = w.kluczDoWindy;
	}

	public Wskazniki ()
	{
	}

	public string ToString(){
		return "iloscIteracji: " + iloscIteracji + " ,czas: " + czas + " ,bateria: " + bateria + " ,naprawa: " + naprawa+" ,klucz do windy: "+kluczDoWindy;
	}
}
