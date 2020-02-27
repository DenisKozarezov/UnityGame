using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{
    public static float normalZoom { private set; get; }
    public static float currentZoom { private set; get; }

    private void Awake()
    {
        normalZoom = Camera.main.orthographicSize;
    }

    public static void InstanceMoveTo(Vector2 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    }
    public static void InstanceMoveTo(Vector3 _direction)
    {
        Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.transform.position, _direction, 1);
    }
    public static void SmoothMoveTo(Vector2 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(new Vector3(_direction.x, _direction.y, Camera.main.gameObject.transform.position.z), _time));
    }
    public static void SmoothMoveTo(Vector3 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedMove(_direction, _time));
    }
    private static IEnumerator InterpolatedMove(Vector3 _direction, float _time)
    {
        float distance = Vector2.Distance(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), _direction);
        float startTime = Time.time;
        while (Camera.main.transform.position != _direction)
        {
            float coveredDistance = (Time.time - startTime) / _time;
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.transform.position, _direction, coveredDistance / distance);
            yield return null;
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedMove(_direction, _time));
    }

    public static void Zoom(float _zoom, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(InterpolatedZoom(_zoom, _time));
    }
    private static IEnumerator InterpolatedZoom(float _zoom, float _time)
    {
        float startTime = Time.time;
        float distance = Mathf.Abs(Camera.main.orthographicSize - _zoom);
        while (Camera.main.orthographicSize != _zoom)
        {
            float coveredDistance = (Time.time - startTime) / _time;
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, _zoom, coveredDistance / distance);
            currentZoom = Camera.main.orthographicSize;
            yield return null;
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(InterpolatedZoom(_zoom, _time));
    }
}
