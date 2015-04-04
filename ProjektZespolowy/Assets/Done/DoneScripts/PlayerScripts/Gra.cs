using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class Gra 
{
	public static int iloscIteracji = 1;
	public static float iloscCzasuWSekundach = 600;
	public static float szybkoscRozladowaniaBaterii = 0.02f;
	public static float czas;
	public static float bateria;
	public static float naprawa;

	public static bool pauza = false;

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
