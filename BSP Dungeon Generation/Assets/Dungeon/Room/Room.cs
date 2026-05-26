using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private float _width = 1, _height = 1;

    private SpriteRenderer sr; 
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // Set color
        sr.color = Random.ColorHSV();
    }
    private void OnValidate() // When edited through the inspector RUN this
    {
        ResizeSprite(_width, _height);
    }

    public void InitializeRoom(float width, float height) // TODO maybe return room or something gameobject even
    {
        // Position gets set through the initialize "Instantiate(room, POSITION, rotation);"

        ResizeSprite(width, height);
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


    #region Getters And Setters
    protected float GetWidth()
    {
        return _width;
    }
    protected float GetHeight()
    {
        return _height;
    }

    protected Vector3 GetPosition()
    {
        return this.transform.position;
    }
    #endregion
}
