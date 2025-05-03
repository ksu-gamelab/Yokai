// PlayerStatus.cs
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    private int hp = 2;

    public int HP => hp;

    public bool CanTransform() => hp > 0;

    public void ConsumeHP(int amount)
    {
        hp = Mathf.Max(0, hp - amount);
    }
}
