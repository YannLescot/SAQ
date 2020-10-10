using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RedirectionsHome : MonoBehaviour
{
	private void Start()
	{
		Screen.orientation = ScreenOrientation.Portrait;
		//Debug.Log(Screen.orientation);
	}
	public void ClickFree()
	{
		PlayerPrefs.SetInt("gameMode", 1);
		SceneManager.LoadScene("Choixjoueurs");
	}

	public void ClickPremium()
	{
		PlayerPrefs.SetInt("gameMode", 2);
		SceneManager.LoadScene("Choixjoueurs");
	}


	public void ClickHebdo()
	{
		PlayerPrefs.SetInt("gameMode", 3);
		SceneManager.LoadScene("Choixjoueurs");
	}
}
