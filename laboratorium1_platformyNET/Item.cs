using System;
namespace lab1;

public class Item
{
    public int Val {get;set;}
    public int Weigh {get;set;}
    public int Indx {get;set;}
    public double Ratio => (double)Val / Weigh;

    public Item(int index,int wart,int waga){
        Indx = index;
        Val = wart;
        Weigh = waga;
    }

    public override string ToString(){
        return $"Item {Indx}: value={Val}, weight={Weigh}, ratio={Ratio:F2}";
    }
}