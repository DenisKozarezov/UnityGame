using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{
    public static float CameraSpeed { private set; get; } = 2f;
    public static float CameraNormalZoom { private set; get; }

    private void Awake()
    {
        CameraNormalZoom = Camera.main.orthographicSize;
    }

    public static void InstanceMoveTo(Vector2 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    }
    public static void SmoothMoveTo(Vector2 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(_direction, _time));
    }
    public static void SmoothMoveTo(Vector3 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(_direction, _time));
    }
    private static IEnumerator InterpolatedMove(Vector3 _direction, float _time)
    {
        float distance = Vector2.Distance(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), _direction);
        while (Camera.main.gameObject.transform.position != _direction)
        {
            float deltaTime = (Time.time - _time);
            yield return new WaitForEndOfFrame();
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.transform.position, _direction, deltaTime / distance);
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_direction, _time));
    }

    public static void CameraZoom(float _zoom, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedZoom(_zoom, _time));
    }
    private static IEnumerator InterpolatedZoom(float _zoom, float _time)
    {
        float current_time = 0;
        while (Camera.main.orthographicSize != _time)
        {
            yield return new WaitForEndOfFrame();
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _zoom, current_time / _time);
            current_time += Time.deltaTime;
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedZoom(_zoom, _time));
    }
}
