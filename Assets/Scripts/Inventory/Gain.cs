using UnityEngine;

[System.Serializable]
public class Gain 
{
    public int levelIncrease;

    public int amountChange;

    public Gain(int levelIncrease, int amountChange)
    {
        this.levelIncrease = levelIncrease; 
        this.amountChange = amountChange;
    }

   
}
