using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointText;
    private List<ActionButtonUI> actionButtionUIList;

    private void Awake()
    {
        actionButtionUIList = new List<ActionButtonUI>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionTaken += UnitActionSystem_OnActionTaken;
        UnitActionSystem.Instance.OnActionPointsRestore += UnitActionSystem_OnActionPointsRestore;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    private void CreateUnitActionButtons()
    {
        foreach (Transform button in actionButtonContainer)
        {
            Destroy(button.gameObject);
        }

        actionButtionUIList.Clear();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtionUIList.Add(actionButtonUI);
        }
    }
    public void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtionUI in actionButtionUIList)
            actionButtionUI.UpdateSelectedVisual();
    }
    public void UpdateActionPoints()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnActionTaken(object sender, EventArgs e)
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        selectedUnit.SubtractActionPoint();
        UpdateActionPoints();
    }
    private void UnitActionSystem_OnActionPointsRestore(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
}
