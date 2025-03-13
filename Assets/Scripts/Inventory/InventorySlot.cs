using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text textAmount;
    public GameObject outline;
    public int id;
    void Awake()
    {
        this.iconGO = transform.GetChild(0).GetChild(1).gameObject;
        this.textAmount =  transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }
    public void SetIcon( Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }


}
