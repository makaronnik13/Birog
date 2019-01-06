public class BattleCardWrapper
{
    public string Guid;
    public int CardId;

    public BattleCard Card
    {
        get
        {
            return (BattleCard)DefaultResources.GetCardById(CardId, DefaultResources.CardType.Battle);
        }
    }

    public BattleCardWrapper(BattleCard card, string guid = "")
    {
        Guid = guid;
        if (Guid == "")
        {
            Guid = System.Guid.NewGuid().ToString();
        }
        CardId = card.Id;
    }
}