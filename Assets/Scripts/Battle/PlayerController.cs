using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private MonsterSettings monsterSettings;
    [HideInInspector]
    public int targerPosition = 0;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        BattleModeStart();
    }
    public void ResetAnimator()
    {
        animator = GetComponentInChildren<Animator>();
        BattleModeStart();
    }
    private void AttackTarget()
    {
        // ��ġ�� ���� ����
        GameObject monster = StageManager.instance.monsterGeneratePosition[targerPosition].GetChild(1).GetChild(targerPosition).gameObject;
        MonsterSettings targetInfo = monster.GetComponentInChildren<MonsterSettings>();
        if (targetInfo != null)
        {
            targetInfo.TakeDamage(PlayerStatManager.instance.playerPower);
        }
    }
    private void BossAttackTarget()
    {
        // ���� ���� ����
        GameObject bossMonster = StageManager.instance.monsterGeneratePosition[4].GetChild(1).GetChild(4).GetChild(0).gameObject;
        BossMonsterController targetInfo = bossMonster.GetComponent<BossMonsterController>();
        if (targetInfo != null)
        {
            targetInfo.TakeDamage(PlayerStatManager.instance.playerPower);
        }
    }
    public void BattleModeStart()
    {
        // ���� ��� ��� ����
        StartCoroutine("PerformAttack");
    }
    public void BattleModeEnd()
    {
        // ���� ��� ��� ����
        StopCoroutine("PerformAttack");
    }
    public void BossBattleModeStart()
    {
        // �������� ��� ��� ����
        StartCoroutine("BossPerformAttack");
    }
    public void BossBattleModeEnd()
    {
        // �������� ��� ��� ����
        StopCoroutine("BossPerformAttack");
    }
    public void MoveModeStart()
    {
        // �̵� ��� ����
        animator.SetTrigger("MoveState");
    }
    public void MoveModeEnd()
    {
        // �̵� ��� ����
        animator.SetTrigger("IdleState");
    }
    IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            if (targerPosition > 4)
            {
                // ��� ���͸� óġ ��

                StageManager.instance.bossMonsterAble = false;

                targerPosition = 0;
                yield return new WaitForSeconds(0.5f);
                StageManager.instance.SetStage();
                yield return new WaitForSeconds(2f);

            }
            // ���� �ִϸ��̼� ���
            animator.SetTrigger("AttackState");
            yield return new WaitForSeconds(0.01f);
            animator.SetTrigger("IdleState");

            // ���Ϳ��� ������ ����
            AttackTarget();


            yield return new WaitForSeconds(PlayerStatManager.instance.playerCoolDown); // ���� �ӵ��� ���� ���

        }

    }
    IEnumerator BossPerformAttack()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            // ���� �ִϸ��̼� ���
            animator.SetTrigger("AttackState");
            yield return new WaitForSeconds(0.01f);
            animator.SetTrigger("IdleState");

            // ���Ϳ��� ������ ����
            BossAttackTarget();


            yield return new WaitForSeconds(PlayerStatManager.instance.playerCoolDown); // ���� �ӵ��� ���� ���

        }

    }
}
