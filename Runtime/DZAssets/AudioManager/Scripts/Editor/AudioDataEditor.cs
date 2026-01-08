using UnityEditor;
using UnityEngine;
using DZAssets.AudioManager;

// Custom editor for AudioData
[CustomEditor(typeof(AudioData))]
public class AudioDataEditor : Editor
{
    SerializedProperty audioListProp;

    void OnEnable()
    {
        audioListProp = serializedObject.FindProperty("audioList");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Audio List", EditorStyles.boldLabel);

        if (GUILayout.Button("Add Audio Entry"))
        {
            audioListProp.arraySize++;
        }

        float buttonWidth = 22f;
        float spacing = 4f;
        float totalWidth = EditorGUIUtility.currentViewWidth - 40f; // padding for scrollbar/inspector
        float clipWidth = totalWidth * 0.45f;
        float nameWidth = totalWidth - clipWidth - buttonWidth - spacing * 2;

        for (int i = 0; i < audioListProp.arraySize; i++)
        {
            SerializedProperty entry = audioListProp.GetArrayElementAtIndex(i);
            SerializedProperty nameProp = entry.FindPropertyRelative("nameAudio");
            SerializedProperty clipProp = entry.FindPropertyRelative("audioClip");

            EditorGUILayout.BeginHorizontal();
            nameProp.stringValue = EditorGUILayout.TextField(nameProp.stringValue, GUILayout.Width(nameWidth));
            GUILayout.Space(spacing);
            clipProp.objectReferenceValue = EditorGUILayout.ObjectField(clipProp.objectReferenceValue, typeof(AudioClip), false, GUILayout.Width(clipWidth));
            GUILayout.Space(spacing);
            if (GUILayout.Button("X", GUILayout.Width(buttonWidth)))
            {
                audioListProp.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
