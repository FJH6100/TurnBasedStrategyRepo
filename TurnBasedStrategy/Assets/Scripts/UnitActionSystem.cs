using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    public event EventHandler OnSelectedActionChange;
    public event EventHandler OnActionTaken;
    public event EventHandler OnActionPointsRestore;
    [SerializeField]
    private Unit selectedUnit;
    [SerializeField]
    private LayerMask UnitLayerMask;
    [SerializeField]
    private GameObject busyUI;
    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        Instance = this;
        isBusy = false;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;
        //check touch
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return;
        }
        if (TryHandleUnitSelection()) 
            return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (isBusy)
            return;
        if (Input.GetMouseButtonDown(0) && selectedUnit.GetActionPoints() > 0)
        {
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (selectedAction.TakeAction(gridPosition))
            {
                selectedUnit.SubtractActionPoint();
                OnActionTaken(this, EventArgs.Empty);
            }
            
        }
    }
    public void SetBusy()
    {
        isBusy = true;
        busyUI.SetActive(true);
    }

    public void ClearBusy()
    {
        isBusy = false;
        busyUI.SetActive(false);
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, UnitLayerMask))
            {
                if (hit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    //Unit already selected
                    if (unit.IsEnemy() || selectedUnit == unit)
                    {
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        if (ActionHideButton.Instance.isHidden)
            SetSelectedAction(null);
        else
            SetSelectedAction(unit.GetComponent<MoveAction>());
        if (OnSelectedUnitChange != null)
            OnSelectedUnitChange(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        if (OnSelectedActionChange != null)
            OnSelectedActionChange(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
    
    public void ActionPointRefresh()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            foreach (Unit u in Resources.FindObjectsOfTypeAll<Unit>())
            {
                if (!u.IsEnemy())
                    u.RestoreActionPoints();
            }
            OnActionPointsRestore(this, EventArgs.Empty);
        }
        else
        {
            EnemyAI.Instance.EnemyTurn();
            foreach (Unit u in Resources.FindObjectsOfTypeAll<Unit>())
            {
                if (u.IsEnemy())
                    u.RestoreActionPoints();
            }
        }
    }
}
