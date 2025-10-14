namespace flashcardo;

public class Program
{

    const string flashCardFilePath = "example.txt"; //what about flsc
    public List<Card> cardList = new List<Card>();

    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Hello, World!");
        Console.ResetColor();
    }

    // Read from TXT
    // Words separates by ;
    public static List<Card> readCardsFromFile(string filePath)
    {
        List<Card> cardList = new List<Card>();
        string[] linhas;

        try
        {
            linhas = File.ReadAllLines(filePath);
        }
        catch (Exception ex)
        {
            linhas = [];
            Console.WriteLine("Unable to open file: ");
            Console.WriteLine(ex.Message);
        }
        
        foreach(string linha in linhas)
        {
            string[] words = linha.Split(';');

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
    public static bool saveToFile(List<Card> cardList, string filePath)
    {
        if(cardList == null)
        {
            Console.WriteLine("Couldn't write file: ");
            Console.WriteLine("No cards found.");
            return false;
        }
        List<string> allLines = new List<string>();
        foreach (Card card in cardList)
        {
            allLines.Add(String.Format("{0};{1}", card.TextFront, card.TextBack));
        }

        using (StreamWriter outputFile = new StreamWriter(filePath))
        {
            try
            {
                foreach(var line in allLines)
                {
                    outputFile.WriteLine(line);
                }
            } catch (Exception ex)
            {
                Console.WriteLine("Couldn't write file: ");
                Console.WriteLine(ex);
                return false;
            }
        }

        return true; 
    }
}