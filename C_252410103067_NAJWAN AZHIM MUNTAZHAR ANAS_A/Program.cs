using System;

//class sekolah
public class Sekolah
{
    //atribut sekolah
    public string idSekolah;
    public string namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string idSekolah, string namaSekolah, int jumlahPaketDiterima = 0)
    {
        this.idSekolah = idSekolah;
        this.namaSekolah = namaSekolah;
        this.jumlahPaketDiterima = jumlahPaketDiterima;
    }

    //method dari sekolah
    public void TampilInfo()
    {
        Console.WriteLine($"ID Sekolah            : {idSekolah}");
        Console.WriteLine($"Nama Sekolah          : {namaSekolah}");
        Console.WriteLine($"Jumlah Paket Diterima : {jumlahPaketDiterima}");
    }
}

public abstract class PaketMBG
{
    //enkapsulasi 
    private string idPaket;
    private string namaPaket;
    private int stokDapur;

    //menggunakan get set untuk mengakses atribut private
    public string IdPaket
    {
        get 
        { 
            return idPaket;
        }
        set {
            idPaket = value;
        }
    }

    public string NamaPaket
    {
        get {
            return namaPaket;
        }
        set {
            namaPaket = value;
        }
    }

    public int StokDapur
    {
        get {
            return stokDapur;
        }   
        private set
        {
            if (value < 0)
            {
                Console.WriteLine("Stok dapur tidak boleh negatif");
                return;
            }
            stokDapur = value;
        }
    }

    protected PaketMBG(string idPaket, string namaPaket, int stokDapur)
    {
        this.idPaket = idPaket;
        this.namaPaket = namaPaket;
        StokDapur = stokDapur;   
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > stokDapur)
        {
            Console.WriteLine("Jumlah harus lebih dari 0 dan tidak boleh melebihi stok dapur");
            return false;
        }

        StokDapur -= jumlah;
        return true;
    }

    public abstract int HitungBiaya(int jumlah);

    public virtual void TampilInfo()
    {
        Console.WriteLine($"ID Paket   : {idPaket}");
        Console.WriteLine($"Nama Paket : {namaPaket}");
        Console.WriteLine($"Stok Dapur : {stokDapur}");
    }
}

public class PaketReguler : PaketMBG
{
    private const int HargaPerPaket = 15000;

    public PaketReguler(string idPaket, string namaPaket, int stokDapur) : base(idPaket, namaPaket, stokDapur)   
    {
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * HargaPerPaket;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis      : Paket Reguler");
        Console.WriteLine($"Harga      : Rp{HargaPerPaket} per paket");
    }
}

public class PaketKhusus : PaketMBG
{
    private const int HargaPerPaket = 20000;

    public string jenisDiet;  

    public PaketKhusus(string idPaket, string namaPaket, int stokDapur, string jenisDiet) : base(idPaket, namaPaket, stokDapur)
    {
        this.jenisDiet = jenisDiet;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * HargaPerPaket;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine($"Jenis      : Paket Khusus {jenisDiet}");
        Console.WriteLine($"Harga      : Rp{HargaPerPaket} per paket");
    }
}

public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiMBG : DapatDidistribusikan
{
    private Sekolah sekolahTujuan;
    private PaketMBG paket;
    private int jumlah;

    public DistribusiMBG(Sekolah sekolahTujuan, PaketMBG paket, int jumlah)
    {
        this.sekolahTujuan = sekolahTujuan;
        this.paket = paket;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Jumlah paket yang dikirim harus lebih dari 0");
            return;
        }

        if (jumlah > paket.StokDapur)
        {
            Console.WriteLine($"Jumlah paket {jumlah} melebihi stok dapur yang ada");
            return;
        }

        paket.KurangiStok(jumlah);
        sekolahTujuan.jumlahPaketDiterima += jumlah;

        Console.WriteLine($"Distribusi MBG berhasil! Sebanyak {jumlah} paket makanan telah dikirim ke sekolah {sekolahTujuan.namaSekolah}");
    }
}

// hubungan antara PengelolaMBG dengan Sekolah dan PaketMBG adalah Aggregation
// PengelolaMBG "memiliki" daftar Sekolah dan PaketMBG relasi has-a
// tetapi objek Sekolah dan PaketMBG tetap bisa berdiri sendiri secara independen
// saat PengelolaMBG dihapus/dihancurkan, objek Sekolah dan PaketMBG masih bisa digunakan di bagian lain program
// Sekolah dan PaketMBG tidak bergantung pada PengelolaMBG.
// berbeda dengan Composition di mana jika parent hancur, child juga ikut hancur 

public class PengelolaMBG
{
    private List<Sekolah> daftarSekolah = new List<Sekolah>();
    private List<PaketMBG> daftarPaket = new List<PaketMBG>();

    public void TambahSekolah(Sekolah sekolah)
    {
        if (sekolah == null)
        {
            Console.WriteLine("Sekolah tidak valid");
            return;
        }
        daftarSekolah.Add(sekolah);
    }

    public void TambahPaket(PaketMBG paket)
    {
        if (paket == null)
        {
            Console.WriteLine("Paket tidak valid");
            return;
        }
        daftarPaket.Add(paket);
    }

    public void TampilkanDataMBG()
    {
        Console.WriteLine("\nSekolah Penerima MBG");
        if (daftarSekolah.Count == 0)
        {
            Console.WriteLine("Belum ada terdaftar");
        }
        foreach (Sekolah s in daftarSekolah)
        {
            s.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("\nPersediaan Paket Dapur");
        if (daftarPaket.Count == 0)
        {
            Console.WriteLine("Belum ada terdaftar");
        }
        foreach (PaketMBG p in daftarPaket)
        {
            p.TampilInfo(); 
            Console.WriteLine();
        }
    }
}

public class Program
{
    static void Main()
    {
        //objek sekolah
        Sekolah sekolah1 = new Sekolah("1", "SDN 01 Jember");
        Sekolah sekolah2 = new Sekolah("2", "SMP Negeri 2 Jember", 20);
        sekolah1.TampilInfo();
        Console.WriteLine();
        sekolah2.TampilInfo();

        PaketMBG paketReguler = new PaketReguler("1", "Paket Reguler", 100);
        PaketMBG paketKhusus = new PaketKhusus("2", "Paket Vegetarian", 50, "Vegetarian");

        Console.WriteLine($"\nStok awal {paketReguler.NamaPaket}: {paketReguler.StokDapur}");
        bool hasil = paketReguler.KurangiStok(-5);
        Console.WriteLine($"KurangiStok -5 berhasil? {hasil}, stok sekarang: {paketReguler.StokDapur}");
        hasil = paketReguler.KurangiStok(500);
        Console.WriteLine($"KurangiStok 500 berhasil? {hasil}, stok sekarang: {paketReguler.StokDapur}");

        PaketMBG[] daftarPaket = { paketReguler, paketKhusus };
        foreach (PaketMBG p in daftarPaket)
        {
            Console.WriteLine($"{p.NamaPaket}: 10 paket = Rp{p.HitungBiaya(10)}");
        }

        Console.WriteLine("\nDistribusi 1");
        DapatDidistribusikan kirim1 = new DistribusiMBG(sekolah1, paketReguler, 30);
        kirim1.KirimPaket();
        Console.WriteLine($"Stok {paketReguler.NamaPaket}: {paketReguler.StokDapur}, diterima {sekolah1.namaSekolah}: {sekolah1.jumlahPaketDiterima}");

        Console.WriteLine("\nDistribusi 2");
        DapatDidistribusikan kirim2 = new DistribusiMBG(sekolah2, paketKhusus, 500);
        kirim2.KirimPaket();
        Console.WriteLine($"Stok {paketKhusus.NamaPaket}: {paketKhusus.StokDapur}, diterima {sekolah2.namaSekolah}: {sekolah2.jumlahPaketDiterima}");

        Console.WriteLine("\nDistribusi 3");
        DapatDidistribusikan kirim3 = new DistribusiMBG(sekolah2, paketKhusus, 0);
        kirim3.KirimPaket();

        Console.WriteLine("\nDistribusi 4");
        DapatDidistribusikan kirim4 = new DistribusiMBG(sekolah2, paketKhusus, 15);
        kirim4.KirimPaket();
        Console.WriteLine($"Stok {paketKhusus.NamaPaket}: {paketKhusus.StokDapur}, diterima {sekolah2.namaSekolah}: {sekolah2.jumlahPaketDiterima}");

        PengelolaMBG pengelola = new PengelolaMBG();
        pengelola.TambahSekolah(sekolah1);
        pengelola.TambahSekolah(sekolah2);
        pengelola.TambahPaket(paketReguler);
        pengelola.TambahPaket(paketKhusus);
        pengelola.TampilkanDataMBG();
    }
}