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
                    Debug.Log("������ �������� A. someInteger: " + someInteger);
                    // ��� ��� ��� ��������� A �����...
                    break;
                case MyParameterType.OptionB:
                    Debug.Log("������ �������� B. someFloat: " + someFloat);
                    // ��� ��� ��� ��������� B �����...
                    break;
                case MyParameterType.OptionC:
                    Debug.Log("������ �������� C. someString: " + someString);
                    // ��� ��� ��� ��������� C �����...
                    break;
            }*/
        }
    }
}