using UnityEngine;

[CreateAssetMenu(fileName = "Battler", menuName = "Battler", order = 2)]
public class BattlerClass : ScriptableObject
{
    public string BattlerName;
    public Sprite BattlerImage;
    public int Hp;
    public string Description;
    public BattleCard[] Hand;
    public BattleCard[] Deck;
    public Color BattlerColor;
}