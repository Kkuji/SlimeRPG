using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;

public class SlimeShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bulletObject;
    [SerializeField] private float _delayBeforeSpawn;

    [Range(0.1f, 5f)]
    [SerializeField] private float _shootHeight;

    private List<GameObject> _enemies = new();
    private float _shootDistanceCounter = 0;
    private Vector3 _shootHeightVector;
    private Vector3 _targetVector;
    private Vector3 _slimeVector;
    private GameObject _currentEnemy;
    private bool _nextWave = true;
    private bool _shoot = false;
    private Slime _slime;

    [HideInInspector] public float shootSpeed = 1.2f;
    [HideInInspector] public float damage = 1;

    public event Action EnemiesDiedAction;

    public event Action StartMovingAction;

    public event Action StopMovingAction;

    private void Start()
    {
        _slimeVector = transform.position;
        _slime = GetComponent<Slime>();
    }

    private void Update()
    {
        _currentEnemy = _enemies.FirstOrDefault(gameObject => gameObject != null);

        if (!_shoot && _currentEnemy != null)
        {
            StartCoroutine(Shoot());
        }

        if (_currentEnemy == null && _nextWave)
        {
            _nextWave = false;
            StartMovingAction?.Invoke();
            Invoke(nameof(NewEnemiesWave), _delayBeforeSpawn);
        }
    }

    private void NewEnemiesWave()
    {
        _enemies.Clear();
        EnemiesDiedAction?.Invoke();
        _nextWave = true;
        StopMovingAction?.Invoke();
    }

    private IEnumerator Shoot()
    {
        SetPreShootingValues();

        GameObject _currentBullet = Instantiate(_bulletObject, transform.position, Quaternion.identity);
        _currentBullet.GetComponent<Bullet>().damage = damage;

        while (_shootDistanceCounter < 1)
        {
            _shootDistanceCounter += shootSpeed * Time.deltaTime;

            Vector3 firstHalfDistance = Vector3.Lerp(_slimeVector, _shootHeightVector, _shootDistanceCounter);
            Vector3 SecondHalfDistance = Vector3.Lerp(_shootHeightVector, _targetVector, _shootDistanceCounter);

            if (_currentBullet != null)
            {
                _currentBullet.transform.position = Vector3.Lerp(firstHalfDistance, SecondHalfDistance, _shootDistanceCounter);
            }
            yield return new WaitForEndOfFrame();
        }

        SetPostShootingValues();
    }

    private void SetPreShootingValues()
    {
        _shoot = true;
        _targetVector = _currentEnemy.transform.position;
        _shootHeightVector = _slimeVector + (_targetVector - _slimeVector) / 2 + Vector3.up * _shootHeight;
    }

    private void SetPostShootingValues()
    {
        _shootDistanceCounter = 0;
        _shoot = false;
    }

    public void SetEnemies(List<GameObject> spawnedEnemies)
    {
        _enemies = spawnedEnemies;
    }
}