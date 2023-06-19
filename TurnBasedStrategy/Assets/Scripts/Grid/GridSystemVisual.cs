using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField]
    private Transform gridSystemVisualSinglePrefab;

    private Transform[,] gridSquares;

    private void Start()
    {
        gridSquares = new Transform[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSquare = Instantiate(gridSystemVisualSinglePrefab, 
                    LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSquares[x, z] = gridSquare;
            }
        }
    }

    private void Update()
    {
        HideAllGridPositions();
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            if (selectedAction != null)
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
        }
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSquares[x, z].Find("Quad").GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSquares[gridPosition.x, gridPosition.z].Find("Quad").GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
