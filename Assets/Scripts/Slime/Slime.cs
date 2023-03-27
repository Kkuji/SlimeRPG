using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Slime : MonoBehaviour, IDamageable
{
    [SerializeField] private TextMeshProUGUI _pointsUI;
    [SerializeField] private GameObject _healthText;
    [SerializeField] private GameObject _healthBar;

    private SlimeShooting _slimeShooting;
    private int _pointsForCube = 1;
    private bool _canMove = false;
    private float _currentHealth;
    private Animator _animator;

    public float maxHealth = 15;
    public int points = 0;

    private void Awake()
    {
        _slimeShooting = GetComponent<SlimeShooting>();

        _slimeShooting.StartMovingAction += WaveEnded;
        _slimeShooting.StopMovingAction += WaveStarted;
    }

    private void Start()
    {
        MoveHealthBar();

        _currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_canMove)
        {
            MoveHealthBar();
        }

        _pointsUI.SetText("Points: " + points.ToString());
    }

    public void GetDamage(float damage)
    {
        _currentHealth -= damage;
        InstantiateHealthText(damage);
        _healthBar.GetComponentInChildren<Image>().fillAmount = Mathf.Clamp(_currentHealth / maxHealth, 0, 1f);
        CheckIsDead();
    }

    public void CheckIsDead()
    {
        if (_currentHealth < 0)
        {
            _healthBar.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    private void WaveEnded()
    {
        _currentHealth = maxHealth;
        _healthBar.GetComponentInChildren<Image>().fillAmount = Mathf.Clamp(_currentHealth / maxHealth, 0, 1f);
        _pointsForCube++;
        SetWaveEndedDefaults(true);
    }

    private void WaveStarted()
    {
        SetWaveEndedDefaults(false);
    }

    private void SetWaveEndedDefaults(bool condition)
    {
        _canMove = condition;
        _animator.SetBool("Move", condition);
    }

    private void OnDestroy()
    {
        _slimeShooting.StartMovingAction -= WaveEnded;
        _slimeShooting.StopMovingAction -= WaveStarted;
    }

    public void MoveHealthBar()
    {
        RectTransform textTransform = _healthBar.GetComponent<RectTransform>();
        textTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void AddPoints()
    {
        points += _pointsForCube;
    }

    public void InstantiateHealthText(float damage)
    {
        RectTransform textTransform = Instantiate(_healthText).GetComponent<RectTransform>();
        textTransform.gameObject.GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
        textTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        textTransform.SetParent(canvas.transform);
    }
}