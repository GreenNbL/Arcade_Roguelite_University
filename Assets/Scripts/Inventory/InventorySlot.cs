using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text textAmount;

    void Awake()
    {
        this.iconGO = transform.GetChild(0).gameObject;
        this.textAmount =  transform.GetChild(1).GetComponent<TMP_Text>();
    }
    public void SetIcon( Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }
}
