using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardPile
{
    private static System.Random rng = new System.Random();

    [SerializeField] private List<Card> contents = new List<Card>();

    public void Init()
    {
        List<Card> newList = new List<Card>();
        foreach (Card c in contents)
        {
            Card clone = GameObject.Instantiate(c);
            newList.Add(clone);
        }
        contents = newList;
    }

    public void Shuffle()
    {
        int n = contents.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = contents[k];
            contents[k] = contents[n];
            contents[n] = value;
        }
    }

    public Card Draw()
    {
        if (contents.Count <= 0)
            return null;
        Card value = contents[rng.Next(contents.Count)];
        contents.Remove(value);
        return value;
    }

    public void Add(Card c)
    {
        contents.Add(c);
    }

    public void AddFromPile(CardPile p)
    {
        Card[] toRemove = new Card[p.contents.Count];
        for (int i = 0; i < p.contents.Count; i++)
        {
            Card c = p.contents[i];
            toRemove[i] = c;
            Add(c);
        }

        foreach (Card c in toRemove)
        {
            p.Remove(c);
        }
    }

    public Card GetAt(int i)
    {
        return contents[i];
    }

    public bool Remove(Card c)
    {
        if (contents.Contains(c))
        {
            contents.Remove(c);
            return true;
        }
        return false;
    }

    public int NumCards
    {
        get { return contents.Count; }
    }
}
