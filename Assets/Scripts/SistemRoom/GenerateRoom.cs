using System.Collections.Generic;
using UnityEngine;

namespace RA
{
    public class GenerateRoom : MonoBehaviour
    {
        [System.Serializable]
        public class Room
        {
            public GameObject prefab;//������
            public Vector3 position;//�������
            public Room neighbors;//������ ������� �������
            public RoomData roomData;
            public List<Transform> entrances = new();//������ ������� ��� ������ ���������
            public List<GameObject> nulling = new();
        }

        [System.Serializable]
        public class RoomData
        {
            public GameObject prefab;
            public Vector3 entrancePosition;//����������
            public Quaternion roomDirection;//����������
        }//������������� ��� ���������

        [Header("prefab Room")]
        public GameObject roomPrefabs;//������ ������
        [Header("prefab Transition")]
        public GameObject transitionPrefab;//������ ��������
        [Header("prefab Start Room")]
        public GameObject startRooms;//������ ��������� ����
        [Header("prefab �������")]
        public GameObject prefabNulling;//������ ��������� ����


        public Vector3 startPosition = Vector3.zero;// ��������� ������� ������
        public int maxRooms = 2;//���������� �������� � ������ ������
        public int minDistance = 4;//��������� ��������� ����� ������

        private Queue<Room> _rooms = new();
        private Room _currentRoom;//������� �������
        private Room endRoom;//��������� �������� �������

        private GameObject startRoomPull;
        private List<GameObject> parentRoomPull = new();
        private List<GameObject> roomPull = new();
        private List<GameObject> nullingPull = new();

        ManagerRoom manager;

        public GameObject Player;

        public void ReGeneration()
        {
            startPosition = Player.transform.position;

            for (int i = 0; i < roomPull.Count; i++)
                roomPull[i].SetActive(false);
            for (int i = 0; i < parentRoomPull.Count; i++)
                parentRoomPull[i].SetActive(false);
            for (int i = 0; i < nullingPull.Count; i++)
                nullingPull[i].SetActive(false);

            _rooms.Clear();

            _currentRoom = null;
            endRoom = null;
            startRoomPull.SetActive(false);

            GenerateInitialRooms();
        }

        public void Init(ManagerRoom mn)
        {
            manager = mn;

            InitialPull();

            GenerateInitialRooms();
        }
        private void InitialPull()
        {
            if (startRoomPull == null)
                startRoomPull = Instantiate(startRooms);

            if (parentRoomPull.Count <= 0)
            {
                for (int i = 0; i < maxRooms; i++)
                {
                    GameObject d = Instantiate(transitionPrefab);

                    TransitionTrigger s = null;
                    if (d.GetComponent<TransitionTrigger>() == null)
                    {
                        s = d.AddComponent<TransitionTrigger>();
                        s.manager = manager;
                    }
                    else
                        d.GetComponent<TransitionTrigger>().manager = manager;

                    d.SetActive(false);
                    parentRoomPull.Add(d);
                }
            }
            if (roomPull.Count <= 0)
            {
                for (int i = 0; i < maxRooms; i++)
                {
                    GameObject d = Instantiate(roomPrefabs);
                    d.SetActive(false);
                    roomPull.Add(d);
                }
            }
            if (nullingPull.Count <= 0)
            {
                for (int i = 0; i < (maxRooms * 3); i++)
                {
                    GameObject d = Instantiate(prefabNulling);
                    d.SetActive(false);
                    nullingPull.Add(d);
                }
            }

        }
        // ��������� ��������� ������
        private void GenerateInitialRooms()
        {
            // ������� ��������� �������
            CreateRoom(startPosition, true, startRoom: startRoomPull);

            //������� ������ �������
            for (int i = 0; i < maxRooms; i++)
            {
                GenerateNewRoom();
            }
        }//������� ��������� �������

        public void OnRoomCleared()
        {
            GenerateNewRoom();
        }//���������� ��� �������� ������� ������� ����� �������-������� ������

        private void GenerateNewRoom()
        {
            Vector3 direction = (endRoom.roomData.entrancePosition - endRoom.position).normalized;
            float tunnelLength = Vector3.Distance(endRoom.position, endRoom.roomData.entrancePosition);  // ����� �������
            Vector3 newRoomPosition = endRoom.roomData.entrancePosition + direction * tunnelLength;  // ����� ������� �������

            // �������� ������� �� ����� �������
            CreateRoom(newRoomPosition, false);
        }

        //������� ����� ������� �� ��������� �������
        private void CreateRoom(Vector3 position, bool isFirstRoom = false, GameObject startRoom = null)
        {
            GameObject targetRoom = null;
            if (startRoom == null)
            {
                foreach (var i in roomPull)
                {
                    if (!i.activeInHierarchy)
                    {
                        targetRoom = i;
                        break;
                    }
                }
                if (targetRoom == null)
                {
                    targetRoom = Instantiate(roomPrefabs);
                    roomPull.Add(targetRoom);
                }
            }
            else
                targetRoom = startRoom;

            targetRoom.transform.position = position;
            targetRoom.transform.rotation = Quaternion.identity;

            // ������� ��������� �������
            Room newRoom = new()
            {
                prefab = targetRoom,
                position = position,
            };

            if(endRoom != null)
                endRoom.neighbors = newRoom;

            _rooms.Enqueue(newRoom);

            Transform[] allTransforms = targetRoom.GetComponentInChildren<RoomTransformParent>().trans;

            foreach (Transform t in allTransforms)
            {
                newRoom.entrances.Add(t);
            }

            if (isFirstRoom)
            {
                _currentRoom = newRoom;
            }

            targetRoom.SetActive(true);

            CreateTransition(newRoom);
            CreateNulling(newRoom);
            endRoom = newRoom;
        }
        void CreateNulling(Room newRoom)
        {
            for (int i = 0; i < newRoom.entrances.Count; i++)
            {
                Transform newRoomEntrance = newRoom.entrances[i];
                GameObject transition = null;

                foreach (var c in nullingPull)
                {
                    if (!c.activeInHierarchy)
                    {
                        transition = c;
                        break;
                    }
                }
                if (transition == null)
                {
                    transition = Instantiate(prefabNulling);
                    nullingPull.Add(transition);
                }
                newRoom.nulling.Add(transition);

                UpdateNullindAndTransition(newRoomEntrance, newRoom, transition);
            }
        }
        //������� ������� ����� ���������
        private void CreateTransition(Room newRoom)
        {
            Transform newRoomEntrance;
            GameObject transition = null;

            if (newRoom.entrances.Count > 1 && endRoom != null)
            {
                Vector3 dir = endRoom.position - newRoom.position;
                Vector2 dir2D = new(dir.x, dir.z);

                foreach (var a in newRoom.entrances)
                {
                    Vector3 dirs = a.position - newRoom.position;
                    Vector2 dirs2D = new(dirs.x, dirs.z);

                    float angleA = Mathf.Atan2(dir2D.y, dir2D.x);
                    float angleB = Mathf.Atan2(dirs2D.y, dirs2D.x);

                    float angleRad = angleB - angleA;
                    float angleDeg = angleRad * Mathf.Rad2Deg;
                    if (angleDeg < 0)
                        angleDeg *= -1;

                    if (angleDeg < 80)
                    {
                        newRoom.entrances.Remove(a);
                        break;
                    }
                }

                newRoomEntrance = newRoom.entrances[Random.Range(0, newRoom.entrances.Count)];
            }
            else
                newRoomEntrance = newRoom.entrances[Random.Range(0, newRoom.entrances.Count)];

            newRoom.entrances.Remove(newRoomEntrance);

            foreach (var c in parentRoomPull)
            {
                if (!c.activeInHierarchy)
                {
                    transition = c;
                    break;
                }
            }
            if (transition == null)
            {
                TransitionTrigger s = null;

                transition = Instantiate(transitionPrefab);
                if (transition.GetComponent<TransitionTrigger>() == null)
                {
                    s = transition.AddComponent<TransitionTrigger>();
                    s.manager = manager;
                }
                else
                    transition.GetComponent<TransitionTrigger>().manager = manager;

                parentRoomPull.Add(transition);
            }

            RoomData rom = UpdateNullindAndTransition(newRoomEntrance, newRoom, transition);

            // ��������� ���������� � �������
            newRoom.roomData = rom;
        }
        RoomData UpdateNullindAndTransition(Transform newRoomEntrance, Room newRoom, GameObject transition, bool nulling = false)
        {
            Vector3 direction = newRoom.position - newRoomEntrance.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transition.transform.position = newRoomEntrance.position;
            transition.transform.rotation = targetRotation;
            transition.SetActive(true);

            if (nulling)
                return null;

            return new RoomData
            {
                prefab = transition,
                entrancePosition = newRoomEntrance.position,//����������
                roomDirection = targetRotation,//�������
            };//������������� ��� ���������
        }

        //��������� ������� �������
        public void MoveToNextRoom()
        {
            _currentRoom = _currentRoom.neighbors;
            GenerateNewRoom();
        }
        public void MoveExitToRoom()
        {
            if (_rooms.Count < 5)
                return;

            Room rom = _rooms.Dequeue();

            for(int i = 0; i < rom.nulling.Count; i++) 
                rom.nulling[i].SetActive(false);

            rom.roomData.prefab.SetActive(false);
            rom.prefab.SetActive(false);
        }
    }
}