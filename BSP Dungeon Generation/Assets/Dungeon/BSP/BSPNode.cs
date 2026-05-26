using UnityEngine;

public class BSPNode : Room
{ 
    public BSPNode _leftNode;
    public BSPNode _rightNode;

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