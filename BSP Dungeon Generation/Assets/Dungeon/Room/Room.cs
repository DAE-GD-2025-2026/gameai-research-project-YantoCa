using UnityEngine;

public class Room
{
    protected float left, right, top, bottom;

    public Room(float left, float right, float top, float bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }

    #region Getters
    // Expects Inclusive bounds (right >= left, same for top/bot)
    public float GetWidth() { return right - left; } // right should be the HIGHEST value
    public float GetHeight() { return top - bottom; } // top should be the HIGHEST value
    public Vector2 GetCenter() { return new Vector2((left + right) * 0.5f, (bottom + top) * 0.5f); }

    // TODO are these even needed????? (maybe make properties atp) {get; private set}
    public float GetLeft() { return left; }
    public float GetRight() {  return right; }
    public float GetTop() { return top; }
    public float GetBottom() { return bottom; }
    #endregion
}
