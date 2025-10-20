using System.Data.Common;

namespace flashcardo;

public class Card
{
    public static int lastId = 0;
    public int Id { get; }
    public bool IsActive = true;
    public string TextFront { get; set; }
    public string TextBack { get; set; }

    public Card(string textFront, string textBack)
    {
        TextFront = textFront;
        TextBack = textBack;
        Id = ++lastId;
    }

    ~Card()
    {
        lastId--;
    }
    
    public static void resetIds()
    {
        lastId = 0;
    }

    public void SetCardCard()
    {
        IsActive = !IsActive;
    }

    public override string ToString()
    {
        return $"{TextFront}:{TextBack}:{IsActive}";
    }
}
