using UnityEngine;
using UnityEditor;

public class PassiveSkillsEditorWindow : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected PassiveSkillBase[] passiveSkills;
    protected string selectedPropertyPach;
    protected string selectedProperty;

    Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Maestro/Design/Skills/PassiveSkills")]
    protected static void ShowWindow()
    {
        var window = GetWindow<PassiveSkillsEditorWindow>("PassiveSkills");
        window.minSize = new Vector2(600, 300);
    }

    private void OnGUI()
    {
        passiveSkills = GetAllInstances<PassiveSkillBase>();
        //serializedObject = new SerializedObject(activeSkills[0]);
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));

        DrawSliderBar(passiveSkills);

        EditorGUILayout.EndVertical();
        //EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);

        if (selectedProperty != null)
        {
            for (int i = 0; i < passiveSkills.Length; i++)
            {
                if (passiveSkills[i].BaseName == selectedProperty)
                {
                    serializedObject = new SerializedObject(passiveSkills[i]);
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawProperties(serializedProperty, passiveSkills[i].BaseName);
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

    protected void DrawProperties(SerializedProperty p, string name)
    {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }

    protected void DrawSliderBar(PassiveSkillBase[] prop)
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
            PassiveSkillBase newSkill = ScriptableObject.CreateInstance<PassiveSkillBase>();
            PassiveSkillsCreateWindow newSkillWindow = GetWindow<PassiveSkillsCreateWindow>("New Passive Skill");
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
