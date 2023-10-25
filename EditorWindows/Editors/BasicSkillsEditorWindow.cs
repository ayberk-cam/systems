using UnityEngine;
using UnityEditor;

public class BasicSkillsEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected BasicSkillBase[] basicSkills;
    protected string selectedPropertyPach;
    protected string selectedProperty;

    Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Maestro/Design/Skills/BasicSkills")]
    protected static void ShowWindow()
    {
        var window = GetWindow<BasicSkillsEditorWindow>("BasicSkills");
        window.minSize = new Vector2(600, 300);
    }

    private void OnGUI()
    {
        basicSkills = GetAllInstances<BasicSkillBase>();
        //serializedObject = new SerializedObject(activeSkills[0]);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));

        DrawSliderBar(basicSkills);

        EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);

        if (selectedProperty != null)
        {
            for (int i = 0; i < basicSkills.Length; i++)
            {
                if (basicSkills[i].BaseName == selectedProperty)
                {
                    serializedObject = new SerializedObject(basicSkills[i]);
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawProperties(serializedProperty, basicSkills[i].BaseName);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from list or create new one.");
        }

        EditorGUILayout.EndScrollView();
        //EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
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

    protected void DrawProperties(SerializedProperty p, string name)
    {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }

    protected void DrawSliderBar(BasicSkillBase[] prop)
    {
        foreach (var p in prop)
        {
            if (GUILayout.Button(p.BaseName))
            {
                selectedPropertyPach = p.BaseName;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPach))
        {
            selectedProperty = selectedPropertyPach;
        }

        if (GUILayout.Button("Create"))
        {
            BasicSkillBase newSkill = ScriptableObject.CreateInstance<BasicSkillBase>();
            BasicSkillsCreateWindow newSkillWindow = GetWindow<BasicSkillsCreateWindow>("New Basic Skill");
            newSkillWindow.newSkill = newSkill;
            newSkillWindow.minSize = new Vector2(600, 300);
        }
    }

    protected void Apply()
    {
        if (serializedObject != null)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
