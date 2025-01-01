using UnityEditor;
using UnityEngine;

namespace RA
{
    [CustomEditor(typeof(CustomEditors))]
    public class Helper : Editor
    {
        public override void OnInspectorGUI()
        {
            CustomEditors myScript = (CustomEditors)target;

            // ����� ���������
            myScript.selectedParameter = (CustomEditors.MyParameterType)EditorGUILayout.EnumPopup("�������� ��������:", myScript.selectedParameter);

            // ���������, ��������� �� ���������� ����
            switch (myScript.selectedParameter)
            {
                case CustomEditors.MyParameterType.OptionA:
                    myScript.someInteger = EditorGUILayout.IntField("����� �����:", myScript.someInteger);
                    break;
                case CustomEditors.MyParameterType.OptionB:
                    myScript.someFloat = EditorGUILayout.FloatField("������������ �����:", myScript.someFloat);
                    break;
                case CustomEditors.MyParameterType.OptionC:
                    myScript.someString = EditorGUILayout.TextField("������:", myScript.someString);
                    break;
            }

            // ������
            if (GUILayout.Button("����������� ����������"))
            {
                myScript.RunMyCode();
            }

            serializedObject.ApplyModifiedProperties(); //��� ����������� ���������� ���������
        }
    }
}