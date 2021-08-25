using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Windows;
using System;

[CustomEditor(typeof(AugmentManager), true)]
public class AugmentManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AugmentManager augmentManager = (AugmentManager)target;

        GUI.backgroundColor = new Color(227/255.0f, 36/255.0f, 43/255.0f);
        if (GUILayout.Button("Import Augments") && 
            EditorUtility.DisplayDialog(
                "Confirmation", 
                "Are you sure to import all the augment data from the excel file? " +
                "This will override all the augment scriptable objests.", 
                "Yes", "No"))
        {
            string synergyDirectory = $"Assets/Resources/Data/Synergies";
            string augmentDirectory = $"Assets/Resources/Data/Augments";

            if (Directory.Exists(synergyDirectory)) { Directory.Delete(synergyDirectory); }
            Directory.CreateDirectory(synergyDirectory);

            if (Directory.Exists(augmentDirectory)) { Directory.Delete(augmentDirectory); }
            Directory.CreateDirectory(augmentDirectory);

            augmentManager.Init();
            for (int i = 0; i < augmentManager.synergyDatas.Count; i++)
            {
                AssetDatabase.CreateAsset(augmentManager.synergyDatas[i], $"{synergyDirectory}/{augmentManager.synergyDatas[i].id}.{augmentManager.synergyDatas[i].title}.asset");
            }

            for (int i = 0; i < augmentManager.augmentDatas.Count; i++)
            {
                AssetDatabase.CreateAsset(augmentManager.augmentDatas[i], $"{augmentDirectory}/{augmentManager.augmentDatas[i].id}.{augmentManager.augmentDatas[i].title}.asset");
            }
        }
        GUI.backgroundColor = Color.white;

        GUI.backgroundColor = Color.blue;
        if (GUILayout.Button("Save Augments"))
        {
            augmentManager.WriteExcel("AugmentList");
        }
        GUI.backgroundColor = Color.white;

    }
}


[CustomEditor(typeof(AugmentData), true)]
public class AugmentDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.GetIterator();

        if (prop.NextVisible(true))
        {
            do
            {
                if (prop.name == "descriptions")
                {
                    SerializedProperty arrayProp = serializedObject.FindProperty("descriptions");
                    SerializedProperty codesProp = serializedObject.FindProperty("codes");


                    arrayProp.isExpanded = EditorGUILayout.Foldout(arrayProp.isExpanded, "Array");
                    if (arrayProp.isExpanded)
                    {
                        //AugmentData data = target as AugmentData;
                        arrayProp.arraySize = 3;
                        codesProp.arraySize = 3;
                        //EditorGUILayout.PropertyField(arrayProp.FindPropertyRelative("Array.size"));

                        serializedObject.ApplyModifiedProperties();
                        //Array.Resize<string>(ref data.descriptions., arrayProp.arraySize);

                        for (int i = 0; i < arrayProp.arraySize; ++i)
                        {
                            SerializedProperty elementProp = arrayProp.GetArrayElementAtIndex(i);
                            EditorGUILayout.PropertyField(elementProp, new GUIContent("Description " + i));

                            GUIStyle style = new GUIStyle(GUI.skin.textArea);
                            style.richText = true;

                            GUI.enabled = false;
                            EditorGUILayout.TextArea(elementProp.stringValue, style);
                            GUI.enabled = true;

                            SerializedProperty codeProp = codesProp.GetArrayElementAtIndex(i);
                            EditorGUILayout.PropertyField(codeProp, new GUIContent("Code " + i));
                        }
                    }
                }
                else if (prop.name == "codes") { }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }

            }
            while (prop.NextVisible(false));
            serializedObject.ApplyModifiedProperties();
        }
    }

}

[CustomEditor(typeof(SynergyData), true)]
public class SynergyDataEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.GetIterator();

        if (prop.NextVisible(true))
        {
            do
            {
                if (prop.name == "descriptions")
                {
                    //ArrayGUI(serializedObject, "descriptions");
                    SerializedProperty descriptionsProp = serializedObject.FindProperty("descriptions");
                    SerializedProperty codesProp = serializedObject.FindProperty("codes");
                    SerializedProperty breakPointsProp = serializedObject.FindProperty("breakpoints");
                    descriptionsProp.isExpanded = EditorGUILayout.Foldout(descriptionsProp.isExpanded, "Array");
                    if (descriptionsProp.isExpanded)
                    {
                        descriptionsProp.arraySize = breakPointsProp.arraySize + 1;
                        codesProp.arraySize = descriptionsProp.arraySize > 1 ? descriptionsProp.arraySize - 1 : 0;

                        serializedObject.ApplyModifiedProperties();

                        for (int i = 0; i < descriptionsProp.arraySize; ++i)
                        {
                            SerializedProperty descriptionProp = descriptionsProp.GetArrayElementAtIndex(i);
                            EditorGUILayout.PropertyField(descriptionProp, new GUIContent("Description " + i), true);

                            GUIStyle style = new GUIStyle(GUI.skin.textArea);
                            style.richText = true;

                            GUI.enabled = false;
                            EditorGUILayout.TextArea(descriptionProp.stringValue, style);
                            GUI.enabled = true;

                            if (i != 0)
                            {
                                SerializedProperty codeProp = codesProp.GetArrayElementAtIndex(i - 1);
                                EditorGUILayout.PropertyField(codeProp, new GUIContent("Code " + i), true);
                            }
                        }
                    }
                }
                else if (prop.name == "codes"){ }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(prop.name), true);
                }

            }
            while (prop.NextVisible(false));
            serializedObject.ApplyModifiedProperties();
        }
    }
}