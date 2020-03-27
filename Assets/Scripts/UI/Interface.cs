using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public GameObject UIPanel;
    public static bool InterfaceHidden { set; get; } = false;
    private void Update()
    {
        if (!InterfaceHidden && CameraScript.CameraFadeStatus == CameraScript.FadeState.IN)
        {
            HideInterface(true);
        }
    }
    
    public void HideInterface(bool _status)
    {
        UIPanel.SetActive(!_status);
        InterfaceHidden = !_status;        
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
