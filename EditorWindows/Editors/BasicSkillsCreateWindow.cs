using UnityEngine;
using UnityEditor;

public class BasicSkillsCreateWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;

    protected BasicSkillBase[] basicSkills;
    public BasicSkillBase newSkill;

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
            basicSkills = GetAllInstances<BasicSkillBase>();

            if (newSkill.BaseName == null)
            {
                newSkill.BaseName = "BasicSkill" + (basicSkills.Length + 1);
            }

            AssetDatabase.CreateAsset(newSkill, "Assets/Design/Skills/BasicSkills/" + newSkill.BaseName + ".asset");
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

    public static T[] GetAllInstances<T>() where T : BasicSkillBase
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