using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{   
    // НОРМАЛЬНЫЕ ЗНАЧЕНИЯ КЛАВИШ
    private static KeyCode DefaultRight { set; get; } = KeyCode.D;
    private static KeyCode DefaultLeft { set; get; } = KeyCode.A;
    private static KeyCode DefaultJump { set; get; } = KeyCode.W;
    private static KeyCode DefaultInteraction { set; get; } = KeyCode.E;
    private static KeyCode DefaultMeleeAttack { set; get; } = KeyCode.Space;
    private static KeyCode DefaultRangeAttack { set; get; } = KeyCode.LeftShift;
    private static KeyCode DefaultGameMenu { set; get; } = KeyCode.Escape;

    // ТЕКУЩИЕ ЗНАЧЕНИЯ КЛАВИШ
    public static KeyCode Right { set; get; } = DefaultRight;
    public static KeyCode Left { set; get; } = DefaultLeft;
    public static KeyCode Jump { set; get; } = DefaultJump; 
    public static KeyCode Interaction { set; get; } = DefaultInteraction;
    public static KeyCode MeleeAttack { set; get; } = DefaultMeleeAttack;
    public static KeyCode RangeAttack { set; get; } = DefaultRangeAttack;
    public static KeyCode GameMenu { set; get; } = DefaultGameMenu;

    private static bool IsButtonChanged = false;
    private static GameObject changedButton;
    private static KeyCode changedKey = KeyCode.None;
    private static ColorBlock colorBlock;

    public GameObject[] Buttons;

    private void OnGUI()
    {
        if (Input.anyKeyDown && Event.current.keyCode != KeyCode.None)
        {
            changedKey = Event.current.keyCode;

            if (IsButtonChanged)
            {
                switch (changedButton.name)
                {
                    case "Right Button":
                        Right = changedKey;
                        break;
                    case "Left Button":
                        Left = changedKey;
                        break;
                    case "Jump Button":
                        Jump = changedKey;
                        break;                    
                    case "Interaction Button":
                        Interaction = changedKey;
                        break;
                    case "Melee Attack Button":
                        MeleeAttack = changedKey;
                        break;
                    case "Range Attack Button":
                        RangeAttack = changedKey;
                        break;
                    case "Game Menu Button":
                        GameMenu = changedKey;
                        break;
                }
                changedButton.GetComponentInChildren<Text>().text = changedKey.ToString();
                
                changedKey = KeyCode.None;
                changedButton.GetComponent<Button>().colors = colorBlock;
                changedButton = null;
                IsButtonChanged = false;
            }
        }
    }

    public void ChangeKey(GameObject _button)
    {    
        changedButton = _button;
        colorBlock = _button.GetComponent<Button>().colors;
        
        foreach (GameObject button in Buttons)
        {
            if (button != _button) button.GetComponent<Button>().colors = colorBlock;
        }

        ColorBlock _colorBlock = colorBlock;
        _colorBlock.highlightedColor = Color.blue;
        _colorBlock.normalColor = Color.blue;

        _button.GetComponent<Button>().colors = _colorBlock;
         IsButtonChanged = true;
    }
    public void Open()
    {
        GameObject.Find("Canvas").GetComponent<Interface>().OptionsPanel.transform.SetAsLastSibling();
        GameObject.Find("Canvas").GetComponent<Interface>().OptionsPanel.SetActive(true);
    }
    public void Close()
    {
        GameObject.Find("Canvas").GetComponent<Interface>().OptionsPanel.SetActive(false);
        GameObject.Find("Canvas").GetComponent<Interface>().OptionsPanel.transform.SetAsFirstSibling();
    }
    public void Reset()
    {
        Right = DefaultRight;
        Left = DefaultLeft;
        Jump = DefaultJump;
        Interaction  = DefaultInteraction;
        MeleeAttack = DefaultMeleeAttack;
        RangeAttack = DefaultRangeAttack;
        GameMenu = DefaultGameMenu;

        Buttons[0].GetComponentInChildren<Text>().text = DefaultRight.ToString();
        Buttons[1].GetComponentInChildren<Text>().text = DefaultLeft.ToString();
        Buttons[2].GetComponentInChildren<Text>().text = DefaultJump.ToString();
        Buttons[3].GetComponentInChildren<Text>().text = DefaultInteraction.ToString();
        Buttons[4].GetComponentInChildren<Text>().text = DefaultMeleeAttack.ToString();
        Buttons[5].GetComponentInChildren<Text>().text = DefaultRangeAttack.ToString();
        Buttons[6].GetComponentInChildren<Text>().text = DefaultGameMenu.ToString();
    }
}
