using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wilberforce.FinalVignette;
using UnityEngine.Rendering;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{
    private void Awake()
    {
        NormalZoom = Camera.main.orthographicSize;
        Vignette = Camera.main.GetComponent<FinalVignetteCommandBuffer>();
    }

    /* ПЕРЕДВИЖЕНИЕ КАМЕРЫ */
    public static void InstanceMoveTo(Vector3 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    } // Мгновенное перемещение к точке
    public static void SmoothMoveTo(Vector3 _direction, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(new Vector3(_direction.x, _direction.y, Camera.main.gameObject.transform.position.z), _time));
    } // Плавное перемещение к точке
    public static void SmoothMoveTo(Unit _target, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(_target, _time));
    } // Плавное перемещение к юниту
    private static IEnumerator InterpolatedMove(Vector3 _direction, float _time)
    {
        float startTime = Time.time;
        Vector3 startPosition = Camera.main.transform.localPosition;
        Vector3 endPosition = _direction;
        while (Camera.main.transform.position != endPosition)
        {
            float elapsedTime = Time.time - startTime;
            Camera.main.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _time);
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_direction, _time));
    }
    private static IEnumerator InterpolatedMove(Unit _target, float _time)
    {
        float startTime = Time.time;
        Vector3 startPosition = Camera.main.transform.localPosition;
        Vector3 endPosition = new Vector3(0, Options.CameraAttachedDepth, Camera.main.transform.localPosition.z);
        while (Camera.main.transform.position != endPosition)
        {
            if (IsDragging) break;

            float elapsedTime = Time.time - startTime;
            Camera.main.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _time);
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_target, _time));
    }


    /* ПРИВЯЗКА КАМЕРЫ К ЮНИТУ */
    public static Unit AttachedTarget { private set; get; } // Цель, к которой привязана камера.    
    public static bool IsDragging { set; get; } = false; // Двигает ли игрок камеру
    public static bool CanDrag { set; get; } = true; // Может ли игрок двигать камеру
    public enum CameraAttachmentType { VERTICAL, HORIZONTAL, BOTH, CENTER } // Оси привязки
    public static CameraAttachmentType AttachmentType { private set; get; } = CameraAttachmentType.HORIZONTAL;
    public static void AttachToUnit(Unit _target, CameraAttachmentType _attachmentType)
    {
        AttachedTarget = _target;        
        AttachmentType = _attachmentType;
        Camera.main.transform.SetParent(AttachedTarget.transform);
        Camera.main.gameObject.transform.localPosition = new Vector3(0, Options.CameraAttachedDepth, Camera.main.transform.localPosition.z);
    }
    public static void Detach()
    {
        AttachedTarget = null;
        Camera.main.transform.SetParent(GameObject.Find("Level").transform.parent);
    }
    public static void Drag()
    {
        if (AttachedTarget != null && CanDrag)
        {
            IsDragging = true;
            float deltaX = 0, deltaY = 0;
            if (Mathf.Abs(Input.GetAxis("Mouse X")) >= Options.CameraAttachedThreshold) deltaX = Input.GetAxis("Mouse X") * Options.CameraAttachedSpeed * Time.deltaTime;
            if (Mathf.Abs(Input.GetAxis("Mouse Y")) >= Options.CameraAttachedThreshold) deltaY = Input.GetAxis("Mouse Y") * Options.CameraAttachedSpeed * Time.deltaTime;
            switch (AttachmentType)
            {
                case CameraAttachmentType.HORIZONTAL:
                    if (Mathf.Abs(Camera.main.gameObject.transform.localPosition.x + deltaX) <= 1) Camera.main.gameObject.transform.localPosition += new Vector3(deltaX, 0, 0);
                    break;
                case CameraAttachmentType.VERTICAL:
                    if (Mathf.Abs(Camera.main.gameObject.transform.localPosition.y + deltaY) <= 1 + Options.CameraAttachedDepth) Camera.main.gameObject.transform.localPosition += new Vector3(0, deltaY, 0);
                    break;
                case CameraAttachmentType.BOTH:
                    if (Mathf.Abs(Camera.main.gameObject.transform.localPosition.x + deltaX) <= 1) Camera.main.gameObject.transform.localPosition += new Vector3(deltaX, 0, 0);
                    if (Mathf.Abs(Camera.main.gameObject.transform.localPosition.y + deltaY) <= 1 + Options.CameraAttachedDepth) Camera.main.gameObject.transform.localPosition += new Vector3(0, deltaY, 0);
                    break;
            }
        }        
    }

    
    /* ОТДАЛЕНИЕ/ПРИБЛИЖЕНИЕ КАМЕРЫ НА ВЕЛИЧИНУ _zoom */
    public static float NormalZoom { private set; get; } // Стандартное значение приближения на старте игры
    public static float CurrentZoom { private set; get; } // Текущее приближение камеры
    public static void Zoom(float _zoom, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedZoom(_zoom, _time));
    } // Приблизить к величине _zoom
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
        Color startVignetteColor = Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor;
        switch (_fadeState)
        {
            case FadeState.IN:                
                while (Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor != Color.black)
                {
                    float elapsedTime = Time.time - startTime;                    
                    Color newColor = Color.Lerp(startVignetteColor, Color.black, elapsedTime / _time);                    
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
    public static void Fade(float _fraction, float _time)
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedFade(_fraction, _time));
    }
    private static IEnumerator InterpolatedFade(float _fraction, float _time)
    {
        float startTime = Time.time;
        Color startVignetteColor = Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor;
        Color endVignetteColor = new Color(startVignetteColor.r, startVignetteColor.g, startVignetteColor.b, _fraction);
        while (Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor != endVignetteColor)
        {
            float elapsedTime = Time.time - startTime;
            Color newColor = Color.Lerp(startVignetteColor, endVignetteColor, elapsedTime / _time);
            Camera.main.GetComponent<Wilberforce.FinalVignette.FinalVignetteCommandBuffer>().VignetteInnerColor = newColor;
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedFade(_fraction, _time));
    }

    /* ВИНЬЕТИРОВАНИЕ */
    private static FinalVignetteCommandBuffer Vignette;
    private static Color VignetteColor;
    public static void PulseVignette(Color _color, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(PulseCoroutine(_color, _time));
    } // Пульсация краёв экрана заданным цветом
    private static IEnumerator PulseCoroutine(Color _color, float _time)
    {
        Color vignetteStartColor = Vignette.VignetteInnerColor;
        Color vignetteEndColor = _color;
        vignetteEndColor.a = 0;
        VignetteColor = vignetteStartColor;
        float startTime = Time.time;

        while (Vignette.VignetteInnerColor != vignetteEndColor)
        {
            float elapsedTime = Time.time - startTime;
            Vignette.VignetteInnerColor = Color.Lerp(vignetteStartColor, vignetteEndColor, elapsedTime / _time);
            yield return null;
        }

        while (Vignette.VignetteInnerColor != VignetteColor)
        {
            float elapsedTime = Time.time - startTime;
            Vignette.VignetteInnerColor = Color.Lerp(vignetteStartColor, VignetteColor, elapsedTime / _time);
            yield return null;
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(PulseCoroutine(_color, _time));
    }
    private static void ResetPulseVignette()
    {
        Color vignetteStartColor = Vignette.VignetteInnerColor;
        float startTime = Time.time;
        while (Vignette.VignetteInnerColor != VignetteColor)
        {
            float elapsedTime = Time.time - startTime;
            Vignette.VignetteInnerColor = Color.Lerp(vignetteStartColor, VignetteColor, elapsedTime);
        }
    }
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
        ResetPulseVignette();
    } // Сбросить виньетирование
}
