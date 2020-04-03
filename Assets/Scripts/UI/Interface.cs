using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public GameObject InterfacePanel;
    public GameObject DefeatPanel;
    public GameObject OptionsPanel;
    public GameObject GameMenuPanel;

    public static bool Hidden { set; get; } = false;
    
    public static bool HealthFrozen = false;
    public static bool ManaFrozen = false;

    private void Update()
    {
        if (!HealthFrozen && !Player.Hero.IsDead) GameObject.Find("Health").GetComponentInChildren<Image>().fillAmount = Player.Hero.Health / Player.Hero.MaxHealth;
        //GameObject.Find("Mana").GetComponent<Slider>().value = Player.Hero.Mana / Player.Hero.MaxMana;
    }
    
    public void Hide(bool _status)
    {
        InterfacePanel.SetActive(!_status);
        Hidden = !_status;        
    }

    public enum UnitBarType { HEALTH, MANA }
    public static void UpdateBar(UnitBarType _type, float _from, float _to, float _time)
    {
        if (_type == UnitBarType.HEALTH) HealthFrozen = true;
        else ManaFrozen = true;

        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedBar(_type, _from, _to, _time));
    }
    public static IEnumerator InterpolatedBar(UnitBarType _type, float _from, float _to, float _time)
    {
        float startTime = Time.time;
        switch (_type)
        {
            case UnitBarType.HEALTH:
                while (GameObject.Find("Health").GetComponent<Image>().fillAmount != _to / Player.Hero.MaxHealth)
                {
                    float elapsedTime = Time.time - startTime;
                    float delta = Mathf.Lerp(_from, _to, elapsedTime / _time);
                    GameObject.Find("Health").GetComponent<Image>().fillAmount = delta / Player.Hero.MaxHealth;
                    yield return null;
                }
                HealthFrozen = false;                
                break;
            case UnitBarType.MANA:
                while (GameObject.Find("Mana").GetComponent<Image>().fillAmount != _to / Player.Hero.MaxMana)
                {
                    float elapsedTime = Time.time - startTime;
                    float delta = Mathf.Lerp(_from, _to, elapsedTime / _time);
                    GameObject.Find("Mana").GetComponent<Image>().fillAmount = delta / Player.Hero.MaxMana;
                    yield return null;
                }
                break;
        }
        GameObject.Find("Canvas").GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedBar(_type, _from, _to, _time));
    }
}
