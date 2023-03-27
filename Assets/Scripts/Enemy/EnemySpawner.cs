using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _enemyDistanceXAddSpawn;
    [SerializeField] private float _enemyDistanceYAddSpawn;
    [SerializeField] private float _enemyDistanceXSpawn;
    [SerializeField] private float _enemyPerkMultiplier;
    [SerializeField] private int _amountEnemies;
    [SerializeField] private GameObject _enemyObject;
    [SerializeField] private GameObject _slimeObject;

    private List<GameObject> _enemies = new();
    private Enemy _enemyScript;
    private float _enemyDamage;
    private float _enemyMaxHealth;
    private SlimeShooting _slimeShooting;
    private Slime _slime;

    private void Awake()
    {
        _slimeShooting = _slimeObject.GetComponent<SlimeShooting>();
        _slimeShooting.EnemiesDiedAction += SpawnEnemies;

        _enemyScript = _enemyObject.GetComponent<Enemy>();

        _enemyDamage = _enemyScript._damage;
        _enemyMaxHealth = _enemyScript._maxHealth;
    }

    private void Start()
    {
        _slime = _slimeObject.GetComponent<Slime>();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        _enemyDamage *= _enemyPerkMultiplier;
        _enemyMaxHealth *= _enemyPerkMultiplier;

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
        Enemy currentEnemyScript = newEnemy.GetComponent<Enemy>();

        currentEnemyScript._damage = (float)(Math.Round((double)_enemyDamage, 2));
        currentEnemyScript._maxHealth = (float)(Math.Round((double)_enemyMaxHealth, 2));

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