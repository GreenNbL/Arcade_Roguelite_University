using UnityEngine;

public class FollowPrefab : MonoBehaviour
{
    public Transform prefab; // ������ �� ������
    public Vector3 offset; // �������� �� �������

    private void Update()
    {
        if (prefab != null)
        {
            // ��������� ������� ������� ������� � ������ ��������
            transform.position = prefab.position + offset;
        }
    }
}
