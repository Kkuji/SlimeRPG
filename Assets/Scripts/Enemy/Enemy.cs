using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _healthBarObjectPrefab;
    [SerializeField] private GameObject _healthText;
    [SerializeField] private float _attackFrequency;
    [SerializeField] private float _speed;

    private GameObject _healthBarObject;
    private bool _collidedSlime = false;
    private float _lastAttackTime;
    private float _currentHealth;
    private Rigidbody _rigidbody;
    private Image _healthBar;

    [HideInInspector] public float _maxHealth;
    [HideInInspector] public float _damage;
    [HideInInspector] public Slime slime;

    private void Start()
    {
        CreateHealthBar();

        _rigidbody = GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_healthBarObject != null)
        {
            MoveHealthBar();
        }
    }

    private void FixedUpdate()
    {
        if (!_collidedSlime)
        {
            _rigidbody.velocity = Vector3.left * _speed;
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;
        InstantiateHealthText(damage);

        CheckHealthBar();
        CheckIsDead();
    }

    public void CheckIsDead()
    {
        if (_currentHealth < 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Slime slime))
        {
            _collidedSlime = true;
            DamageSlime(slime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Slime slime))
        {
            if (slime.gameObject != null && Time.time > _lastAttackTime + _attackFrequency)
            {
                DamageSlime(slime);
            }
        }
    }

    private void DamageSlime(Slime slime)
    {
        slime.GetDamage(_damage);
        _lastAttackTime = Time.time;
    }

    private void CheckHealthBar()
    {
        if (_healthBarObject.activeSelf == false)
        {
            _healthBarObject.SetActive(true);
            _healthBar = _healthBarObject.GetComponentInChildren<Image>();
        }

        _healthBar.fillAmount = Mathf.Clamp(_currentHealth / _maxHealth, 0, 1f);
    }

    private void Die()
    {
        slime.AddPoints();
        Destroy(_healthBarObject);
        Destroy(gameObject);
    }

    private void CreateHealthBar()
    {
        _healthBarObject = Instantiate(_healthBarObjectPrefab);
        RectTransform textTransform = _healthBarObject.GetComponent<RectTransform>();
        SetUIPosition(textTransform);
        _healthBarObject.SetActive(false);
    }

    private void SetUIPosition(RectTransform rectTransform)
    {
        rectTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        rectTransform.SetParent(canvas.transform);
    }

    public void InstantiateHealthText(float damage)
    {
        RectTransform textTransform = Instantiate(_healthText).GetComponent<RectTransform>();
        textTransform.gameObject.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        SetUIPosition(textTransform);
    }

    public void MoveHealthBar()
    {
        RectTransform textTransform = _healthBarObject.GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}