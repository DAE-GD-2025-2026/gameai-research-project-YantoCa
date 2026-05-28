using UnityEngine;

public class RoomRenderer : MonoBehaviour
{ 
    [SerializeField] private float _width = 1, _height = 1; 

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnValidate() // When edited through the inspector RUN this
    {
        ResizeSprite(_width, _height);
    }

    public void InitializeRoom(float width, float height, Color color, int SortOrder) // TODO maybe return room or something gameobject even
    {
        // Position gets set through the initialize "Instantiate(room, POSITION, rotation);"
        if(sr != null) sr.color = color;

        ResizeSprite(width, height);
        sr.sortingOrder = SortOrder;
    }

    private void ResizeSprite(float width, float height)
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>(); // for OnValidate() to function

        _width = width;
        _height = height;

        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector3(
            _width / spriteSize.x,
            _height / spriteSize.y,
            1f
        );
    }
}