using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{
    public static float normalZoom { private set; get; } // Нормальное приближение при старте игры
    public static float currentZoom { private set; get; } // Текущее приближение камеры
    
    /* Привязка камеры к юниту */
    public static Unit currentTarget { private set; get; } // Юнит, к которому привязана камера.    
    public static float attachedDepth { private set; get; } = 0f; // Доп. глубина Y' отн. Yцентр.
    public static float attachedY { private set; get; } // Результат привязки: Yцентр. - attachedDepth

    /* Состояние камеры: ЗАТУХАНИЕ, ПОЯВЛЕНИЕ */
    public enum FadeState { IN, OUT }
    public static FadeState CameraFadeStatus { private set; get; } = FadeState.OUT;
    
    private void Awake()
    {
        normalZoom = Camera.main.orthographicSize;
    }
    private void Update()
    {
        if (currentTarget != null) attachedY = currentTarget.gameObject.transform.position.y - attachedDepth;
    }
    
    /* Передвижение камеры */
    public static void InstanceMoveTo(Vector2 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    }
    public static void InstanceMoveTo(Vector3 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    }
    public static void SmoothMoveTo(Vector2 _direction, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(new Vector3(_direction.x, _direction.y, Camera.main.gameObject.transform.position.z), _time));
    }
    public static void SmoothMoveTo(Vector3 _direction, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(new Vector3(_direction.x, _direction.y, Camera.main.gameObject.transform.position.z), _time));
    }
    private static IEnumerator InterpolatedMove(Vector3 _direction, float _time)
    {
        float startTime = Time.time;        
        while (Camera.main.transform.position != _direction)
        {
            float elapsedTime = Time.time - startTime;
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.transform.position, _direction, elapsedTime / _time);
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_direction, _time));
    }

    public static void AttachToUnit(Unit _target)
    {
        currentTarget = _target;
        CameraScript.InstanceMoveTo(new Vector2(currentTarget.gameObject.transform.position.x, currentTarget.gameObject.transform.position.y) - new Vector2(0, attachedY));
    }

    /* Отдаление/приближение камеры на величину _zoom */
    public static void Zoom(float _zoom, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedZoom(_zoom, _time));
    }
    private static IEnumerator InterpolatedZoom(float _zoom, float _time)
    {
        float startTime = Time.time;
        float distance = Camera.main.orthographicSize;
        while (Camera.main.orthographicSize != _zoom)
        {
            float elapsedTime = Time.time - startTime;
            Camera.main.orthographicSize = Mathf.Lerp(distance, _zoom, elapsedTime / _time);
            currentZoom = Camera.main.orthographicSize;
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedZoom(_zoom, _time));
    }

    /* Затухание/появление камеры */
    public static void Fade(FadeState _fadeState, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedFade(_fadeState, _time));
    }
    private static IEnumerator InterpolatedFade(FadeState _fadeState, float _time)
    {
        float startTime = Time.time;
        Color vignetteColor = Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor;
        switch (_fadeState)
        {
            case FadeState.IN:                
                while (Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteOuterColor != Color.black)
                {
                    float elapsedTime = Time.time - startTime;                    
                    Color newColor = Color.Lerp(vignetteColor, Color.black, elapsedTime / _time);                    
                    Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor = newColor;
                    yield return null;
                }
                CameraFadeStatus = FadeState.IN;
                break;
            case FadeState.OUT:
                CameraFadeStatus = FadeState.OUT;
                break;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedFade(_fadeState, _time));
    }
}
