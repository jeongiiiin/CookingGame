using UnityEditor;
using UnityEngine;

public class Align : EditorWindow
{
    private GameObject baseObject;
    private Vector3 spacing;

    [MenuItem("Tools/Bonnate/Object Align Via Vector3 Value")]
    private static void OpenWindow()
    {
        Align window = GetWindow<Align>();
        window.titleContent = new GUIContent("Object Align");
        window.Show();
    }

    private void OnGUI()
    {
        baseObject = EditorGUILayout.ObjectField("������ �� ������Ʈ", baseObject, typeof(GameObject), true) as GameObject;
        spacing = EditorGUILayout.Vector3Field("����", spacing);

        if (GUILayout.Button("����"))
        {
            AlignObjects();
        }

        GUILayout.Label("������ �� ������Ʈ ������Ƽ�� �Ҵ��մϴ�.");
        GUILayout.Label("Scene���� ������ �� ������Ʈ�κ��� ���ݸ��� ������ ������Ʈ���� �����մϴ�.");
        GUILayout.Label("[����] ��ư�� ���� �����մϴ�.");
    }

    private void AlignObjects()
    {
        if (baseObject == null)
        {
            Debug.LogError("������ �� ������Ʈ�� ����!");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;
        int objectCount = selectedObjects.Length;

        if (objectCount == 0)
        {
            Debug.LogError("���õ� ������Ʈ�� ����!");
            return;
        }

        Undo.RegisterCompleteObjectUndo(selectedObjects, "Align Objects");

        Vector3 currentPosition = baseObject.transform.position + spacing;

        for (int i = 0; i < objectCount; i++)
        {
            GameObject obj = selectedObjects[i];

            if (obj == baseObject)
                continue;

            Undo.RecordObject(obj.transform, "Move Object");

            obj.transform.position = currentPosition;
            currentPosition += spacing;
        }
    }
}