using System.Collections.Generic;
using UnityEngine;

namespace Moon {
  public class GridManager : MonoBehaviour {
    public float gridSize = 3;
    private readonly Dictionary<Vector2Int, GameObject> grid = new();

    private void Awake() {
      Services.Grid = this;
    }

    public Vector2Int ToGrid(Vector3 worldPosition) {
      return new(Mathf.RoundToInt(worldPosition.x / gridSize), Mathf.RoundToInt(worldPosition.z / gridSize));
    }

    public Vector3 FromGrid(Vector2Int gridPosition, float withY = 0f) {
      return new(gridPosition.x * gridSize, withY, gridPosition.y * gridSize);
    }

    public Vector2Int Insert(Vector3 worldPosition, GameObject o) {
      Vector2Int gridPosition = ToGrid(worldPosition);
      Insert(gridPosition, o);
      return gridPosition;
    }

    public void Insert(Vector2Int gridPosition, GameObject o) {
      grid.Add(gridPosition, o);
    }

    public bool Remove(Vector2Int gridPosition, out GameObject o) {
      return grid.Remove(gridPosition, out o);
    }

    public bool Remove(Vector3 worldPosition, out GameObject o) {
      return grid.Remove(ToGrid(worldPosition), out o);
    }

    public bool Find(Vector3 worldPosition, out Vector2Int gridPosition) {
      gridPosition = ToGrid(worldPosition);
      return grid.ContainsKey(gridPosition);
    }

    public void Move(Vector3 worldPosition, Vector2Int direction) {
      var gridPosition = ToGrid(worldPosition);
      Move(gridPosition, direction);
    }

    public void Move(Vector2Int gridPosition, Vector2Int direction) {
      if (Remove(gridPosition, out var o)) {
        Insert(gridPosition + direction, o);
      }
    }
  }
}

