using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;

    [SerializeField] private GameObject _roomCollection;
    [SerializeField] private GameObject _debugCollection;

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
        BSPGenerator BSPTree = new BSPGenerator(0,
                                                _startWidth, 
                                                _startHeight, 
                                                0,
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
            DrawBSPSquare(node);
            BuildRoom(node._actualRoom);
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
        GameObject roomInstance = Instantiate(_roomPrefab, Vector3.one * room.GetCenter(), Quaternion.identity, _roomCollection.transform);
        roomInstance.GetComponent<RoomRenderer>().InitializeRoom(room.GetWidth(), room.GetHeight(), Random.ColorHSV());
    } 
    private void DrawBSPSquare(Room BSP)
    {
        GameObject roomInstance = Instantiate(_roomPrefab, Vector3.one * BSP.GetCenter(), Quaternion.identity, _debugCollection.transform);
        roomInstance.GetComponent<RoomRenderer>().InitializeRoom(BSP.GetWidth(), BSP.GetHeight(), new Color(Random.Range(0f, 1f),0f,0f));
    }
}
