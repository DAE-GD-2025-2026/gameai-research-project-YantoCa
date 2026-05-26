using UnityEngine;

public class BSPGenerator
{
    private BSPNode _rootNode;

    // Limits
    private const int _minHeight = 8;
    private const int _maxHeight = 20;

    private const int _minWidth = 8;
    private const int _maxWidth = 20;

    private const int _trimSize = 1;
    private const int _corridorMargin = 1;
    private const int _minCorridorSize = 2;

    private bool _horizontalSplit;
    private bool _verticalSplit;

    public BSPGenerator(int left, int top, int right, int bottom)
    {
        _rootNode = new BSPNode(left, top, right, bottom);

        // Start the recursion
        Split(_rootNode);
    }

    private void Split(BSPNode node)
    {
        if (node.GetWidth() < _minWidth && node.GetHeight() < _minHeight) return; // Stop early, Smallest it can go

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
            float splitHeight = Random.Range(_minHeight, node.GetHeight() - _minHeight); // limit between size

            node._leftNode = new BSPNode(node.GetLeft(), node.GetRight(), splitHeight, node.GetBottom());
            node._rightNode = new BSPNode(node.GetLeft(), node.GetRight(), node.GetTop() - splitHeight, node.GetBottom() + splitHeight);
        }
        else // Vertical Split
        {
            float splitWidth = Random.Range(_minHeight, node.GetHeight() - _minHeight); // limit between size

            node._leftNode = new BSPNode(node.GetLeft(), splitWidth, node.GetTop(), node.GetBottom());
            node._rightNode = new BSPNode(node.GetLeft() + splitWidth, node.GetRight() - splitWidth, node.GetTop(), node.GetBottom());
        }

        // Recursion
        Split(node._leftNode);
        Split(node._rightNode);
    }
}
