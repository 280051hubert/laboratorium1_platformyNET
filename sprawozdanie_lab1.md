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

Testy konstruktora klasy Problem
1. Poprawna liczba wygenerowanych przedmiotów (Constructor_CorrectItemNumber)
Sprawdza, czy konstruktor klasy Problem tworzy dokładnie tyle przedmiotów, ile zostało podane jako argument n. Test wykonywany jest dla kilku różnych wartości: 5, 10, 20 i 50.
2. Wartości w dozwolonym zakresie (ValuesInRange)
Weryfikuje, czy każdy wygenerowany przedmiot ma wartość (Val) oraz wagę (Weigh) mieszczące się w przedziale od 1 do 10 włącznie, zgodnie z wymaganiami zadania.
3. Deterministyczność generatora – to samo ziarno daje ten sam wynik (SameSeed_SameInstant)
Sprawdza, czy dwa obiekty Problem utworzone z identyczną liczbą przedmiotów i tym samym ziarnem (seed) generują dokładnie taką samą listę przedmiotów pod względem wartości i wag.
4. Metoda ToString() zwraca niepusty ciąg znaków (ToString_ReturnsNonEmpty)
Upewnia się, że wywołanie ToString() na obiekcie Problem nie zwraca wartości null ani pustego ciągu znaków.

Testy metody Solve
5. Dla wystarczającej pojemności wynik zawiera co najmniej jeden przedmiot (OneItem_oneresult_lista)
Dla pojemności plecaka równej 10 (każdy przedmiot waży maksymalnie 10) sprawdza, czy algorytm wybiera przynajmniej jeden przedmiot.
6. Dla pojemności zerowej żaden przedmiot nie wchodzi do plecaka (Zerocap_zeroresult_lista)
Weryfikuje, że przy pojemności równej 0 wynikowa lista przedmiotów jest pusta, a całkowita wartość i waga wynoszą 0.
7. Łączna waga wybranych przedmiotów nie przekracza pojemności plecaka (TotWeight_Smoller_Than_Capacity)
Sprawdza, czy suma wag przedmiotów w wynikowym plecaku jest mniejsza lub równa zadanej pojemności (30). To podstawowy warunek poprawności algorytmu.
8. Znana instancja daje spójny wynik (KnowInst_KnowResult_lista)
Dla konkretnej instancji problemu (10 przedmiotów, ziarno 1, pojemność 50) weryfikuje ogólne warunki poprawności: wynik nie jest pusty, waga mieści się w pojemności, a wartość całkowita jest dodatnia.
9. Suma wartości wybranych przedmiotów zgadza się z polem ValTot (Solve_TotalValueMatchesSumOfSelectedItems)
Sprawdza spójność wewnętrzną wyniku: oblicza sumę wartości przedmiotów o indeksach zwróconych przez algorytm i porównuje ją z polem ValTot w obiekcie Result.
10. Suma wag wybranych przedmiotów zgadza się z polem MasTol (Solve_TotalWeightMatchesSumOfSelectedItems)
Analogicznie do poprzedniego testu – weryfikuje, czy pole MasTol w wynikowym obiekcie odpowiada rzeczywistej sumie wag wybranych przedmiotów.
