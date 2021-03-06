﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class Gra 
{
	public static int iloscIteracji = 1;
	public static float iloscCzasuWSekundach = 600f;
	public static float szybkoscRozladowaniaBaterii = 0.02f;
	public static float czas = 600f;
	public static float bateria = 100f;
	public static float naprawa = 100f;

	//zapamietujemy wartoci poczatkowe. Potrzebne przy restarcie.
	public static float czasPoczatkowy = 600f; //podajemy w sekundach
	public static float bateriaPoczatkowa = 100f;
	public static float naprawaPoczatkowa = 100f;

	public static int OstatniWayPointIndex = 0;

	//public static Graph<string> graf = new Graph<string>();
	public static Hashtable tablicaPunktow = new Hashtable();
	public static Hashtable tablicaArtefaktow = new Hashtable();//TODO zapisywać nazwe i punkt artefaktu
	public static List<string >listaPunktowOdwiedzonych = new List<string>();
	public static Queue<string> kolejkaKomunikatow = new Queue<string>();
	
	public static bool pauza = false;

	public static void WyswietlKomunikatWChmurze(string komunikat){
		kolejkaKomunikatow.Enqueue (komunikat);
	}

	public static void RestartGame(){
		iloscIteracji++;
		Application.LoadLevel(0);
	}

	public static void ResumeGame(){
		pauza = false;
		Time.timeScale=1;
	}
	
	public static void PauseGame(){
		pauza = true;
		Time.timeScale=0;
	}
}
