using System;
using System.Text;
namespace lab1;
 public class Result
{
    public List<int> Przedmioty {get;set;} = new List<int>();
    public int ValTot {get;set;}
    public int MasTol {get;set;}

    public override string ToString() {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Przedmioty:");
        foreach (var i in Przedmioty) {
            sb.AppendLine($"Przedmiot {i}");
        }
        sb.AppendLine($"Wartosc calkowita: {ValTot}");
        sb.AppendLine($"Waga calkowita: {MasTol}");
        return sb.ToString();
    }
}

