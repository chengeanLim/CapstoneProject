using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StageDataTable")]
public class StageDataTable : ScriptableObject
{
    public int mainStageNumber; // ���� �������� ��ȣ
    public int subStageNumber; // ���� �������� ��ȣ
    public GameObject[] monsterPrefab = new GameObject[5]; // ���� ��ġ�� ���� ������
    public GameObject bossMonsterPrefab; //  ���� ���� ������
}
