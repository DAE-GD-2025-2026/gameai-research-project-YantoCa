using UnityEngine;

public class BSPGenerator
{
    public BSPNode _rootNode { get; private set; }

    // Limits
    private float _minWidth = 8;  
    private float _minHeight = 8;

    private const float _padding = 1f;

    public BSPGenerator(float left, float right, float top, float bottom, float smallestWidth, float smallestHeight)
    {
        _minWidth = smallestWidth;
        _minHeight = smallestHeight;

        _rootNode = new BSPNode(left, right, top, bottom);

        // Start the recursion
        Split(_rootNode);

        // Resize rooms
        GenerateRoomsInLeaves(_rootNode); 
    }

    private void Split(BSPNode node)
    { 
        if (node.GetWidth() <= _minWidth * 2 && node.GetHeight() <= _minHeight * 2) return; // Stop early, Smallest it can go 

        bool splitHorizontally = Random.value > 0.5f; // Randomize Vertical or Horizontal
         
        if (node.GetWidth() > node.GetHeight())
        {
            splitHorizontally = false; 
        }
        else if (node.GetHeight() > node.GetWidth())
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

    private void GenerateRoomsInLeaves(BSPNode node)
    {
        if (node == null) return;

        if (node.IsLeaf())
        { 
            float extraPadding = _padding * 2f;

            float maxW = node.GetWidth() - extraPadding;
            float maxH = node.GetHeight() - extraPadding;

            float minW = Mathf.Min(_minWidth * 0.5f - extraPadding, maxW);
            float minH = Mathf.Min(_minWidth * 0.5f - extraPadding, maxH);

            // Randomize Width/Height of room
            float roomW = Random.Range(minW, maxW);
            float roomH = Random.Range(minH, maxH);

            // Randomly position the custom room inside the leaf boundaries
            float roomLeft = Random.Range(node.GetLeft() + _padding, node.GetRight() - roomW - _padding);
            float roomBottom = Random.Range(node.GetBottom() + _padding, node.GetTop() - roomH - _padding);
            
            node._actualRoom = new Room(roomLeft, roomLeft + roomW, roomBottom + roomH, roomBottom);
        }
        else
        {
            GenerateRoomsInLeaves(node._leftNode);
            GenerateRoomsInLeaves(node._rightNode);
        }
    }

    
}
