using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public static void Open()
    {
        GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.transform.SetAsLastSibling();
        GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.SetActive(true);
    }

    public static void Close()
    {
        GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.transform.SetAsFirstSibling();
        GameObject.Find("Canvas").GetComponent<Interface>().GameMenuPanel.SetActive(false);
    }
}
