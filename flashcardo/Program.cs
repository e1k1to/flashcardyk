namespace flashcardo;

public class Program
{

    const string flashCardFilePath = "example.txt"; //what about flsc

    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }



    // Read from TXT
    // Words separates by TABS
    public static List<Card> readCardsFromFile(string filePath)
    {
        List<Card> cardList = new List<Card>();
        string[] linhas = [];

        try
        {
            linhas = File.ReadAllLines(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to open file: ");
            Console.WriteLine(ex.Message);
        }
        
        foreach(string linha in linhas)
        {
            string[] words = linha.Split("\t");

            if(words.Length < 2)
            {
                throw new Exception("Error on loading cards, check your file for errors.");
            }

            string textFront = words[0];
            string textBack = words[1];

            cardList.Add(
                new Card(textFront:textFront, textBack:textBack)
            );
        }

        return cardList;
    }

    // Save to TXT
    //
    public static bool saveToTXT() 
    {
        return false; 
    }

}