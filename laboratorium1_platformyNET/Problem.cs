using System;

using System.Text;

namespace lab1
{
    public class Problem{
        public int N {get;set;} 
        public List<Item> Przedmioty {get;set;}
        public Problem(int n,int seed){
            N = n;
            Przedmioty = new List<Item>();
            Random random = new Random(seed);

            for (int i=0;i<n;i++){
                int value = random.Next(1, 11);
                int weight = random.Next(1, 11);

                Przedmioty.Add(new Item(i, value, weight));
            }
        }
        public Result Solve(int capacity) {
            Result wynik = new Result();

            var sorted = Przedmioty
                .OrderByDescending(i => i.Ratio)
                .ToList();

            int currentWeight = 0;

            foreach (var item in sorted) {
                if (currentWeight + item.Weigh <= capacity) {
                    wynik.Przedmioty.Add(item.Indx);
                    wynik.ValTot += item.Val;
                    wynik.MasTol += item.Weigh;

                    currentWeight += item.Weigh;
                }}
            return wynik;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Przedmioty:");
            foreach (var item in Przedmioty) {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}