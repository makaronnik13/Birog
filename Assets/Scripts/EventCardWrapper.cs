public class EventCardWrapper
{
    public string Guid;
    public int CardId;

    public EventCard Card
    {
        get
        {
            return (EventCard)DefaultResources.GetCardById(CardId, DefaultResources.CardType.Event);
        }
    }

    public EventCardWrapper(EventCard card, string guid = "")
    {
        if (guid == "")
        {
            guid = System.Guid.NewGuid().ToString();
        }
        CardId = card.Id;
        Guid = guid;
    }
}