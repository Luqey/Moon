using Moon;
using UnityEngine;

public class GridObject : MonoBehaviour {
  private void Start() {
    var gridPosition = Services.Grid.Insert(transform.position, gameObject);
    transform.position = Services.Grid.FromGrid(gridPosition);
  }
}
