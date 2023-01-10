using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float spinDegrees;
    protected override void Awake()
    {
        base.Awake();
        spinDegrees = 0;
    }
    public void Spin()
    {
        isActive = true;
    }

    public override string GetActionName()
    {
        return "SPIN";
    }
    private void Update()
    {
        if (isActive)
        {
            float spinAmount = 720f * Time.deltaTime;
            spinDegrees += spinAmount;
            transform.eulerAngles += new Vector3(0, spinAmount, 0);
            if (spinDegrees >= 360)
            {
                isActive = false;
                spinDegrees = 0;
                UnitActionSystem.Instance.ClearBusy();
            }
        }
    }
}
