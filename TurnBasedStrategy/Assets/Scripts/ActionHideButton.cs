using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionHideButton : MonoBehaviour
{
    public GameObject actionUI;
    public static ActionHideButton Instance { get; private set; }
    [HideInInspector]
    public bool isHidden;
    private void Awake()
    {
        Instance = this;
        isHidden = false;
    }
    public void ShowOrHideActionList()
    {
        actionUI.SetActive(!actionUI.activeSelf);
        isHidden = !isHidden;
        if (actionUI.activeSelf == false)
        {
            UnitActionSystem.Instance.SetSelectedAction(null);
            isHidden = true;
            GetComponentInChildren<Image>().transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.GetSelectedUnit().GetComponent<MoveAction>());
            isHidden = false;
            GetComponentInChildren<Image>().transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
