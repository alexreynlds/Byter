using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public int

                columns,
                rows;

        public float

                verticalOffset,
                horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public bool isBossRoom = false;

    public List<Vector2> availablePositions = new List<Vector2>();

    void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.columns = room.roomW - 4;
        grid.rows = room.roomH - 4;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        if (!isBossRoom)
        {
            grid.verticalOffset += room.transform.localPosition.y;
            grid.horizontalOffset += room.transform.localPosition.x;

            for (int y = 0; y < grid.rows; y++)
            {
                for (int x = 0; x < grid.columns; x++)
                {
                    GameObject go = Instantiate(gridTile, transform);
                    go.transform.position =
                        new Vector2(x - (grid.columns - grid.horizontalOffset),
                            y - (grid.rows - grid.verticalOffset));
                    go.name = "X: " + x + ", Y: " + y;
                    availablePositions.Add(go.transform.position);
                    go.SetActive(false);
                }
            }
            GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
        }
        else
        {
            grid.verticalOffset += room.transform.localPosition.y;
            grid.horizontalOffset += room.transform.localPosition.x;

            GameObject go = Instantiate(gridTile, transform);
            go.transform.position = new Vector2(transform.position.x, transform.position.y);
            go.name = "X: " + 0 + ", Y: " + 0;
            go.SetActive(false);
            availablePositions.Add(go.transform.position);
            GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
        }

    }
}
