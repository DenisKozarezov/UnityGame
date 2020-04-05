using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wilberforce.FinalVignette;

public class Interface : MonoBehaviour
{
    public GameObject InterfacePanel;
    public GameObject DefeatPanel;
    public GameObject OptionsPanel;
    public GameObject GameMenuPanel;
    public GameObject BossBar;

    public static GameObject CurrentPanel { set; get; } = null;

    private static FinalVignetteCommandBuffer Vignette;

    private void Start()
    {
        Vignette = Camera.main.GetComponent<FinalVignetteCommandBuffer>();        
    }

    public static bool Hidden { private set; get; } = false;

    private void Update()
    {
        if (Input.GetKeyDown(Options.GameMenu))
        {
            if (!Game.IsDefeat && !GameMenuPanel.activeInHierarchy)
            {
                Open(GameMenuPanel);
            }
            else Close();
        }
    }

    // ФУНКЦИИ ПОЛЬЗОВАТЕЛЬСКОГО ИНТРЕФЕЙСА
    public static void Show(bool _status)
    {
        GameObject.Find("Canvas").GetComponent<Interface>().InterfacePanel.SetActive(_status);
        Hidden = !_status;
    } // Скрыть пользовательский интерфейс
    public void Open(GameObject _panel)
    {
        _panel.transform.SetAsLastSibling();
        _panel.SetActive(true);
        CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
    } // Открыть указанное окно интерфейса
    public void Close(GameObject _panel)
    {
        _panel.transform.SetAsFirstSibling();
        _panel.SetActive(false);
        CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
    }  // Закрыть указанное окно интерфейса
    public void Close()
    {
        if (CurrentPanel != DefeatPanel)
        {
            CurrentPanel.transform.SetAsFirstSibling();
            CurrentPanel.SetActive(false);
            CurrentPanel = GameObject.Find("Canvas").transform.GetChild(GameObject.Find("Canvas").transform.childCount - 1).gameObject;
        }
    } // Закрыть последнее окно интерфейса

    /* ВИНЬЕТИРОВАНИЕ */
    public static void PulseVignette(Color _color, float _frequency, float _time)
    {

    } // Пульсация краёв экрана заданным цветом
    
    public static void VignetteToElement(GameObject _object, float _radius, float _alphaChannel, float _time)
    {
        Vector2 _position = new Vector2(0.5f, 0.5f);

        if (_object.GetComponent<RectTransform>()) _position = _object.GetComponent<RectTransform>().position;
        else if (!_object.GetComponent<RectTransform>()) _position = Camera.main.WorldToScreenPoint(_object.transform.position);

        GameObject.Find("Canvas").GetComponent<Interface>().StartCoroutine(InterpolatedVignetteToObject(_position, _radius, _alphaChannel, _time));
    } // Виньетирование к объекту на экране
    private static IEnumerator InterpolatedVignetteToObject(Vector2 _position, float _radius, float _alphaChannel, float _time)
    {
        float startTime = Time.time;
        Vector2 centerPosition = Vignette.VignetteCenter;
        Vector2 endPosition = new Vector2(_position.x / Screen.width, _position.y / Screen.height + 0.2f);
        
        Color outerColor = Vignette.VignetteOuterColor;
        float falloffLinearity = Vignette.VignetteFalloff;

        float outerDistance = Vignette.VignetteOuterValueDistance;
        float endDistance = _radius / 10;

        while (Vignette.VignetteCenter != endPosition)
        {
            float elapsedTime = Time.time - startTime;

            Vector2 position = Vector2.Lerp(centerPosition, endPosition, elapsedTime / _time);
            Color color = Color.Lerp(outerColor, new Color(outerColor.r, outerColor.g, outerColor.b, _alphaChannel), elapsedTime / _time);
            float falloff = Mathf.Lerp(falloffLinearity, 10, elapsedTime / _time);
            float radius = Mathf.Lerp(outerDistance, endDistance, elapsedTime / _time);

            Vignette.VignetteCenter = position;
            Vignette.VignetteOuterValueDistance = radius;
            Vignette.VignetteFalloff = falloff;
            Vignette.VignetteOuterColor = color;

            yield return null;
        }
        GameObject.Find("Canvas").GetComponent<Interface>().StopCoroutine(InterpolatedVignetteToObject(_position, _radius, _alphaChannel, _time));
    }
    
    public static void ResetVignette()
    {

    } // Сбросить виньетирование
    
}
