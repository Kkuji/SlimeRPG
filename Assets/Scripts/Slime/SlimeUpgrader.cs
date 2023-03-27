using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SlimeUpgrader : MonoBehaviour
{
    [SerializeField] private GameObject[] _upgradeButtons = new GameObject[3];
    [SerializeField] private int _stepIncreasePrice;
    [SerializeField] private float _powerMultiplier;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _healthMultiplier;

    private int[] _upgradePrices = new int[3];

    private SlimeShooting _slimeShooting;
    private Slime _slime;

    private void Start()
    {
        _slimeShooting = GetComponent<SlimeShooting>();
        _slime = GetComponent<Slime>();

        for (int i = 0; i < _upgradePrices.Length; i++)
        {
            _upgradePrices[i] = 1;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            if (_slime.points < _upgradePrices[i])
            {
                _upgradeButtons[i].GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                _upgradeButtons[i].GetComponentInChildren<Button>().interactable = true;
            }
        }
    }

    public void Power()
    {
        UpdatePoints(0);

        float longDamage = _slimeShooting.damage * _powerMultiplier;
        _slimeShooting.damage = (float)(Math.Round((double)longDamage, 2));
    }

    public void Speed()
    {
        UpdatePoints(1);

        float longSpeed = _slimeShooting.shootSpeed * _speedMultiplier;
        _slimeShooting.shootSpeed = (float)(Math.Round((double)longSpeed, 2));
    }

    public void Health()
    {
        UpdatePoints(2);

        float longHealth = _slime.maxHealth * _healthMultiplier;
        _slime.maxHealth = (float)(Math.Round((double)longHealth, 2));
    }

    private void UpdatePoints(int perkIndex)
    {
        _slime.points -= _upgradePrices[perkIndex];
        _upgradePrices[perkIndex] += _stepIncreasePrice;
        _upgradeButtons[perkIndex].GetComponentInChildren<TextMeshProUGUI>().SetText(_upgradePrices[perkIndex] + " points");
    }
}