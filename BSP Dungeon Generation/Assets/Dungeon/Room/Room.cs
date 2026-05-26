using UnityEngine;

public class Room
{
    protected int left, right, top, bottom;

    public Room(int left, int right, int top, int bottom)
    {
        this.left = left;
        this.right = right;
        this.top = top;
        this.bottom = bottom;
    }

    #region Getters
    // Expects Inclusive bounds (right >= left, same for top/bot)
    public int GetWidth() { return right - left + 1; }
    public int GetHeight() { return bottom - top + 1; }
    public Vector2 GetCenter() { return new Vector2((left + right) * 0.5f, (top + bottom) * 0.5f); }

    // TODO are these even needed?????
    protected int GetLeft() { return left; }
    protected int GetRight() {  return right; }
    protected int GetTop() { return top; }
    protected int GetBottom() { return bottom; }
    #endregion
}
