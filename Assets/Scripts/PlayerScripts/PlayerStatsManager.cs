using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsManager : MonoBehaviour
{
    #region Singleton

    public static PlayerStatsManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Multiple PlayerStatsManagers In Scene");
            Destroy(gameObject);
        }
    }

    #endregion


    [Header("Health Variables")]
    public Slider UIHealthSlider;
    public Text UIHealthText;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    [Header("Armour Variables")]

    [Header("Hunger Variables")]
    public Slider UIHungerSlider;
    public Text UIHungerText;
    [SerializeField] float maxHunger = 100;
    [SerializeField] float currentHunger;
    [SerializeField] float hungerTime = 4;
    [SerializeField] bool hungerActive = true;



    public float currentArmour;

    private void Start()
    {
        InitializeHealth();
        InitializeHunger();
    }

    private void Update()
    {
        if (hungerActive)
        {
            HungerTick();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        UIHungerSlider.value = currentHunger;
        UIHungerText.text = ("Hunger : " + Mathf.CeilToInt(currentHunger));

        UIHealthSlider.value = currentHealth;
        UIHealthText.text = ("Health : " + Mathf.CeilToInt(currentHealth));
    }


    #region Health Methods
    void InitializeHealth()
    {
        UIHealthSlider.maxValue = maxHealth;
        currentHealth = maxHealth;
    }

    public void AdjustHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth += value, 0, maxHealth);
    }


    #endregion

    #region Armour Methods

    #endregion

    #region HungerMethods
    void InitializeHunger()
    {
        UIHungerSlider.maxValue = maxHunger;
        currentHunger = maxHunger;
    }

    public void AdjustHunger(float value)
    {
        currentHunger = Mathf.Clamp(currentHunger += value, 0, maxHunger);
    }

    void HungerTick()
    {
        currentHunger -= (1 / hungerTime * Time.deltaTime);
        Mathf.Clamp(currentHunger, 0, maxHunger);

        if (currentHunger <= 0)
        {
            AdjustHealth(-10);
        }
    }
    #endregion
}
