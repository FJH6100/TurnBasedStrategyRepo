using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    private Transform cameraTransform;
    private Unit unit;
    [SerializeField]
    private Image healthBarImage;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        unit = transform.parent.GetComponent<Unit>();
    }

    private void Start()
    {
        UpdateHealth();
        unit.OnDamaged += Unit_OnDamaged;
    }

    private void LateUpdate()
    {
        transform.LookAt(cameraTransform);
    }

    private void UpdateHealth()
    {
        float healthPercentage = unit.ReturnNormalizedHealth();
        healthBarImage.fillAmount = healthPercentage;
    }

    private void Unit_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealth();
    }

}
