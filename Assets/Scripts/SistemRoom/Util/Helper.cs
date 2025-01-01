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

            // Выбор параметра
            myScript.selectedParameter = (CustomEditors.MyParameterType)EditorGUILayout.EnumPopup("Выберите параметр:", myScript.selectedParameter);

            // Параметры, зависимые от выбранного типа
            switch (myScript.selectedParameter)
            {
                case CustomEditors.MyParameterType.OptionA:
                    myScript.someInteger = EditorGUILayout.IntField("Целое число:", myScript.someInteger);
                    break;
                case CustomEditors.MyParameterType.OptionB:
                    myScript.someFloat = EditorGUILayout.FloatField("Вещественное число:", myScript.someFloat);
                    break;
                case CustomEditors.MyParameterType.OptionC:
                    myScript.someString = EditorGUILayout.TextField("Строка:", myScript.someString);
                    break;
            }

            // Кнопка
            if (GUILayout.Button("пересоздать подземелье"))
            {
                myScript.RunMyCode();
            }

            serializedObject.ApplyModifiedProperties(); //Для корректного сохранения изменений
        }
    }
}