using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSettings : MonoBehaviour
{
    public Slider monsterHPBar; // ü�� ��
    public MonsterDataTable monsterDataTable;
    private Animator animator;

    private float currentHealth; // ���� ü�¹�
    private float maxHealth; //�ִ� ü��

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        maxHealth = monsterDataTable.monsterHP;
        currentHealth = maxHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(DamageMotion());
        

        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.targerPosition++;
        
        // ���� Ÿ������ ����
        StartCoroutine(DeadMotion());

        ResourceManager.instance.AddResource(ResourceManager.ResourceType.Stone, monsterDataTable.dropResorceAmount);
        // ��� �� ��ŭ �ڿ��߰�
    }

    private void UpdateHealthUI()
    {
        if (monsterHPBar != null)
        {
            // �����̴��� ���� ������Ʈ�Ͽ� ���� ü���� �ݿ��մϴ�.
            monsterHPBar.value = (float)(currentHealth / maxHealth)*100f;
        }
    }
    IEnumerator DamageMotion()
    {
        // ���� �ִϸ��̼� ���
        animator.SetTrigger("Damage");
        yield return new WaitForSeconds(0.01f); // ���� �ӵ��� ���� ���
        UpdateHealthUI();
        animator.SetTrigger("Idle");
    }
    IEnumerator DeadMotion()
    {
        // ���� �ִϸ��̼� ���
        animator.SetTrigger("Dead");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
