using UnityEngine;

namespace RA
{
    public class CustomEditors : MonoBehaviour
    {
        public GenerateRoom room;

        public enum MyParameterType { OptionA, OptionB, OptionC }

        public MyParameterType selectedParameter;
        public int someInteger;
        public float someFloat;
        public string someString;

        private void Start()
        {
            room = GetComponent<GenerateRoom>();
        }
        public void RunMyCode()
        {
            room.ReGeneration();

            /*switch (selectedParameter)
            {
                case MyParameterType.OptionA:
                    Debug.Log("Выбран параметр A. someInteger: " + someInteger);
                    // Ваш код для параметра A здесь...
                    break;
                case MyParameterType.OptionB:
                    Debug.Log("Выбран параметр B. someFloat: " + someFloat);
                    // Ваш код для параметра B здесь...
                    break;
                case MyParameterType.OptionC:
                    Debug.Log("Выбран параметр C. someString: " + someString);
                    // Ваш код для параметра C здесь...
                    break;
            }*/
        }
    }
}