using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class Gra 
{
	//ustawienia początkowe wskaźników
	public static float iloscCzasuWSekundach = 600f;
	public static float szybkoscRozladowaniaBaterii = 0.02f;
	public static Wskazniki wskazniki = new Wskazniki(1,600f,100f,100f,false);

	//wartosc krytyczna dla wskaznikow
	public static float wartoscKrytyczna = 50f;

	//zapamiętujemy wartości początkowe. Potrzebne przy restarcie.
	public static Wskazniki wskaznikiPoczatkowe = new Wskazniki(1,600f,100f,100f,false);

	//wskaźniki bólu
	public static Wskazniki wskaznikiBolu = new Wskazniki(1,0f,0f,0f,false);

	//waypointy
	public static Hashtable tablicaPunktow = new Hashtable();
	public static List<string >listaPunktowOdwiedzonych = new List<string>();

	//artefakty
	public static Hashtable tablicaPozycjiRozpoznanychArtefaktow = new Hashtable();
	public static List<Vector3> listaPozycjiZnalezionychArtefaktow = new List<Vector3>();
	public static Hashtable tablicaArtefaktBol = new Hashtable();

	//priorytety
	public static Dictionary<string, int> slownikPriorytetow = new Dictionary<string, int>(){
		{"wydostanieSieZWiezienia", 1},
		{"czas", 2},
		{"bateria", 3},
		{"naprawa", 4},
		{"kluczDoWindy", 5},
		{"poznawanieArtefaktow", 6}
	};
	public static bool czyPotrzebujeKluczaDoWindy = false;

	//komunikaty "chmurka"
	public static Queue<string> kolejkaKomunikatow = new Queue<string>();

	//potrzebne do zatrzymywania gry oraz deltatime
	public static bool pauza = false;

	public static void WyswietlKomunikatWChmurze(string komunikat){
		kolejkaKomunikatow.Enqueue (komunikat);
	}

	public static void RestartGame(){
		wskazniki.iloscIteracji++;
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


