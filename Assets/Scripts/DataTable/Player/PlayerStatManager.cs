using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    private static object _lock = new object();
    private static PlayerStatManager _instance = null;
    public static PlayerStatManager instance
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
                    GameObject obj = new GameObject("PlayerStatManager ");
                    obj.AddComponent<PlayerStatManager>();
                    _instance = obj.GetComponent<PlayerStatManager>();
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
    public float playerPower = 10;
    public float playerCoolDown = 3f;
    public int PowerLevel = 1; // ���ݷ� ����
    public int CoolDownLevel = 1; // ���ݼӵ� ����


    public void AddPower(float count)
    {
        // ���ݷ� ���׷��̵�
        for(int i=0; i < count; i++)
        {
            playerPower += playerPower * 0.002f;

        }
    }

    public void AddCoolDown(float count)
    {
        // ���ݼӵ� ���׷��̵�
        for (int i = 0; i < count; i++)
        {
            playerCoolDown -= playerCoolDown * 0.0005f;
        }
    }

    public int GetPowerLevelAmount()
    {
        // ���ݷ� ���� ��ȯ
        if (ResourceManager.instance.consumeAble == true)
            PowerLevel = PowerLevel + EnhanceManager.instance.upgradeCount;
        else
            return PowerLevel;
        return PowerLevel;
    }
    public int GetCoolDownLevelAmount()
    {
        // ���ݼӵ� ���� ��ȯ
        if (ResourceManager.instance.consumeAble == true)
            CoolDownLevel = CoolDownLevel + EnhanceManager.instance.upgradeCount;
        else
            return CoolDownLevel;
        return CoolDownLevel;
    }

    public float GetPowerAmount()
    {
        // ��ȭ �� ������ ��ȯ
        float statAmount = playerPower;
        for (int i = 0; i < EnhanceManager.instance.upgradeCount - 1; i++)
        {
            statAmount = statAmount + (statAmount * 0.001f);
        }
        return statAmount;
    }
    public float GetCooldownAmount()
    {
        // ��ȭ �� ������ ��ȯ
        float statAmount = playerCoolDown;
        for (int i = 0; i < EnhanceManager.instance.upgradeCount - 1; i++)
        {
            statAmount = statAmount - (statAmount * 0.0005f);
        }
        return statAmount;
    }
    private IEnumerator SetValue()
    {
        yield return new WaitForSeconds(0.5f);
        playerPower = BackendGameData.Instance.UserGameData.Power;
        playerCoolDown = BackendGameData.Instance.UserGameData.Speed;
        PowerLevel = BackendGameData.Instance.UserGameData.PowerLevel;
        CoolDownLevel = BackendGameData.Instance.UserGameData.SpeedLevel;
        GameUI gameUI = FindObjectOfType<GameUI>();
        gameUI.SettingAPLevelText();
        gameUI.SettingASLevelText();
        // �ɷ�ġ ���� �� ����
    }
}
