using UnityEngine;

[CreateAssetMenu(fileName = "Battler", menuName = "Battler", order = 2)]
public class BattlerClass : ScriptableObject
{
    public string BattlerName;
    public Sprite BattlerImage;
    public int Hp;
    public int Armor;
    public int Initiative;
    public string Description;
    public BattleCard[] Deck;
}