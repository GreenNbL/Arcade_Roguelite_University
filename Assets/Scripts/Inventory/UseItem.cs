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
    private float doubleClickTime = 0.3f; // Время для определения двойного клика
    private float lastClickTime = 0f; // Время последнего клика
    private float fKeyCooldown = 0.1f; // Время ожидания между нажатиями F
    private float lastFKeyTime = 0f; // Время последнего нажатия F
    public GameObject outline;
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
            // Проверка нажатия клавиши F с учетом задержки
            if (isMouseOver && Input.GetKeyDown(KeyCode.F) && slot.item.clickable && slot.item.typeItem == ItemType.Armor)
            {
                float currentTime = Time.time;
                if (currentTime - lastFKeyTime >= fKeyCooldown)
                {
                    Debug.Log("Переключение рамки");
                    ToggleOutline(); // Вызов метода для переключения состояния рамки
                    lastFKeyTime = currentTime; // Сохраняем время последнего нажатия
                }
            }
        }
    }
    // Метод для переключения состояния рамки
    private void ToggleOutline()
    {
        if (slot.outline.activeSelf == true)
        {
            InventoryManager.setInactiveOutline(slot.item.typeItem);
        }
        else
        {
            InventoryManager.setInactiveOutline(slot.item.typeItem);
            outline.SetActive(!outline.activeSelf); // Переключение состояния рамки
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
