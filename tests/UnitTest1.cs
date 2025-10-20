using System.Runtime;
using flashcardo;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Program = flashcardo.Program;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRead1()
    {
        List<Card> cards = new List<Card>();

        Card? card = Program.readCardsFromFile("/home/yuki/Programaria/flashcard-yk/tests/teste.txt").FirstOrDefault();

        if (card == null)
        {
            Assert.Fail();
        }

        Assert.That(card.ToString(), Is.EqualTo("teste:testo:True"));
        //Assert.Equals(card.ToString(), "teste:testo");
    }

    [Test]
    public void TestRead2()
    {
        List<Card> cards = new List<Card>();

        string[] resultados = { "teste:testo:True", "testa:testi:True" };

        cards = Program.readCardsFromFile("/home/yuki/Programaria/flashcard-yk/tests/teste2.txt");

        if (cards == null)
        {
            Assert.Fail();
        }

        string[] testes = cards.Select(a => a.ToString()).ToArray();

        Assert.That(testes, Is.EquivalentTo(resultados));
        //Assert.Equals(card.ToString(), "teste:testo");
    }

    [Test]
    public void TestWrite()
    {
        List<Card> cards = new List<Card>();

        cards.Add(new Card(textFront: "oi", textBack: "tchau"));
        cards.Add(new Card(textFront: "boi", textBack: "bhau"));

        string[] resultados = { "oi:tchau:True", "boi:bhau:True" };

        Program.saveToFile(cards, "/home/yuki/Programaria/flashcard-yk/tests/teste3.txt");

        cards = Program.readCardsFromFile("/home/yuki/Programaria/flashcard-yk/tests/teste3.txt");

        if (cards == null)
        {
            Assert.Fail();
        }

        string[] testes = cards.Select(a => a.ToString()).ToArray();

        Assert.That(testes, Is.EquivalentTo(resultados));
    }
    
    [Test]
    public void TestReadPrint()
    {
        List<Card> cards = new List<Card>();

        string[] resultados = { "teste:testo:True", "testa:testi:True" };

        cards = Program.readCardsFromFile("/home/yuki/Programaria/flashcard-yk/tests/teste2.txt");

        if (cards == null)
        {
            Assert.Fail();
        }

        string[] testes = cards.Select(a => a.ToString()).ToArray();

        Assert.That(testes, Is.EquivalentTo(resultados));
        //Assert.Equals(card.ToString(), "teste:testo");
    }
}
