// PlayerStatus.cs
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHP = 3;

    public bool CanTransform()
    {
        return currentHP > 0;
    }

    public void ConsumeHP(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        Debug.Log("HP消費: " + currentHP);
    }

    public void RecoverHP(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
    }
}