using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static bool IsDefeat { private set; get; } = false;
    public static bool IsPaused { private set; get; } = false;

    private void Start()
    {
        Initialize();
    }
    public static void Initialize()
    {
        foreach (Unit unit in Unit.Units)
        {
            if (unit != null)
            {
                if (unit.tag == "Enemy")
                {
                    if (unit.GetComponent<Enemy>().OnPatrol) unit.Queue.Add(new Order(method => unit.GetComponent<Enemy>().Patrol(), "Patrol"));
                }
            }
        }
    }
    public static void Defeat()
    {
        IsDefeat = true;
        CameraScript.Detach();

        PlayerBar.Update(PlayerBar.PlayerBarType.HEALTH, -Player.Hero.Health, 1f);
        GameObject.Find("Game Manager").GetComponent<Game>().StartCoroutine(DefeatNumerator(2f));
    }
    public static void Pause()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    private static IEnumerator DefeatNumerator(float _time)
    {
        CameraScript.Fade(CameraScript.FadeState.IN, _time);
        yield return new WaitForSeconds(_time);

        //Player.Hero.Remove();
        Interface.Show(false);
        GameObject.Find("Canvas").GetComponent<Interface>().Close(GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel);
        GameObject.Find("Canvas").GetComponent<Interface>().Open(GameObject.Find("Canvas").GetComponent<Interface>().DefeatPanel);

        GameObject.Find("Game Manager").GetComponent<Game>().StopCoroutine(DefeatNumerator(_time));
    }
}
