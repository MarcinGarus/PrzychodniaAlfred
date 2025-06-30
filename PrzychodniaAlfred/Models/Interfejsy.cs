namespace PrzychodniaAlfred.Models
{
    public interface Raport
    {
        string Nazwa { get; }
        int Wartosc { get; }
    }

    public interface InterfejsRaportu
    {
        string GenerujRaport();
    }
}
