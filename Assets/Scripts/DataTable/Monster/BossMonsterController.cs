using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMonsterController : MonoBehaviour
{
    private Animator animator;
    private GameUI gameUI;
    private PlayerController playerController;

    private float currentHealth; // ���� ü�¹�
    private float maxHealth; //�ִ� ü��

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        gameUI = FindObjectOfType<GameUI>();
        animator = GetComponent<Animator>();
        maxHealth = StageManager.instance.bossMonsterHP;
        currentHealth = maxHealth;

        gameUI.BossHPBar.value = gameUI.BossHPBar.maxValue;
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
        StartCoroutine(DeadMotion());
        ResourceManager.instance.AddResource(ResourceManager.ResourceType.Stone, StageManager.instance.bossMonsterDropResource);
        // ��� �� ��ŭ �ڿ��߰�

        BackgroundScrolling backgroundScrolling = FindObjectOfType<BackgroundScrolling>();
        backgroundScrolling.StartScrolling();
        gameUI.MovingStage();
        StageManager.instance.stageLevel++;
        playerController.targerPosition = 0;
        playerController.BossBattleModeEnd();

    }

    private void UpdateHealthUI()
    {
        if (gameUI.BossHPBar != null)
        {
            // �����̴��� ���� ������Ʈ�Ͽ� ���� ü���� �ݿ��մϴ�.
            gameUI.BossHPBar.value = (float)(currentHealth / maxHealth)*100f;
        }
    }
    IEnumerator DamageMotion()
    {
        // �ǰ� �ִϸ��̼� ���
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
