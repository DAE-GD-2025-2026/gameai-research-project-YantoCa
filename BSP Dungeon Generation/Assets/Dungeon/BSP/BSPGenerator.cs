using Unity.VisualScripting;
using UnityEngine;

public class BSPGenerator
{
    public BSPNode _rootNode { get; private set; }

    // Limits
    private float _minWidth = 8; // currently this is hte min on BOTH sides
    private const int _maxWidth = 20;

    private float _minHeight = 8; // currently is the min on both sides
    private const int _maxHeight = 20;

    private const int _trimSize = 1;
    private const int _corridorMargin = 1;
    private const int _minCorridorSize = 2;

    private bool _horizontalSplit;
    private bool _verticalSplit;

    public BSPGenerator(float left, float right, float top, float bottom, float smallestWidth, float smallestHeight)
    {
        _minWidth = smallestWidth;
        _minHeight = smallestHeight;

        _rootNode = new BSPNode(left, right, top, bottom);

        // Start the recursion
        Split(_rootNode);
    }

    private void Split(BSPNode node)
    { 
        if (node.GetWidth() <= _minWidth * 2 && node.GetHeight() <= _minHeight * 2) return; // Stop early, Smallest it can go 

        bool splitHorizontally = Random.value > 0.5f; // Randomize Vertical or Horizontal
         
        if (node.GetWidth() > node.GetHeight() * 1.25f)
        {
            splitHorizontally = false; 
        }
        else if (node.GetHeight() > node.GetWidth() * 1.25f)
        {
            splitHorizontally = true; 
        }

        // TODO maybe this can be done INSIDE BSPNode (so we dont have to make the left and ... public)
        // Splitting of the node
        if (splitHorizontally)
        {
            if (node.GetHeight() <= _minHeight * 2)
            {
                SplitVertically(node);
            }
            else
            {
                SplitHorizontally(node);
            }
        }
        else // Split vertical
        { 
            if (node.GetWidth() <= _minWidth * 2)
            {
                SplitHorizontally(node);
            }
            else
            {
                SplitVertically(node);
            }
        }

        // Recursion  
        if (node._leftNode != null) 
            Split(node._leftNode);

        if (node._rightNode != null) 
            Split(node._rightNode);
    }

    private void SplitHorizontally(BSPNode node)
    { 
        float splitHeight = Random.Range(_minHeight, node.GetHeight() - _minHeight);
        float splitY = node.GetBottom() + splitHeight;
         
        node._leftNode = new BSPNode(node.GetLeft(), node.GetRight(), node.GetTop(), splitY); 
        node._rightNode = new BSPNode(node.GetLeft(), node.GetRight(), splitY, node.GetBottom());
    }

    private void SplitVertically(BSPNode node)
    { 
        float splitWidth = Random.Range(_minWidth, node.GetWidth() - _minWidth);
        float splitX = node.GetLeft() + splitWidth;
         
        node._leftNode = new BSPNode(node.GetLeft(), splitX, node.GetTop(), node.GetBottom()); 
        node._rightNode = new BSPNode(splitX, node.GetRight(), node.GetTop(), node.GetBottom());
    }
}
