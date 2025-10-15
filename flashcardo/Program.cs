using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Formats.Asn1;
using System.Numerics;
using System.Runtime.InteropServices;

namespace flashcardo;

public class Program
{
    const string flashCardFilePath = "/home/yuki/Programaria/flashcard-yk/flashcardo/example.txt"; //what about flsc
    public static List<Card> fullCardList = new List<Card>();

    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        readCardsFromFile(filePath: flashCardFilePath);

        bool exitSelected = false;
        int menuChoice = -1;
        while(!exitSelected)
        {
            menuChoice = mainMenu();
            switch (menuChoice)
            {
                case 1:
                    play(fullCardList);
                    break;
                case 2:
                    showAllCards();
                    break;
                case 3:
                    addCard();
                    break;
                case 4:
                    deleteCard();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }
    }


    public static void play(List<Card> playlist)
    {
        int totalCards = playlist.Count();
        int correctAnswers = 0;
        int sleepTime = 1000;
        List<Card> playCards = playlist;
        Shuffle(playCards);

        foreach (var card in playCards)
        {
            string cardfront = card.TextFront;
            string cardback = card.TextBack;
            string answer = "";

            Console.Clear();
            Console.WriteLine("Word is: ");
            Console.WriteLine(cardfront);
            (int left, int top) = Console.GetCursorPosition();
            Console.CursorVisible = true;
            while (answer == null || answer == "")
            {
                Console.SetCursorPosition(left, top);
                Console.WriteLine("Type your answer: ");
                answer = Console.ReadLine();
            }
            Console.CursorVisible = false;
            Console.Clear();
            if (answer.ToLower() == cardback.ToLower())
            {
                Console.WriteLine("Congratulations, you got it right!");
                correctAnswers++;
                Thread.Sleep(sleepTime);
            }
            else
            {
                Console.WriteLine("Unfortunately, you got it wrong!");
                Console.WriteLine($"Correct answer: {cardback}");
                Thread.Sleep(sleepTime);
            }

        }
        Console.Clear();
        Console.WriteLine("><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><");
        Console.WriteLine();
        Console.WriteLine($"\t \tYou got {correctAnswers} out of {totalCards} right! Good job!");
        Console.WriteLine();
        Console.WriteLine("><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><");

        Thread.Sleep(sleepTime*3);
    }

    public static void Shuffle(List<Card> list)
    {
        Random rand = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static int mainMenu()
    {
        ConsoleKeyInfo key;
        int choice = 1;
        bool enterPressed = false;
        while (!enterPressed)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Flashcardo: Your terminal flashcard manager!");
            Console.ResetColor();
            (int left, int top) = Console.GetCursorPosition();

            Console.WriteLine($"{(choice == 1 ? "> " : "  ")}Play!");
            Console.WriteLine($"{(choice == 2 ? "> " : "  ")}Show all flashcards.");
            Console.WriteLine($"{(choice == 3 ? "> " : "  ")}Create new flashcard");
            Console.WriteLine($"{(choice == 4 ? "> " : "  ")}Delete existing flashcard");
            Console.WriteLine($"{(choice == 5 ? "> " : "  ")}Exit");
            

            key = Console.ReadKey(false);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    choice = choice == 1 ? 5 : choice - 1;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    choice = choice == 5 ? 1 : choice + 1;
                    break;
                case ConsoleKey.Enter:
                    enterPressed = true;
                    break;
            }
        }

        return choice;
    }
    public static void showAllCards()
    {
        ConsoleKeyInfo key;
        bool enterPressed = false;
        int page = 1;
        int itemsPerPage = 10;
        int numberOfItems = fullCardList.Count();
        int maxPag = Convert.ToInt32(Math.Ceiling((double)numberOfItems / itemsPerPage)); 
        while (!enterPressed)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Full list of cards: ");

            foreach (var card in fullCardList.Skip((page - 1) * itemsPerPage).Take(itemsPerPage))
            {
                Console.WriteLine($"Id: {card.Id} - Front: {card.TextFront} - Back: {card.TextBack}");
            }
            Console.WriteLine();
            Console.WriteLine("Press enter to continue.");
            Console.WriteLine("Press <- and -> to navigate through pages.");

            key = Console.ReadKey(false);
            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                case ConsoleKey.L:
                    page = (page == maxPag) ? 1 : page + 1;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.H:
                    page = (page == 1) ? maxPag : page - 1;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Q:
                    enterPressed = true;
                    break;
            }
        }

    }
    public static bool addCard()
    {
        string fronttxt = "";
        Console.CursorVisible = true;
        string backtxt = "";
        while (fronttxt == "" || fronttxt == null)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Write the text for the front side of the card: (write \\CANCEL to cancel)");
            fronttxt = Console.ReadLine();
            if (fronttxt == "\\CANCEL")
            {
                Console.CursorVisible = false;
                return false;
            }
            else if (fronttxt.Contains(";"))
            {
                Console.WriteLine("Card cannot contain the character ';'");
                Console.WriteLine("Returning in a second...");
                Thread.Sleep(2000);
                fronttxt = "";
            }
        }
        while (backtxt == "" || backtxt == null)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Write the text for the back side of the card: (write \\CANCEL to cancel)");
            backtxt = Console.ReadLine();
            if (backtxt == "\\CANCEL")
            {
                Console.CursorVisible = false;
                return false;
            }
            else if (backtxt.Contains(";"))
            {
                Console.WriteLine("Card cannot contain the character ';'");
                Console.WriteLine("Returning in a second...");
                Thread.Sleep(2000);
                backtxt = "";
            }
        }

        fullCardList.Add(new Card(textFront: fronttxt, textBack: backtxt));

        saveToFile(cardList: fullCardList, filePath: flashCardFilePath);
        Console.CursorVisible = false;
        return true;
    }
    public static bool deleteCard()
    {
        int numberOfItems = fullCardList.Count();
        int page = 1;
        int itemsPerPage = 10;
        int itemsOnCurrentPage = 0;
        int maxPag = Convert.ToInt32(Math.Ceiling((double)numberOfItems / itemsPerPage));
        int idMarkedForDeletion = 0;
        Card markedForDeletion = null;

        ConsoleKeyInfo key;
        int choice = -1;
        bool enterPressed = false;
        while (!enterPressed)
        {
            itemsOnCurrentPage = 0;
            Console.Clear();
            Console.WriteLine("Flashcardo: Your terminal flashcard manager!");
            (int left, int top) = Console.GetCursorPosition();

            Console.WriteLine($"{(choice == -1 ? "> " : "  ")} Return to menu.");
            Console.WriteLine($"{(choice == 0 ? "> " : "  ")} Search by name.");
            foreach (var Card in fullCardList.Skip((page - 1) * itemsPerPage).Take(itemsPerPage))
            {
                Console.WriteLine($"{(Card.Id == (choice + ((page - 1) * itemsPerPage)) ? "> " : "  ")} Card {Card.Id}: {Card.TextFront} <> {Card.TextBack}");
                itemsOnCurrentPage++;
            }

            key = Console.ReadKey(false);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.K:
                    choice = (choice == -1) ? itemsOnCurrentPage : choice - 1;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.J:
                    choice = (choice == itemsOnCurrentPage) ? -1 : choice + 1;
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.L:
                    choice = -1;
                    page = (page == maxPag) ? 1 : page + 1;
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.H:
                    choice = -1;
                    page = (page == 1) ? maxPag : page - 1;
                    break;
                case ConsoleKey.Enter:
                    enterPressed = true;
                    break;
            }
        }
        
        if(choice == -1)
        {
            return false;
        }
        if (choice == 0)
        {
            Console.Clear();
            Console.WriteLine("Write the text (front or back) of the card you want to delete: (leave blank to return)");
            Console.CursorVisible = true;
            string nameForDeletion = Console.ReadLine();
            Console.CursorVisible = false;

            if (nameForDeletion == null || nameForDeletion == "")
            {
                Console.WriteLine("Returning to main menu.");
                Thread.Sleep(1000);
                return false;
            }

            List<Card> deleteQuery = fullCardList.Where(a => a.TextBack.Contains(nameForDeletion) || a.TextFront.Contains(nameForDeletion)).ToList();

            if (deleteQuery == null)
            {
                Console.WriteLine("Couldn't find card, returning to main menu.");
                Thread.Sleep(1000);
                return false;
            }

            while (idMarkedForDeletion == 0)
            {
                Console.WriteLine("Type the id of chosen card: (type -1 to return to main menu)");
                Console.WriteLine("Candidates for deletion: ");
                foreach (var card in deleteQuery)
                {
                    Console.WriteLine($"ID: {card.Id}, Front: {card.TextFront}, Back: {card.TextBack}");
                }

                try
                {
                    Console.CursorVisible = true;
                    int.TryParse(Console.ReadLine(), out idMarkedForDeletion);
                    Console.CursorVisible = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't delete card, returning to main menu.");
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(1000);
                    return false;
                }

                if (idMarkedForDeletion == -1)
                {
                    return false;
                }

                markedForDeletion = deleteQuery.FirstOrDefault(a => a.Id == idMarkedForDeletion);

                if (markedForDeletion == null)
                {
                    Console.WriteLine("Couldn't delete card.");
                    Thread.Sleep(500);
                    idMarkedForDeletion = 0;
                }
            }
        }

        else
        {
            idMarkedForDeletion = choice + ((page - 1) * itemsPerPage);
            markedForDeletion = fullCardList.FirstOrDefault(a => a.Id == idMarkedForDeletion);

            if(markedForDeletion == null )
            {
                Console.WriteLine("Couldn't delete card... Returning to main menu.");
                Thread.Sleep(500);
            }
        }
        
        string confirm = "";

        while (confirm.ToLower() == "y")
        {
            Console.WriteLine("Are you sure you want to delete this card?: ");
            Console.WriteLine($"ID: {markedForDeletion.Id}, Front: {markedForDeletion.TextFront}, Back: {markedForDeletion.TextBack}");
            Console.CursorVisible = true;
            confirm = Console.ReadLine();
            Console.CursorVisible = false;
            if (confirm.ToLower() == "n")
            {
                Console.WriteLine("Returning to main menu.");
                Thread.Sleep(500);
                return false;
            }
        }

        fullCardList.Remove(markedForDeletion);

        Card.resetIds();
        saveToFile(fullCardList, flashCardFilePath);
        readCardsFromFile(flashCardFilePath);

        Console.WriteLine("Success on deleting card.");
        Thread.Sleep(500);

        return true;
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

        foreach (string linha in linhas)
        {
            string[] words = linha.Split(';');

            if (words.Length < 2)
            {
                throw new Exception("Error on loading cards, check your file for errors.");
            }

            string textFront = words[0];
            string textBack = words[1];

            cardList.Add(
                new Card(textFront: textFront, textBack: textBack)
            );
        }
        fullCardList = cardList;
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
