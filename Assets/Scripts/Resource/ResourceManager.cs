using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InfiniteValue;


public class ResourceManager : MonoBehaviour
{
    private static object _lock = new object();
    private static ResourceManager _instance = null;
    public static ResourceManager instance
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
                    GameObject obj = new GameObject("ResourceManager ");
                    obj.AddComponent<ResourceManager>();
                    _instance = obj.GetComponent<ResourceManager>();
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


    [SerializeField]
    private InfVal currentStone; // ���� ��� ��

    [SerializeField]
    private InfVal currentDiamond; // ���� ���̾� ��

    private string startStone = "0"; // ���� ��� ��
    private string startDiamond = "0"; // ���� ���̾� ��

    public bool consumeAble = false;

    public event Action<ResourceType, InfVal> OnResourceChanged; // �� ��ȯ �̺�Ʈ

    public enum ResourceType
    {
        Stone,
        Diamond
    }
    // �ڿ� Ÿ��
    private void Awake()
    {
        _instance = this;
        // �̱��� �ν��Ͻ�


    }
    private void Start()
    {
        StartCoroutine(SetValue());
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
        // �ν��Ͻ� ����
    }

    public InfVal Stone
    {
        get { return currentStone; }
        set
        {
            // ���� ����� ��쿡�� �� ��ȯ �ִϸ��̼� ����
            if (value != currentStone)
            {
                StartCoroutine(IncreaseResourceValue(ResourceType.Stone, currentStone, value));
            }
            currentStone = value;

        }
    }
    // ���� ������Ƽ


    public InfVal Diamond
    {
        get { return currentDiamond; }
        set
        {
            if (value != currentDiamond) // ���� ����� ��쿡�� �ִϸ��̼� ����
            {
                StartCoroutine(IncreaseResourceValue(ResourceType.Diamond, currentDiamond, value));
            }
            currentDiamond = value;

        }
    }
    // ���̾� ������Ƽ

    public InfVal GetResourceValue(ResourceType resourceType)
    {
        // �ڿ� �� �б� �޼���
        switch (resourceType)
        {
            case ResourceType.Stone:
                return Stone;

            case ResourceType.Diamond:
                return Diamond;

            default:
                return 0;
        }
    }
    public void AddResource(ResourceType resourceType, InfVal amount)
    {
        // �ڿ� ���� �޼���
        switch (resourceType)
        {
            case ResourceType.Stone:
                Stone += amount;
                break;

            case ResourceType.Diamond:
                Diamond += amount;
                break;
        }
    }

    public void RemoveResource(ResourceType resourceType, InfVal amount)
    {
        // �ڿ� ���� �޼���
        switch (resourceType)
        {
            case ResourceType.Stone:
                if (consumeAble)
                {
                    Stone -= amount;
                }
                break;

            case ResourceType.Diamond:
                if (consumeAble)
                {
                    Diamond -= amount;
                }
                break;
        }
    }

    public void CheckResourceAmount(ResourceType resourceType, InfVal amount)
    {
        // �ڿ� ���� �� Ȯ�� �޼���
        switch (resourceType)
        {
            case ResourceType.Stone:
                if (currentStone >= amount)
                {
                    consumeAble = true;
                }
                else
                {
                    consumeAble = false;
                    ToastMessage.I.ShowToastMessage("������ �����մϴ�!", ToastMessage.ToastLength.Short);
                    Debug.Log($"{resourceType}�� �����մϴ�.");
                }
                break;

            case ResourceType.Diamond:
                if (currentDiamond >= amount)
                {
                    consumeAble = true;
                }
                else
                {
                    consumeAble = false;
                    ToastMessage.I.ShowToastMessage("������ �����մϴ�!", ToastMessage.ToastLength.Short);
                    Debug.Log($"{resourceType}�� �����մϴ�.");
                }
                break;
        }
    }

    private IEnumerator IncreaseResourceValue(ResourceType resourceType, InfVal startValue, InfVal endValue)
    {
        float duration = 1.0f; // �ִϸ��̼� ���� �ð� (��)
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            InfVal newValue = MathInfVal.Round(Mathf.Lerp((float)startValue, (float)endValue, t));
            OnResourceChanged?.Invoke(resourceType, newValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OnResourceChanged?.Invoke(resourceType, endValue);
    }

    private IEnumerator SetValue()
    {
        yield return new WaitForSeconds(0.5f);
        startStone = BackendGameData.Instance.UserGameData.Stone;
        startDiamond = BackendGameData.Instance.UserGameData.Diamond;
        currentStone = InfVal.Parse(startStone);
        currentDiamond = InfVal.Parse(startDiamond);
        // �ڿ� ���� �� ����
    }
}

