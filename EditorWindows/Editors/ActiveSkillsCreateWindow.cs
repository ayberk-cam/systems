using UnityEngine;
using UnityEditor;

public class ActiveSkillsCreateWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;

    protected ActiveSkillBase[] activeSkills;
    public ActiveSkillBase newSkill;

    Vector2 scrollPosition;

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
            activeSkills = GetAllInstances<ActiveSkillBase>();

            if (newSkill.BaseName == null)
            {
                newSkill.BaseName = "ActiveSkill" + (activeSkills.Length + 1);
            }

            AssetDatabase.CreateAsset(newSkill, "Assets/Design/Skills/ActiveSkills/" + newSkill.BaseName + ".asset");
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

    public static T[] GetAllInstances<T>() where T : ActiveSkillBase
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