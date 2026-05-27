using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;

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

    private void GenerateDungeon()
    {
        BSPGenerator BSPTree = new BSPGenerator(transform.position.x - (_startWidth * 0.5f),
                                                _startWidth, 
                                                _startHeight, 
                                                transform.position.y - (_startHeight * 0.5f),
                                                _smallestWidth,
                                                _smallestHeight);

        CreateRooms(BSPTree._rootNode);
    }

    private void CreateRooms(BSPNode node)
    {
        if (node == null) return; // Empty skip

        if (node.IsLeaf())
        {
            // Build Leaf Room
            BuildRoom(node);
        }
        else
        {
            // Recursion
            CreateRooms(node._leftNode);
            CreateRooms(node._rightNode);
        }
    }

    private void BuildRoom(Room room)
    {
        GameObject roomInstance = Instantiate(_roomPrefab, Vector3.one * room.GetCenter(), Quaternion.identity, transform);
        roomInstance.GetComponent<RoomRenderer>().InitializeRoom(room.GetWidth(), room.GetWidth());
    }
}
