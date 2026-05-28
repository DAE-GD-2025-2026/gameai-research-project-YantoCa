using UnityEngine;

public class BSPNode : Room
{ 
    public BSPNode _leftNode;
    public BSPNode _rightNode;

    public bool _horizontalSplit;
    public Room _actualRoom; // resizing it inside of the BSP bounds

    public BSPNode(float left, float right, float top, float bottom) : base(left, right, top, bottom)
    { 
        _leftNode = null;
        _rightNode = null; 
    }

    public bool IsLeaf()
    {
        return (_leftNode == null) && (_rightNode == null); // No Neighbours = Leaf
    }
}