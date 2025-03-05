using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;

    private void OnTriggerEnter2D(Collider2D other)
    {
       // if (Input.GetKeyDown(KeyCode.F))
        //{
            if (other.CompareTag("Player")) // Проверяем, что столкновение с объектом игрока
            {
                GameObject triggerObject = gameObject;
                InventoryManager.AddItem(item, amount);
                Destroy(triggerObject);
            }
        //}
    }
}
