using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static bool IsDefeat { private set; get; } = false;
    public static bool IsPaused { private set; get; } = false;

    public GameObject DefeatPanel;

    public static void Defeat()
    {
        IsDefeat = true;
        CameraScript.AttachToUnit(null);        
        
        GameObject.Find("Game Manager").GetComponent<Game>().StartCoroutine(DefeatNumerator(2f));        
    }
    public static void Pause()
    {
        
    }

    private static IEnumerator DefeatNumerator(float _time)
    {
        CameraScript.Fade(CameraScript.FadeState.IN, _time);
        yield return new WaitForSeconds(_time);
        GameObject.Find("Game Manager").GetComponent<Game>().DefeatPanel.SetActive(true);
        GameObject.Find("Game Manager").GetComponent<Game>().StopCoroutine(DefeatNumerator(_time));
    }
}
