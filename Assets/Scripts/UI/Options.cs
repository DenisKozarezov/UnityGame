using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Options : MonoBehaviour
{   
    // СТАНДАРТНЫЕ ЗНАЧЕНИЯ ОПЦИЙ
    private static KeyCode DefaultRight { set; get; } = KeyCode.D;
    private static KeyCode DefaultLeft { set; get; } = KeyCode.A;
    private static KeyCode DefaultJump { set; get; } = KeyCode.W;
    private static KeyCode DefaultInteraction { set; get; } = KeyCode.E;
    private static KeyCode DefaultMeleeAttack { set; get; } = KeyCode.Space;
    private static KeyCode DefaultRangeAttack { set; get; } = KeyCode.LeftShift;
    private static KeyCode DefaultGameMenu { set; get; } = KeyCode.Escape;
    private static bool DefaultTipsOn { set; get; } = true;
    private static float DefaultCameraAttachedDepth { set; get; } = 1.3f;

    // ТЕКУЩИЕ ЗНАЧЕНИЯ ОПЦИЙ
    public static KeyCode Right { set; get; } = DefaultRight;
    public static KeyCode Left { set; get; } = DefaultLeft;
    public static KeyCode Jump { set; get; } = DefaultJump; 
    public static KeyCode Interaction { set; get; } = DefaultInteraction;
    public static KeyCode MeleeAttack { set; get; } = DefaultMeleeAttack;
    public static KeyCode RangeAttack { set; get; } = DefaultRangeAttack;
    public static KeyCode GameMenu { set; get; } = DefaultGameMenu;
    public static bool TipsOn { set; get; } = DefaultTipsOn;
    public static float CameraAttachedDepth { set; get; } = DefaultCameraAttachedDepth;

    /* ------------------------------------ */

    private static bool IsButtonChanged = false;
    private static GameObject changedButton;
    private static KeyCode changedKey = KeyCode.None;
    private static ColorBlock defaultColorBlock;

    public GameObject[] OptionsParameters;

    private void Awake()
    {
        defaultColorBlock.normalColor = Color.white;
        defaultColorBlock.highlightedColor = Color.white;
        defaultColorBlock.pressedColor = Color.grey;
        defaultColorBlock.selectedColor = Color.white;
        defaultColorBlock.colorMultiplier = 1;
    }
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
                changedButton.GetComponent<Button>().colors = defaultColorBlock;
                changedButton = null;
                IsButtonChanged = false;
            }
        }
    }

    public void ChangeKey(GameObject _button)
    {
        if (IsButtonChanged && changedButton != _button)
        {
            changedKey = KeyCode.None;
            changedButton.GetComponent<Button>().colors = defaultColorBlock;
            IsButtonChanged = false;
        }

        changedButton = _button;

        for (int i = 0; i < GameObject.Find("Player Control").transform.GetChild(1).childCount; i++)
        {
            if (OptionsParameters[i] != _button) OptionsParameters[i].GetComponent<Button>().colors = defaultColorBlock;
        }

        ColorBlock _colorBlock = defaultColorBlock;
        _colorBlock.highlightedColor = Color.blue;
        _colorBlock.normalColor = Color.blue;

        _button.GetComponent<Button>().colors = _colorBlock;
        IsButtonChanged = true;
    }
    public void ChangeTipsOn()
    {
        TipsOn = OptionsParameters[7].GetComponent<Toggle>().isOn;
    }
    public void Reset()
    {
        // Сброс переменных
        Right = DefaultRight;
        Left = DefaultLeft;
        Jump = DefaultJump;
        Interaction  = DefaultInteraction;
        MeleeAttack = DefaultMeleeAttack;
        RangeAttack = DefaultRangeAttack;
        GameMenu = DefaultGameMenu;
        TipsOn = DefaultTipsOn;
        CameraAttachedDepth = DefaultCameraAttachedDepth;

        // Сброс клавиш
        OptionsParameters[0].GetComponentInChildren<Text>().text = DefaultRight.ToString();
        OptionsParameters[1].GetComponentInChildren<Text>().text = DefaultLeft.ToString();
        OptionsParameters[2].GetComponentInChildren<Text>().text = DefaultJump.ToString();
        OptionsParameters[3].GetComponentInChildren<Text>().text = DefaultInteraction.ToString();
        OptionsParameters[4].GetComponentInChildren<Text>().text = DefaultMeleeAttack.ToString();
        OptionsParameters[5].GetComponentInChildren<Text>().text = DefaultRangeAttack.ToString();
        OptionsParameters[6].GetComponentInChildren<Text>().text = DefaultGameMenu.ToString();

        // Сброс подсказок
        OptionsParameters[7].GetComponent<Toggle>().isOn = DefaultTipsOn;
    }
    
}
