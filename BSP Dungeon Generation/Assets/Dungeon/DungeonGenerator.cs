using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;

    [Space(10), Header("Grouping")]
    [SerializeField] private GameObject _roomCollection;
    [SerializeField] private GameObject _corridorsCollection;
    [SerializeField] private GameObject _debugCollection;

    [Space(10), Header("Dungeon Options")]
    [SerializeField] private bool _generateDebugBSP = false;
    [SerializeField] private bool _isTrimmed = false;

    [Space(10), Header("Starting Values")]
    [SerializeField] private float _startWidth;
    [SerializeField] private float _startHeight;

    [Space(10), Header("Stopping Parameters")]
    [SerializeField] private float _smallestWidth;
    [SerializeField] private float _smallestHeight;
    
    void Start()
    {
        GenerateDungeon();
    }

    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Generating new Dungeon...");
            DestroyDungeon();
            GenerateDungeon();
        }
    }
    private void DestroyDungeon()
    {
        foreach (Transform child in _roomCollection.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _corridorsCollection.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _debugCollection.transform)
        {
            Destroy(child.gameObject);
        }
    }
    private void GenerateDungeon()
    {
        BSPGenerator BSPTree = new BSPGenerator(0,
                                                _startWidth, 
                                                _startHeight, 
                                                0,
                                                _smallestWidth,
                                                _smallestHeight,
                                                _isTrimmed);

        if (_generateDebugBSP)
        {
            ShowBSP(BSPTree._rootNode);
        }

        // Draw the generated Dungeon
        BuildCorridors(BSPTree.Corridors);
        CreateRooms(BSPTree._rootNode);
    }

    private void CreateRooms(BSPNode node)
    {
        if (node == null) return; // Empty skip

        if (node.IsLeaf())
        {
            // Build Leaf Room
            BuildRoom(node._actualRoom, _roomCollection, Random.ColorHSV(), 2);
        }
        else
        {
            // Recursion
            CreateRooms(node._leftNode);
            CreateRooms(node._rightNode);
        }
    }

    private void BuildRoom(Room room, GameObject collection, Color color, int sortOrder)
    {
        GameObject roomInstance = Instantiate(_roomPrefab, room.GetCenter(), Quaternion.identity, collection.transform);
        roomInstance.GetComponent<RoomRenderer>().InitializeRoom(room.GetWidth(), room.GetHeight(), color, sortOrder);
    } 
    private void BuildCorridors(List<Room> corridors)
    {
        foreach (Room corridor in corridors)
        {
            BuildRoom(corridor, _corridorsCollection, Color.white, 1);
        }
    }

    #region Debug
    private void ShowBSP(BSPNode node)
    {
        if (node == null) return; // Empty skip

        if (node.IsLeaf())
        {
            // Build Leaf Room
            BuildRoom(node, _debugCollection, new Color(Random.Range(0f, 1f), 0f, 0f), 0);
        }
        else
        {
            // Recursion
            ShowBSP(node._leftNode);
            ShowBSP(node._rightNode);
        }
    }
    #endregion
}
