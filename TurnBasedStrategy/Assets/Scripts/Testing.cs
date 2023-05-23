using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            GridPosition startPosition = new GridPosition(0, 0);

            List<GridPosition> gridPositionList = Pathfinding.Instance.FindPath(startPosition, mouseGridPosition, out int pathLength);
            Debug.Log(gridPositionList.Count);
            for (int i = 0; i < gridPositionList.Count - 1; i++)
            {
                Debug.DrawLine(
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
                    Color.red,
                    5f,
                    false
                );
            }
        }
    }
}
