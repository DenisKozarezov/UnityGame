using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
public class Sound : MonoBehaviour
{
    public enum PlayerSoundType { 
        HIT,
        DEATH,
        ATTACK,
        MAGIC,
        HEALING,
        WALKING,
        JUMP
    }

    public enum GameEventSoundType
    {
        DEFEAT,
        PAUSE, 
        VICTORY
    }

    public AudioClip Pause;

    public static void Play(PlayerSoundType _sound)
    {
       
    }
    public static void Play(GameEventSoundType _sound)
    {
        var source = new GameObject();
        source.transform.position = Player.Hero.transform.position;
        source.transform.SetParent(GameObject.Find("Sound Manager").transform);
        source.name = "Game Event Sound";
        switch (_sound)
        {
            case GameEventSoundType.PAUSE:
                source.AddComponent<AudioSource>().clip = GameObject.Find("Sound Manager").GetComponent<Sound>().Pause;
                break;
        }
        source.AddComponent<AudioObject>().Play();
    }
}
