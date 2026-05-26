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
        GameObject test = Instantiate(_roomPrefab, this.transform.position, this.transform.localRotation, transform);
        test.GetComponent<Room>().InitializeRoom(_startWidth, _startHeight);
    }
}
