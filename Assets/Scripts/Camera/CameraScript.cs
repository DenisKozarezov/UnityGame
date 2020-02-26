using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraScript))]
public class CameraScript : MonoBehaviour
{
    private static float CameraSpeed { set; get; } = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void InstanceMoveTo(Vector2 _direction)
    {
        Vector3 _camera_pos = Camera.main.gameObject.transform.position;
        Camera.main.gameObject.transform.position = Vector3.Lerp(_camera_pos, new Vector3(_direction.x, _direction.y, _camera_pos.z), 1);
    }
    public static void SmoothMoveTo(Vector2 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(SmoothMove(_direction, _time));
    }
    public static void SmoothMoveTo(Vector3 _direction, float _time)
    {
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(SmoothMove(_direction, _time));
    }

    private static IEnumerator SmoothMove(Vector3 _direction, float _time)
    {
        float distance = Vector2.Distance(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y), _direction);
        while (Camera.main.gameObject.transform.position != _direction)
        {
            float deltaTime = (Time.time - _time);
            yield return new WaitForEndOfFrame();
            Camera.main.gameObject.transform.position = Vector3.Lerp(Camera.main.transform.position, _direction, deltaTime / distance);
        }
        Camera.main.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(SmoothMove(_direction, _time));
    }
}
