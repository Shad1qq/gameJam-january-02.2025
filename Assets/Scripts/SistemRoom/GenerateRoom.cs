using System.Collections.Generic;
using UnityEngine;

namespace RA
{
    public class GenerateRoom : MonoBehaviour
    {
        [System.Serializable]
        public class Room
        {
            public GameObject prefab;//префаб
            public Vector3 position;//позиция
            public Room neighbors;//список соседий комнаты
            public RoomData roomData;
            public List<Transform> entrances = new();//список позиций для спавна переходов
            public List<GameObject> nulling = new();
        }

        [System.Serializable]
        public class RoomData
        {
            public GameObject prefab;
            public Vector3 entrancePosition;//координата
            public Quaternion roomDirection;//направлние
        }//трансформация для переходов

        [Header("prefab Room")]
        public GameObject roomPrefabs;//префаб комнат
        [Header("prefab Transition")]
        public GameObject transitionPrefab;//префаб перехода
        [Header("prefab Start Room")]
        public GameObject startRooms;//префаб начальной румы
        [Header("prefab затычка")]
        public GameObject prefabNulling;//префаб начальной румы


        public Vector3 startPosition = Vector3.zero;// начальная позиция старта
        public int maxRooms = 2;//количество созданых в начале комнат
        public int minDistance = 4;//минималка растояния между комнат

        private Queue<Room> _rooms = new();
        private Room _currentRoom;//текущая комната
        private Room endRoom;//последняя созданая комната

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
        // Генерация начальных комнат
        private void GenerateInitialRooms()
        {
            // Создаем начальную комнату
            CreateRoom(startPosition, true, startRoom: startRoomPull);

            //Создаем другие комнаты
            for (int i = 0; i < maxRooms; i++)
            {
                GenerateNewRoom();
            }
        }//создает начальные комнаты

        public void OnRoomCleared()
        {
            GenerateNewRoom();
        }//вызывается при зачистке комнаты генерит новую комнату-удаляет старую

        private void GenerateNewRoom()
        {
            Vector3 direction = (endRoom.roomData.entrancePosition - endRoom.position).normalized;
            float tunnelLength = Vector3.Distance(endRoom.position, endRoom.roomData.entrancePosition);  // длина туннеля
            Vector3 newRoomPosition = endRoom.roomData.entrancePosition + direction * tunnelLength;  // новая позиция комнаты

            // Создание комнаты на новой позиции
            CreateRoom(newRoomPosition, false);
        }

        //Создает новую комнату на указанной позиции
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

            // Создаем экземпляр комнаты
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
        //Создает переход между комнатами
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

            // Добавляем информацию о соседях
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
                entrancePosition = newRoomEntrance.position,//координата
                roomDirection = targetRotation,//поворот
            };//трансформация для переходов
        }

        //Обновляет текущую комнату
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