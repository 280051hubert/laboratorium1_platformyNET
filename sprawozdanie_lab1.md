# Sprawozdanie – Laboratorium 1

Hubert Łoziński 280051 ISA
---

## 1. Cel laboratorium

Celem laboratorium było zaprojektowanie i zaimplementowanie rozwiązania problemu plecakowego w języku C# na platformie .NET. oraz stworzenie testów jednostkowych aby przetestować rozwiązanie.

---

## 2. Opis implementacji

### 2.1 Klasa `Item`

Klasa reprezentuje pojedynczy przedmiot plecakowy z trzema właściwościami: indeksem, wartością i wagą. Kluczową cechą jest właściwość obliczana `Ratio`, wyrażająca stosunek wartości do wagi.

```csharp
namespace lab1;

public class Item
{
    public int Val   { get; set; }
    public int Weigh { get; set; }
    public int Indx  { get; set; }
    public double Ratio => (double)Val / Weigh;

    public Item(int index, int wart, int waga)
    {
        Indx  = index;
        Val   = wart;
        Weigh = waga;
    }

    public override string ToString()
    {
        return $"Item {Indx}: value={Val}, weight={Weigh}, ratio={Ratio:F2}";
    }
}
```

### 2.2 Klasa `Problem` 

Klasa generuje `n` losowych przedmiotów na podstawie podanego ziarna (`seed`) i implementuje algorytm zachłanny.y.

Algorytm działa w następujący sposób:
1. Sortuje przedmioty malejąco według wskaźnika wartość/waga.
2. Iteruje po posortowanej liście i dodaje przedmiot do plecaka, jeśli jego waga mieści się w dostępnej pojemności.

```csharp
namespace lab1
{
    public class Problem
    {
        public int N { get; set; }
        public List<Item> Przedmioty { get; set; }

        public Problem(int n, int seed)
        {
            N = n;
            Przedmioty = new List<Item>();
            Random random = new Random(seed);

            for (int i = 0; i < n; i++)
            {
                int value  = random.Next(1, 11);
                int weight = random.Next(1, 11);
                Przedmioty.Add(new Item(i, value, weight));
            }
        }

        public Result Solve(int capacity)
        {
            Result wynik = new Result();

            var sorted = Przedmioty
                .OrderByDescending(i => i.Ratio)
                .ToList();

            int currentWeight = 0;

            foreach (var item in sorted)
            {
                if (currentWeight + item.Weigh <= capacity)
                {
                    wynik.Przedmioty.Add(item.Indx);
                    wynik.ValTot  += item.Val;
                    wynik.MasTol  += item.Weigh;
                    currentWeight += item.Weigh;
                }
            }
            return wynik;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Przedmioty:");
            foreach (var item in Przedmioty)
                sb.AppendLine(item.ToString());
            return sb.ToString();
        }
    }
}
```

### 2.3 Klasa `Result`

Klasa przechowuje wynik działania algorytmu.
```csharp
namespace lab1;

public class Result
{
    public List<int> Przedmioty { get; set; } = new List<int>();
    public int ValTot { get; set; }
    public int MasTol { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Przedmioty:");
        foreach (var i in Przedmioty)
            sb.AppendLine($"Przedmiot {i}");
        sb.AppendLine($"Wartosc calkowita: {ValTot}");
        sb.AppendLine($"Waga calkowita: {MasTol}");
        return sb.ToString();
    }
}
```

### 2.4 Main

Program pobiera od użytkownika liczbę przedmiotów, ziarno generatora losowego oraz pojemność plecaka, a następnie wyświetla listę przedmiotów i wynik działania algorytmu.

```csharp
using lab1;

Console.WriteLine("Liczba przedmiotow:");
int n = int.Parse(Console.ReadLine());

Console.WriteLine("Ziarno:");
int seed = int.Parse(Console.ReadLine());

Problem problem = new Problem(n, seed);
Console.WriteLine(problem);

Console.WriteLine("Pojemnosc plecaka:");
int pojemnosc = int.Parse(Console.ReadLine());

Result wynik = problem.Solve(pojemnosc);
Console.WriteLine(wynik);
```

## 3. Testy jednostkowe

Projekt testów korzysta z frameworka MSTest

```csharp
[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
    }
}
```
