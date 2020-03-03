using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum UnitBarType { HEALTH, MANA }
    public static void UpdateBar(UnitBarType _type, float _from, float _to, float _time)
    {
        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedBar(_type, _from, _to, _time));
    }
    public static IEnumerator InterpolatedBar(UnitBarType _type, float _from, float _to, float _time)
    {
        float startTime = Time.time;
        switch (_type)
        {
            case UnitBarType.HEALTH:
                while (GameObject.Find("Health").transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount != _to / Player.Hero.MaxHealth)
                {
                    float elapsedTime = Time.time - startTime;
                    float delta = Mathf.Lerp(_from, _to, elapsedTime / _time);
                    GameObject.Find("Health").transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = delta / Player.Hero.MaxHealth;
                    yield return null;
                }
                break;
            case UnitBarType.MANA:
                break;
        }
        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedBar(_type, _from, _to, _time));
    }
}
