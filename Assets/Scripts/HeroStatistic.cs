using TMPro;
using UnityEngine;

public class HeroStotistic : MonoBehaviour
{
    public float health = 100;

    public int armor = 1;

    public int score = 0;

    public TMP_Text healthPoint;

    public TMP_Text scrorePoint;

    private void Start()
    {
        healthPoint.text= health.ToString();
        scrorePoint.text = score.ToString();
    }

}
