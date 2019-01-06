using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(BattleCard))]
public class CardInspector : Editor
{

    private BattleCard _card;
    private ReorderableList _bonusesList;

    private void OnEnable()
    {
        _card = (BattleCard)target;

        _bonusesList = new ReorderableList(serializedObject, serializedObject.FindProperty("Resources"));

        _bonusesList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "resources");
        };

        
        _bonusesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            _card.Resources[index].Resource = (CardStats.Resources)EditorGUI.EnumPopup(new Rect(rect.position, new Vector2(rect.width/2-2, rect.height)), _card.Resources[index].Resource);
            _card.Resources[index].Value = EditorGUI.IntSlider(new Rect(rect.position+Vector2.right* (rect.width/2), new Vector2(rect.width/2 - 5, rect.height)), _card.Resources[index].Value, 0, 5);
        };
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.BeginHorizontal();
        _card.Id = EditorGUILayout.IntField(_card.Id, GUILayout.Width(100f));
        _card.CardName = EditorGUILayout.TextField(_card.CardName);
        EditorGUILayout.EndHorizontal();
        _card.Image = (Sprite)EditorGUILayout.ObjectField(_card.Image, typeof(Sprite), false, GUILayout.Width(150), GUILayout.Height(150));
        _card.CardType = (CardStats.CardType)EditorGUILayout.EnumPopup("CardType", _card.CardType); 
        _card.Description = EditorGUILayout.TextArea(_card.Description, GUILayout.Height(50));

        _bonusesList.DoLayoutList();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_card);
            serializedObject.ApplyModifiedProperties();
        }
    }
}

public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        _property.intValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
    }
}
