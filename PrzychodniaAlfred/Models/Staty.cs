namespace PrzychodniaAlfred.Models
{
    public interface IStaty
    {
        string Kto { get; }
        int Ile { get; }
    }

    public interface IZapisywalny
    {
        string DoPliku();
    }
}
