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
        if (AttachedTarget != null && CanDrag)
        {
            if (Input.GetMouseButton(1))
            {
                IsDragged = true;
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
            else if (Input.GetMouseButtonUp(1))
            {
                IsDragged = false;
                SmoothMoveTo(AttachedTarget, 0.4f);
            }            
        }
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
            if (IsDragged) break;

            float elapsedTime = Time.time - startTime;
            Camera.main.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / _time);
            yield return null;
        }
        Camera.main.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_target, _time));
    }


    /* ПРИВЯЗКА КАМЕРЫ К ЮНИТУ */
    public static Unit AttachedTarget { private set; get; } // Цель, к которой привязана камера.    
    private static bool IsDragged { set; get; } = false; // Двигает ли игрок камеру
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
}
