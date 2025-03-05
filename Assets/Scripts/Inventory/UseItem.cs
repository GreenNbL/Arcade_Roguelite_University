using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOver = false;
    private Coroutine applyCoroutine = null;
    public InventorySlot slot;

    private void Start()
    {
       
        // Находим скрипт InventorySlot в слоте в иерархии
        slot = transform.GetComponentInParent<InventorySlot>();
    }

    void Update()
    {
        if (!slot.isEmpty)
        {
            // Проверяем, если мышка над иконкой и клавиша E зажата
            if (isMouseOver && Input.GetKey(KeyCode.E) && slot.item.clickable == true)
            {
                if (applyCoroutine == null)
                {
                    // Запускаем корутину для применения
                    applyCoroutine = StartCoroutine(ApplyItemWithDelay(1f)); // Задержка в 1 секунды
                }
            }
            else
            {
                // Если кнопка E отпущена или мышь покинула объект, останавливаем корутину
                if (applyCoroutine != null)
                {
                    StopCoroutine(applyCoroutine);
                    applyCoroutine = null;
                }
            }
        }
    }

    // Метод для регистрации наведения мыши на иконку
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    // Метод для регистрации покидания мыши иконки
    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    // Корутина, обрабатывающая применение предмета с задержкой
    private IEnumerator ApplyItemWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Здесь выполните действия по применению предмета
        Debug.Log("Предмет применен!");
        if (slot.item.typeItem == ItemType.Food)
        {
            slot.item.healPlayer();
            slot.amount -= 1;
            slot.textAmount.text= slot.amount.ToString();
            if (slot.amount==0)
            {
                NullifySlotData();
            }
        }
        // Сбросим корутину после выполнения
        applyCoroutine = null;
    }
    void NullifySlotData()
    {
        // убираем значения InventorySlot
        slot.item = null;
        slot.amount = 0;
        slot.isEmpty = true;
        slot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        slot.iconGO.GetComponent<Image>().sprite = null;
        slot.textAmount.text = "";
    }
}
