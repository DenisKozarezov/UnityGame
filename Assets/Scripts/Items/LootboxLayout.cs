using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Scripting.Pipeline;

[CustomEditor(typeof(Lootbox))]
public class Lootboxyout : Editor
{
    Lootbox Target;
    AnimBool itemsFoldoutValue;
    Vector2 scrollPos;

    GUIStyle buttonStyle = new GUIStyle();
    bool AddItem;
    string AddItemName = "Добавить предмет";

    const int ItemHeight = 30;
    private void OnEnable()
    {
        Target = (Lootbox)target;
        itemsFoldoutValue = new AnimBool(false);
        itemsFoldoutValue.valueChanged.AddListener(Repaint);

        buttonStyle.normal.background = Texture2D.whiteTexture;
        buttonStyle.normal.textColor = Color.black;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
    }

    public override void OnInspectorGUI()
    {
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.background = Texture2D.grayTexture;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.normal.textColor = Color.white;

        GUIStyle textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.black;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.alignment = TextAnchor.MiddleCenter;

        Target.ItemsFoldout = EditorGUILayout.Foldout(Target.ItemsFoldout, "Предметы");
        itemsFoldoutValue.target = Target.ItemsFoldout;
        if (EditorGUILayout.BeginFadeGroup(itemsFoldoutValue.faded))
        {
            if (Target.Items.Capacity > 1)
            {
                EditorGUILayout.LabelField("", labelStyle, GUILayout.Height(ItemHeight * Target.Items.Count));
            }
            else
            {
                EditorGUILayout.LabelField("Пусто", labelStyle, GUILayout.Height(ItemHeight));
            }

            if (GUILayout.Button("Добавить предмет"))
            {
                if (!AddItem)
                {
                    AddItem = true;
                    buttonStyle.normal.background = Texture2D.grayTexture;
                    buttonStyle.normal.textColor = Color.white;
                }
                else
                {
                    AddItem = false;
                    buttonStyle.normal.background = Texture2D.whiteTexture;
                    buttonStyle.normal.textColor = Color.black;
                }
            }

            if (AddItem)
            {
                EditorGUILayout.Space(20);
                EditorGUILayout.BeginVertical();
                if (GameData.Items.Count > 0)
                {
                    EditorGUILayout.LabelField("Выберите предмет", textStyle);
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(ItemHeight * GameData.Items.Count));
                    for (int i = 0; i < GameData.Items.Count; i++)
                    {                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("[" + i + "]");
                        if (GUILayout.Button(GameData.Items[i].Name))
                        {
                            
                        }
                        EditorGUILayout.LabelField("Вероятность" + GameData.Items[i].Probability);
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView();
                }
                else EditorGUILayout.HelpBox("В базе данных отсутствуют предметы. Для продолжения добавьте предмет в Tools/Game Data.", MessageType.Warning);              
                EditorGUILayout.EndVertical();
            }
        }
        EditorGUILayout.EndFadeGroup();
    }
}
