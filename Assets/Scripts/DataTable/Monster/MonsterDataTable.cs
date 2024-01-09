using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MonsterDataTable")]
public class MonsterDataTable : ScriptableObject
{
    public string monsterName; // ���� �̸�
    public float monsterHP; // ���� ü��
    public float dropResorceAmount; // ����Ǵ� �ڿ��� ��
}
