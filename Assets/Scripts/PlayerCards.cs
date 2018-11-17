using System.Collections.Generic;

public class PlayerCards
{
    public Queue<BattleCard> Deck = new Queue<BattleCard>();
    public List<BattleCard> Hand = new List<BattleCard>();
    public List<BattleCard> Drop = new List<BattleCard>();
}