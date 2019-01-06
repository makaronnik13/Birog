public class EncounterCardWrapper
{
    public string Guid;
    public int CardId;

    public EncounterCard Card
    {
        get
        {
            return (EncounterCard)DefaultResources.GetCardById(CardId, DefaultResources.CardType.Encounter);
        }
    }

    public EncounterCardWrapper(EncounterCard card, string guid = "")
    {
        if (guid == "")
        {
            guid = System.Guid.NewGuid().ToString();
        }

        CardId = card.Id;
        Guid = guid;
    }
}