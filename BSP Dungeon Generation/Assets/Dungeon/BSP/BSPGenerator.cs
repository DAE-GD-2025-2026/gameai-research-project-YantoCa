using System.Collections.Generic;
using System.Diagnostics; 
using UnityEngine;

public class BSPGenerator
{
    public BSPNode _rootNode { get; private set; }
    public List<Room> Corridors { get; private set; } = new List<Room>();

    // Limits
    private float _minWidth = 8;  
    private float _minHeight = 8;

    private const float _padding = 1f;

    // Corridor
    public const float _corridorThickness = 1f;

    // Time
    private double _timeSplit;
    private double _timeRooms;
    private double _timeCorridors;

    public BSPGenerator(float left, float right, float top, float bottom, float smallestWidth, float smallestHeight)
    {
        _minWidth = smallestWidth;
        _minHeight = smallestHeight;

        _rootNode = new BSPNode(left, right, top, bottom);

        // Start Timer
        Stopwatch timer = Stopwatch.StartNew();

        // Start the recursion
        Split(_rootNode);
        _timeSplit = timer.Elapsed.TotalMilliseconds;
        
        // Resize rooms
        GenerateRoomsInLeaves(_rootNode);
        _timeRooms = timer.Elapsed.TotalMilliseconds - _timeSplit;

        // Create Corridors
        //ConnectNodes(_rootNode);
        ConnectLeafSisters(_rootNode);
        _timeCorridors = timer.Elapsed.TotalMilliseconds - (_timeSplit + _timeRooms);

        timer.Stop();
        DisplayTime();
    }
    private void DisplayTime()
    {
        UnityEngine.Debug.Log($"Time Spent on Splitting: {_timeSplit:F4} ms");
        UnityEngine.Debug.Log($"Time Spent on Rooms: {_timeRooms:F4} ms");
        UnityEngine.Debug.Log($"Time Spent on Corridors: {_timeCorridors:F4} ms");

        double totalTime = _timeSplit + _timeRooms + _timeCorridors;
        UnityEngine.Debug.Log($"Total Time: {totalTime:F4} ms");
    }

#region BSP
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
    #endregion

#region Rooms
    private void GenerateRoomsInLeaves(BSPNode node)
    {
        if (node == null) return;

        if (node.IsLeaf())
        { 
            float extraPadding = _padding * 2f;

            float maxW = node.GetWidth() - extraPadding;
            float maxH = node.GetHeight() - extraPadding;

            float minW = Mathf.Min(_minWidth * 0.5f - extraPadding, maxW);
            float minH = Mathf.Min(_minHeight * 0.5f - extraPadding, maxH);

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
#endregion

#region Corridors

    private void ConnectLeafSisters(BSPNode node)
    {
        if (node == null || node.IsLeaf()) return; // return early

        // Recursion TODO maybe after??
        ConnectLeafSisters(node._leftNode);
        ConnectLeafSisters(node._rightNode);

        if (node._leftNode.IsLeaf() && node._rightNode.IsLeaf()) // are sisters leaves
        { 
            ConnectRooms(node._leftNode._actualRoom, node._rightNode._actualRoom);
        }
    } 

    private void ConnectRooms(Room a, Room b)
    {
        float overlapBottom = Mathf.Max(a.GetBottom(), b.GetBottom());
        float overlapTop = Mathf.Min(a.GetTop(), b.GetTop());
    
        if(overlapBottom < overlapTop)
        {
            CreateHorizontalCorridor(a, b, overlapBottom, overlapTop);
            return;
        }

        float overlapLeft = Mathf.Max(a.GetLeft(), b.GetLeft());
        float overlapRight = Mathf.Min(a.GetRight(), b.GetRight());

        if (overlapLeft < overlapRight)
        {
            CreateVerticalCorridor(a, b, overlapLeft, overlapRight);
            return;
        }

        BuildZSegment(a, b);
    }

     
    private Room GetRoomFromSubtree(BSPNode node)
    {
        //if (node == null) return null;
        //if (node.IsLeaf()) return node._actualRoom;

        //return Random.value > 0.5f ? GetRoomFromSubtree(node._leftNode) : GetRoomFromSubtree(node._rightNode);
        return null;
    }
     
    private void BuildZSegment(Room a, Room b)
    {
        Vector2 centerA = a.GetCenter();
        Vector2 centerB = b.GetCenter();

        float midX = (centerA.x + centerB.x) * 0.5f;

        // Z Shape
        BuildHorizontalSegment(centerA.x, midX, centerA.y); 
        BuildVerticalSegment(centerA.y, centerB.y, midX); 
        BuildHorizontalSegment(midX, centerB.x, centerB.y);
    }

    private void CreateHorizontalCorridor(Room a, Room b, float overlapBot, float overlapTop)
    {
        // Within Y Limits
        float y = Random.Range(overlapBot, overlapTop);

        float startX, endX;

        // Check Which Side
        if(a.GetCenter().x < b.GetCenter().x)
        {
            startX = a.GetRight();
            endX = b.GetLeft();
        }
        else
        {
            startX = b.GetRight();
            endX = a.GetLeft();
        }

        BuildHorizontalSegment(startX, endX, y);
    }
    private void CreateVerticalCorridor(Room a, Room b, float overlapLeft, float overlapRight)
    {
        // Within X Limits
        float x = Random.Range(overlapLeft, overlapRight);

        float startY, endY;

        // Check Which Side
        if (a.GetCenter().y < b.GetCenter().y)
        {
            startY = a.GetTop();
            endY = b.GetBottom();
        }
        else
        {
            startY = b.GetTop();
            endY = a.GetBottom();
        }

        BuildVerticalSegment(startY, endY, x);
    }

    private void BuildHorizontalSegment(float startX, float endX, float y)
    {
        float leftX = Mathf.Min(startX, endX);
        float rightX = Mathf.Max(startX, endX);

        if (rightX - leftX <= 0.01f) return;

        Corridors.Add(new Room(leftX, rightX, y + (_corridorThickness * 0.5f), y - (_corridorThickness * 0.5f)));
    }

    private void BuildVerticalSegment(float startY, float endY, float x)
    {
        float bottomY = Mathf.Min(startY, endY);
        float topY = Mathf.Max(startY, endY);

        if (topY - bottomY <= 0.01f) return;  

        Corridors.Add(new Room(x - (_corridorThickness * 0.5f), x + (_corridorThickness * 0.5f), topY, bottomY));
    }
    #endregion
}
