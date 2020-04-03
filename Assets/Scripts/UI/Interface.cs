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
    public GameObject BossBar;

    public static bool Hidden { private set; get; } = false;
    
    public static void Hide(bool _status)
    {
        GameObject.Find("Canvas").GetComponent<Interface>().InterfacePanel.SetActive(!_status);
        Hidden = !_status;        
    }

    // ВИНЬЕТИРОВАНИЕ
    public static void PulseVignette(Color _color, float _frequency, float _time)
    {

    } // Пульсация краёв экрана цветом
    public static void VignetteToElement(GameObject _object)
    {

    } // Виньетирование к элементу интерфейса
    public static void ResetVignette()
    {

    } // Сбросить виньетирование
    private IEnumerator InterpolatedVignette(Vector2 _position)
    {
        yield return null;
    }
}
