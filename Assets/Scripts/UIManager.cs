using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private Image boostChargeFill;
    [SerializeField] private Image boostChargeCooldown;
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    void Update()
    {
        BoostCharge();
    }

    private void BoostCharge()
    {
        if (_player.GetBoostCooldown()>0)
        {
            boostChargeCooldown.enabled = true;
            boostChargeCooldown.fillAmount = _player.GetBoostCooldown();
            boostChargeFill.enabled = false;
        }
        else{
            boostChargeFill.enabled = true;
            boostChargeFill.fillAmount = _player.GetBoostCharge();
            boostChargeCooldown.enabled = false;
        }

    }
}
