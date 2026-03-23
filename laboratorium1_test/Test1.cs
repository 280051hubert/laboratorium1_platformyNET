using lab1;

namespace laboratorium1_test
{
    [TestClass]
    public class UnitTest1
    {
        //ile konstruktor robi przedmiotow
        [TestMethod]
        public void Constructor_CorrectItemNumber()
        {
            List<int> sizes = new List<int>() { 5, 10, 20, 50 };
            foreach (int n in sizes)
            {
                Problem problem = new Problem(n, seed: 42);
                Assert.AreEqual(n, problem.Przedmioty.Count, $"Zly count przedmiotow");
            }
        }

        //czy wartosc sie mieszczą
        [TestMethod]
        public void ValuesInRange()
        {
            Problem problem = new Problem(100, seed: 7);
            foreach (Item item in problem.Przedmioty)
            {
                Assert.IsTrue(item.Val >= 1 && item.Val <= 10,
                    $"Wartość spoza zakresu - ERROR");
                Assert.IsTrue(item.Weigh >= 1 && item.Weigh <= 10,
                    $"Waga spoza zakresu - ERROR");
            }
        }

        ///ten sam seed daje ta sama instancje
        [TestMethod]
        public void SameSeed_SameInstant()
        {
            Problem p1 = new Problem(20, seed: 123);
            Problem p2 = new Problem(20, seed: 123);

            for (int i = 0; i < p1.Przedmioty.Count; i++)
            {
                Assert.AreEqual(p1.Przedmioty[i].Val, p2.Przedmioty[i].Val);
                Assert.AreEqual(p1.Przedmioty[i].Weigh, p2.Przedmioty[i].Weigh);
            }
        }

        //Czy ToString zwraca niepusty string
        [TestMethod]
        public void ToString_ReturnsNonEmpty()
        {
            Problem problem = new Problem(5, seed: 1);
            string result = problem.ToString();
            Assert.IsFalse(string.IsNullOrEmpty(result),"TO STRING() ZWRÓCIŁ PUSTY");
        }

        //Test Solve. co najmniej jeden przedmiot spełnia ograniczenie pojemności, wynik powinien zawierać co najmniej jeden element.
        [TestMethod]
        public void OneItem_oneresult_lista()
        {
            Problem problem = new Problem(10, seed: 1);
            // capacity=10 — każdy przedmiot ma wagę max 10, więc co najmniej jeden wejdzie
            Result result = problem.Solve(capacity: 10);
            Assert.IsTrue(result.Przedmioty.Count >= 1,"Wynik nie zwrocil 1<= przedmiotu");
        }

        ///jesli pojemnosc 0, to czy poprawnie nic nie wejdzie
        [TestMethod]
        public void Zerocap_zeroresult_lista()
        {
            Problem problem = new Problem(10, seed: 1);
            Result result = problem.Solve(capacity: 0);
            Assert.AreEqual(0, result.Przedmioty.Count, "Dla 0 lista przedmiot powinna == null");
            Assert.AreEqual(0, result.ValTot);
            Assert.AreEqual(0, result.MasTol);
        }

        ///czy syma wag nie przekracza poj plecaka
        [TestMethod]
        public void TotWeight_Smoller_Than_Capacity()
        {
            int capacity = 30;
            Problem problem = new Problem(15, seed: 42);
            Result result = problem.Solve(capacity);
            Assert.IsTrue(result.MasTol <= capacity, "Waga przekracza pojemnosc");
        }

        //sprawdza dla instancji (n=10, seed=1, capacity=50).
        [TestMethod]
        public void KnowInst_KnowResult_lista()
        {
            Problem problem = new Problem(10, seed: 1);
            Result result = problem.Solve(capacity: 50);

            Assert.IsTrue(result.Przedmioty.Count > 0, "WYNIK JEST PUSTY");
            Assert.IsTrue(result.MasTol <= 50,"WAGA > CAPACITY");
            Assert.IsTrue(result.ValTot > 0,"TOTAL VALUE < 0");
        }

        //CZY VALTOT JEST ROWNE WARTOSCI PRZEDMIOTOW
        [TestMethod]
        public void Solve_TotalValueMatchesSumOfSelectedItems()
        {
            Problem problem = new Problem(10, seed: 9);
            Result result = problem.Solve(capacity: 30);

            int expectedValue = result.Przedmioty
                .Sum(idx => problem.Przedmioty.First(p => p.Indx == idx).Val);

            Assert.AreEqual(expectedValue, result.ValTot, "ValTot powinien być równy sumie wartości wybranych przedmiotów");
        }

        //CZY MASTOL ZGADZA SIE Z WAGA
        [TestMethod]
        public void Solve_TotalWeightMatchesSumOfSelectedItems()
        {
            Problem problem = new Problem(10, seed: 9);
            Result result = problem.Solve(capacity: 30);

            int expectedWeight = result.Przedmioty
                .Sum(idx => problem.Przedmioty.First(p => p.Indx == idx).Weigh);

            Assert.AreEqual(expectedWeight, result.MasTol, "MasTol powinien być równy sumie wag wybranych przedmiotów");
        }
    }
}
