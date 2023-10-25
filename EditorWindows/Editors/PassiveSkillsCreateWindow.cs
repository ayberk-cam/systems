using UnityEngine;
using UnityEditor;

public class PassiveSkillsCreateWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;

    protected PassiveSkillBase[] passiveSkills;
    public PassiveSkillBase newSkill;

    Vector2 scrollPosition = Vector2.zero;

    private void OnGUI()
    {
        //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);

        serializedObject = new SerializedObject(newSkill);
        serializedProperty = serializedObject.GetIterator();
        serializedProperty.NextVisible(true);
        DrawProperties(serializedProperty);

        if (GUILayout.Button("Save"))
        {
            passiveSkills = GetAllInstances<PassiveSkillBase>();

            if (newSkill.BaseName == null)
            {
                newSkill.BaseName = "PassiveSkill" + (passiveSkills.Length + 1);
            }

            AssetDatabase.CreateAsset(newSkill, "Assets/Design/Skills/PassiveSkills/" + newSkill.BaseName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        EditorGUILayout.EndScrollView();

        Apply();
    }

    protected void DrawProperties(SerializedProperty p)
    {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }

    public static T[] GetAllInstances<T>() where T : PassiveSkillBase
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}