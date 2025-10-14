namespace flashcardo;

public class Card
{
    public bool IsFlipped = false;
    public string TextFront { get; set; }
    public string TextBack { get; set; }
    
    public Card(string textFront, string textBack) 
    {
        TextFront = textFront;
        TextBack = textBack;
    }

    public void FlipCard()
    {
        IsFlipped = !IsFlipped;
    }

    public override string ToString()
    {
        return $"{TextFront}:{TextBack}";
    }

}
