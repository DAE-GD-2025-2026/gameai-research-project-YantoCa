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
        // Testing BSPNode
        BSPNode node = new BSPNode(2, 3, 0, 2);
        
        BuildRoom(node);

        //GameObject test = Instantiate(_roomPrefab, this.transform.position, this.transform.localRotation, transform);
        //test.GetComponent<RoomRenderer>().InitializeRoom(_startWidth, _startHeight);
    }

    private void BuildRoom(Room room)
    {
        GameObject roomInstance = Instantiate(_roomPrefab, Vector3.one * room.GetCenter(), Quaternion.identity, transform);
        roomInstance.GetComponent<RoomRenderer>().InitializeRoom(room.GetWidth(), room.GetWidth());
    }
}
