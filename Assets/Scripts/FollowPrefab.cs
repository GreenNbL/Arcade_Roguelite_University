using UnityEngine;

public class FollowPrefab : MonoBehaviour
{
    public Transform prefab; // Ссылка на префаб
    public Vector3 offset; // Смещение от префаба

    private void Update()
    {
        if (prefab != null)
        {
            // Обновляем позицию пустого объекта с учетом смещения
            transform.position = prefab.position + offset;
        }
    }
}
