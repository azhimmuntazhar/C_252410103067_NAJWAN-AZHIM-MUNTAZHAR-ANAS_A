using System;


public class Sekolah
{
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;
    public void TampilInfo()
    {
        Console.WriteLine($"Sekolah dengan ID {idSekolah} bernama {namaSekolah} menerima {jumlahPaketDiterima} jumlah paket");
    }
}


abstract class PaketMBG
{
    private string idPaket { get; set; }

    private string namaPaket { get; set; }
    private int stokDapur { get; set; }

    //public string idpaket;
    //public string namapaket;
    //public int stokdapur;

    public PaketMBG(string idPaket, string namaPaket, int stokDapur)
    {
        this.idPaket = idPaket;
        this.namaPaket = namaPaket;
        this.stokDapur = stokDapur;
    }


    public void KurangiStok(int jumlah)
    {

        if (jumlah > 0 && jumlah <= stokDapur)
        {
            stokDapur = stokDapur - jumlah;
        }
        else
        {
            Console.WriteLine($"Harus lebih dari 0 atau dibawah stok {stokDapur}");
        }
    }

    public abstract void HitungBiaya(int jumlah);

}


class PaketRegular : PaketMBG
{
    public PaketRegular(string id, string nama, int stok) : base("0", "none", 0)
    {
    }
    public void infoRegular()
    {
        Console.WriteLine("Paket ini berisi nasi, ayam, sayur, dan buah");
    
    }

    public override void HitungBiaya(int jumlah)
    {
        if (jumlah > 0)
        {
            jumlah = jumlah * 15000;
            Console.WriteLine($"Jumlahnya jadi {jumlah} mas");
        }
    }
}

class PaketKhusus : PaketMBG
{
    public string jenisDiet;

    public PaketKhusus(string id, string nama, int stok, string jenisDiet) : base("0", "none", 0)
    {
        this.jenisDiet = jenisDiet;
    }
    public override void HitungBiaya(int jumlah)
    {

        if (jumlah > 0)
        {
            jumlah = jumlah * 20000;
            Console.WriteLine($" Mbg Jumlahnya jadi {jumlah} mas");
        }

    }

}

public interface DapatDidistribusikan
{
    public void KirimPaket() {

    }
}

public class DistribusiMBG : DapatDidistribusikan
{
    List<DistribusiMBG> dataDistribusi;
    public DistribusiMBG(List<DistribusiMBG> data)
    {
        dataDistribusi = data;
    }
}

public class Program
{
    static void Main()
    {

        PaketRegular mbgRegular = new PaketRegular("1", "Paket Biasa", 30);
        mbgRegular.HitungBiaya(30);
        mbgRegular.infoRegular();
        mbgRegular.KurangiStok(10);

        PaketKhusus mbgKhusus = new PaketKhusus("2", "Paket Vegetarian", 50, "Vegetarian");
        mbgRegular.HitungBiaya(50);
    }
}