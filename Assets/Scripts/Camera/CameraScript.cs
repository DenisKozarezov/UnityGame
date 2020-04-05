using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{    
    private void Awake()
    {
        NormalZoom = Camera.main.orthographicSize;
    }
    private void Update()
    {
        if (AttachedTarget != null)
        {
            switch (AttachmentType)
            {
                case CameraAttachmentType.HORIZONTAL:
                    Camera.main.gameObject.transform.position = new Vector3(AttachedTarget.transform.position.x, attachedPosition.y, attachedPosition.z);
                    break;
                case CameraAttachmentType.VERTICAL:
                    Camera.main.gameObject.transform.position = new Vector3(attachedPosition.x, AttachedTarget.transform.position.y + attachedPosition.y, attachedPosition.z);
                    break;
            }
        }
    }
    
    
    /* ПЕРЕДВИЖЕНИЕ КАМЕРЫ */
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


    /* ПРИВЯЗКА КАМЕРЫ К ЮНИТУ */
    public static Unit AttachedTarget { private set; get; } // Цель, к которой привязана камера.    
    public static float AttachedDepth { private set; get; } = Options.CameraAttachedDepth; // Доп. высота Y отн. цели
    private static Vector3 attachedPosition { set; get; }
    public enum CameraAttachmentType { VERTICAL, HORIZONTAL, BOTH } // Оси привязки
    public static CameraAttachmentType AttachmentType { private set; get; } = CameraAttachmentType.HORIZONTAL;
    public static void AttachToUnit(Unit _target, CameraAttachmentType _attachmentType)
    {
        AttachedTarget = _target;        
        AttachmentType = _attachmentType;

        switch (_attachmentType)
        {
            case CameraAttachmentType.BOTH:
                attachedPosition = new Vector3(0, AttachedDepth, Camera.main.transform.position.z);
                Camera.main.transform.SetParent(AttachedTarget.transform);
                Camera.main.gameObject.transform.localPosition = attachedPosition;
                break;
            case CameraAttachmentType.HORIZONTAL:
                attachedPosition = new Vector3(0, Camera.main.gameObject.transform.position.y + AttachedDepth, Camera.main.gameObject.transform.position.z);
                break;
            case CameraAttachmentType.VERTICAL:
                attachedPosition = new Vector3(Camera.main.gameObject.transform.position.x, AttachedDepth, Camera.main.gameObject.transform.position.z);
                break;
        }
    }
    public static void Detach()
    {
        AttachedTarget = null;
        attachedPosition = Vector3.zero;
        Camera.main.transform.SetParent(GameObject.Find("Level").transform.parent);
    }

    
    /* ОТДАЛЕНИЕ/ПРИБЛИЖЕНИЕ КАМЕРЫ НА ВЕЛИЧИНУ _zoom */
    public static float NormalZoom { private set; get; } // Стандартное значение приближения при старте игры
    public static float CurrentZoom { private set; get; } // Текущее приближение камеры
    public static void Zoom(float _zoom, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedZoom(_zoom, _time));
    }
    private static IEnumerator InterpolatedZoom(float _zoom, float _time)
    {
        float startTime = Time.time;
        float startZoom = Camera.main.orthographicSize;
        while (Camera.main.orthographicSize != _zoom)
        {
            float elapsedTime = Time.time - startTime;
            Camera.main.orthographicSize = Mathf.Lerp(startZoom, _zoom, elapsedTime / _time);
            CurrentZoom = Camera.main.orthographicSize;
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedZoom(_zoom, _time));
    }

    
    /* ЗАТУХАНИЕ/ПОЯВЛЕНИЕ ЭКРАНА */
    public enum FadeState { IN, OUT, NONE } // Состояния экрана: { ЧЁРНЫЙ ЭКРАН, ПОЯВЛЕНИЕ, ОБЫЧНОЕ }
    public static FadeState CameraFadeStatus { private set; get; } = FadeState.NONE;
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
                while (Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor != Color.black)
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
