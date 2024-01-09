using InfiniteValue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnhanceManager : MonoBehaviour
{
    private static object _lock = new object();
    private static EnhanceManager _instance = null;
    public static EnhanceManager instance
    {
        get
        {
            if (applicationQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("EnhanceManager ");
                    obj.AddComponent<EnhanceManager>();
                    _instance = obj.GetComponent<EnhanceManager>();
                }
                return _instance;
            }
        }
        set
        {
            _instance = value;
        }
    }
    private static bool applicationQuitting = false;
    // �̱���

    private void Awake()
    {
        _instance = this;
        // �̱��� �ν��Ͻ�
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
        // �ν��Ͻ� ����
    }
    private void Start()
    {
        StartCoroutine(SetValue());
    }
    [HideInInspector] public InfVal powerResourceAmount = 100; //���ҽ�ų �ڿ��� �� (�Ŀ�)
    [HideInInspector] public InfVal cooldownResourceAmount = 100; //���ҽ�ų �ڿ��� �� (���ݼӵ�)
    [HideInInspector] public int upgradeCount = 1; //���׷��̵�Ƚ��  
    public void EnhancePower()
    {
        InfVal increaseAmount = powerResourceAmount;
        //���ݷ� ���� �Լ�
        for(int i=0; i<upgradeCount; i++)
        {
            increaseAmount = increaseAmount + (increaseAmount * 0.01);
        }
        ResourceManager.instance.CheckResourceAmount(ResourceManager.ResourceType.Stone, increaseAmount);
        if (ResourceManager.instance.consumeAble == true)
        {
            powerResourceAmount = increaseAmount;
            ResourceManager.instance.RemoveResource(ResourceManager.ResourceType.Stone, powerResourceAmount);
            PlayerStatManager.instance.AddPower(upgradeCount);

            GameUI gameUI = FindObjectOfType<GameUI>();
            gameUI.SettingAPUpgradeText(); // UI ����
        }
    }
    public void Enhancecooldown()
    {
        InfVal increaseAmount = cooldownResourceAmount;
        //���ݼӵ� ���� �Լ�
        for (int i = 0; i < upgradeCount; i++)
        {
            increaseAmount = increaseAmount + (increaseAmount * 0.01);
        }
        ResourceManager.instance.CheckResourceAmount(ResourceManager.ResourceType.Stone, increaseAmount);
        if(ResourceManager.instance.consumeAble == true)
        {
            cooldownResourceAmount = increaseAmount;
            ResourceManager.instance.RemoveResource(ResourceManager.ResourceType.Stone, cooldownResourceAmount);
            PlayerStatManager.instance.AddCoolDown(upgradeCount);

            GameUI gameUI = FindObjectOfType<GameUI>();
            gameUI.SettingASUpgradeText(); // UI ����
        }
    }
    public void PowerResourceIncrease(InfVal resourceAmount)
    {
        powerResourceAmount += resourceAmount;
    }
    public void CooldownResourceIncrease(InfVal resourceAmount)
    {
        cooldownResourceAmount += resourceAmount;
    }

    public void SetUpgradeCount1()
    {
        //���׷��̵�Ƚ���� 1�� ����
        upgradeCount = 1;
    }
    public void SetUpgradeCount10() 
    {
        //���׷��̵�Ƚ���� 10���� ����
        upgradeCount = 10;
    }
    public void SetUpgradeCount100()     
    {
        //���׷��̵�Ƚ���� 100���� ����
        upgradeCount = 100;
    }
    public InfVal GetResourceIncrease(InfVal resourceType)
    {
        // ��ȭ�� �ʿ��� �ڿ��� ��ȯ
        InfVal increaseAmount = resourceType;
        for (int i = 0; i < upgradeCount; i++)
        {
            increaseAmount = increaseAmount + (increaseAmount * 0.01);
        }
        return increaseAmount;
    }

    private IEnumerator SetValue()
    {
        yield return new WaitForSeconds(0.5f);
        powerResourceAmount = InfVal.Parse(BackendGameData.Instance.UserGameData.PRA);
        cooldownResourceAmount = InfVal.Parse(BackendGameData.Instance.UserGameData.SPA);
        GameUI gameUI = FindObjectOfType<GameUI>();
        gameUI.SettingAPUpgradeText();
        gameUI.SettingASUpgradeText();
        // �ڿ� ���� �� ����
    }

}
