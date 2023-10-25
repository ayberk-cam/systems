using UnityEngine;
using UnityEditor;

public class ActiveSkillsEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected ActiveSkillBase[] activeSkills;
    protected string selectedPropertyPach;
    protected string selectedProperty;

    Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Maestro/Design/Skills/ActiveSkills")]
    protected static void ShowWindow()
    {
        var window = GetWindow<ActiveSkillsEditorWindow>("ActiveSkills");
        window.minSize = new Vector2(600, 300);
    }

    private void OnGUI()
    {
        activeSkills = GetAllInstances<ActiveSkillBase>();
        //serializedObject = new SerializedObject(activeSkills[0]);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));

        DrawSliderBar(activeSkills);

        EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);

        if (selectedProperty != null)
        {
            for (int i = 0; i < activeSkills.Length; i++)
            {
                if (activeSkills[i].BaseName == selectedProperty)
                {
                    serializedObject = new SerializedObject(activeSkills[i]);
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawProperties(serializedProperty,activeSkills[i].BaseName);
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

    protected void DrawProperties(SerializedProperty p, string name)
    {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }

    protected void DrawSliderBar(ActiveSkillBase[] prop)
    {
        foreach (ActiveSkillBase p in prop)
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
            ActiveSkillBase newSkill = ScriptableObject.CreateInstance<ActiveSkillBase>();
            ActiveSkillsCreateWindow newSkillWindow = GetWindow<ActiveSkillsCreateWindow>("New Active Skill");
            newSkillWindow.newSkill = newSkill;
            newSkillWindow.minSize = new Vector2(600, 300);
        }
    }

    protected void Apply()
    {
        if(serializedObject != null)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
