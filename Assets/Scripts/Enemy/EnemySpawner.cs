using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _enemyDistanceXAddSpawn;
    [SerializeField] private float _enemyDistanceYAddSpawn;
    [SerializeField] private float _enemyDistanceXSpawn;
    [SerializeField] private int _amountEnemies;
    [SerializeField] private GameObject _enemyObject;
    [SerializeField] private GameObject _slimeObject;

    private List<GameObject> _enemies = new();
    private SlimeShooting _slimeShooting;
    private Slime _slime;

    private void Awake()
    {
        _slimeShooting = _slimeObject.GetComponent<SlimeShooting>();
        _slimeShooting.EnemiesDiedAction += SpawnEnemies;
    }

    private void Start()
    {
        _slime = _slimeObject.GetComponent<Slime>();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < _amountEnemies; i++)
        {
            SpawnEnemy();
        }
        _slimeShooting.SetEnemies(_enemies);
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(_enemyObject, GetRandomVectorSpawn(), Quaternion.identity);
        newEnemy.GetComponent<Enemy>().slime = _slime;

        _enemies.Add(newEnemy);
    }

    private Vector3 GetRandomVectorSpawn()
    {
        float xPostionAdd = UnityEngine.Random.Range(-_enemyDistanceXAddSpawn, _enemyDistanceXAddSpawn);
        float yPostionAdd = UnityEngine.Random.Range(-_enemyDistanceYAddSpawn, _enemyDistanceYAddSpawn);

        Vector3 randomVectorEnemy = new(_slime.transform.position.x + _enemyDistanceXSpawn + xPostionAdd,
                                        _slime.transform.position.y + yPostionAdd,
                                        _slime.transform.position.z);
        return randomVectorEnemy;
    }

    private void OnDestroy()
    {
        _slimeShooting.EnemiesDiedAction -= SpawnEnemies;
    }
}