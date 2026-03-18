using lab1;
Console.WriteLine("Liczba przzedmiotow:");
int n = int.Parse(Console.ReadLine());

Console.WriteLine("Ziarno:");
int seed = int.Parse(Console.ReadLine());
Problem problem = new Problem(n, seed);

Console.WriteLine(problem);
Console.WriteLine("Pojemnosc plecaka:");
int pojemnosc = int.Parse(Console.ReadLine());

Result wynik = problem.Solve(pojemnosc);
Console.WriteLine(wynik);