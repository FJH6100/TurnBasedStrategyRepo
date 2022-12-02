using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChange;
    [SerializeField]
    private Unit SelectedUnit;
    [SerializeField]
    private LayerMask UnitLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {        
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            SelectedUnit.Move(MouseWorld.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
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
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        SelectedUnit = unit;
        if (OnSelectedUnitChange != null)
            OnSelectedUnitChange(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return SelectedUnit;
    }
}
