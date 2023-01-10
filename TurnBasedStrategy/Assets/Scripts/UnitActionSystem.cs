using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    [SerializeField]
    private Unit selectedUnit;
    [SerializeField]
    private LayerMask UnitLayerMask;
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

        if (TryHandleUnitSelection()) 
            return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (isBusy)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            switch(selectedAction)
            {
                case MoveAction moveAction:
                    if (TryHandleUnitSelection()) return;
                    moveAction.Move(MouseWorld.GetPosition());
                    SetBusy();
                    break;
                case SpinAction spinAction:
                    spinAction.Spin();
                    SetBusy();
                    break;
            }    
        }
    }
    public void SetBusy()
    {
        isBusy = true;
    }

    public void ClearBusy()
    {
        isBusy = false;
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
        SetSelectedAction(unit.GetComponent<MoveAction>());
        if (OnSelectedUnitChange != null)
            OnSelectedUnitChange(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
