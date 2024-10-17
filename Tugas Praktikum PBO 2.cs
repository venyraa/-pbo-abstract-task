using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

using System;
using System.Collections.Generic;

public interface IKemampuan
{
    string Nama { get; }
    int Cooldown { get; }
    void Gunakan(Robot pengguna, Robot target);
}

public abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    public void Serang(Robot target)
    {
        Console.WriteLine($"{Nama} menyerang {target.Nama} !");
        int damage = Serangan - target.Armor;
        if (damage < 0) damage = 0;
        target.Energi -= damage;
        Console.WriteLine($"{target.Nama} menerima {damage} damage. Energi tersisa : {target.Energi}");
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);

    public virtual void CetakInformasi()
    {
        Console.WriteLine($"Robot: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }

    public virtual void PerbaruiStatus()
    {
        if (Energi < 100) Energi += 10;
    }
}

public class BosRobot : Robot
{
    public int Pertahanan { get; set; }
    public BosRobot(string nama, int energi, int armor, int serangan, int pertahanan)
        : base(nama, energi, armor, serangan)
    {
        Pertahanan = pertahanan;
    }

    public void Diserang(Robot penyerang)
    {
        Console.WriteLine($"{penyerang.Nama} menyerang bos {Nama}");
        int damage = penyerang.Serangan - Pertahanan;
        if (damage < 0) damage = 0;
        Energi -= damage;
        Console.WriteLine($"Bos {Nama} menerima {damage} damage, energi tersisa: {Energi}");
    }

    public bool Mati()
    {
        if (Energi <= 0)
        {
            Console.WriteLine($"{Nama} telah dikalahkan!");
            return true;
        }
        return false;
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (Energi > 0)
        {
            Console.WriteLine($"{Nama} menggunakan {kemampuan.Nama}!");
            kemampuan.Gunakan(this, target);
        }
    }
}

public class Perbaikan : IKemampuan
{
    public string Nama => "Perbaikan";
    public int Cooldown { get; private set; }

    public Perbaikan()
    {
        Cooldown = 2;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        pengguna.Energi += 20;
        Console.WriteLine($"{pengguna.Nama} memulihkan 20 energi");
    }
}

public class SeranganListrik : IKemampuan
{
    public string Nama => "Serangan Listrik";
    public int Cooldown { get; private set; }

    public SeranganListrik()
    {
        Cooldown = 4;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        int damage = 25;
        target.Armor -= 5;
        target.Energi -= damage;
        Console.WriteLine($"{target.Nama} terkena serangan listrik dan menerima {damage} damage! Armor berkurang.");
    }
}

public class PertahananSuper : IKemampuan
{
    public string Nama => "Pertahanan Super";
    public int Cooldown { get; private set; }

    public PertahananSuper()
    {
        Cooldown = 3;
    }

    public void Gunakan(Robot pengguna, Robot target)
    {
        pengguna.Armor += 10;
        Console.WriteLine($"{pengguna.Nama} meningkatkan pertahanan sebanyak 10 poin");
    }
}

public class RobotTempur : Robot
{
    public RobotTempur(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan) { }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        Console.WriteLine($"{Nama} menggunakan {kemampuan.Nama}!");
        kemampuan.Gunakan(this, target);
    }
}

public class Program
{
    public static void Main()
    {
        Robot robot1 = new RobotTempur("Roboto", 100, 10, 20);
        Robot robot2 = new RobotTempur("Robox", 100, 15, 15);
        BosRobot bos = new BosRobot("Boss Servo", 200, 20, 25, 30);

      
        IKemampuan repair = new Perbaikan();
        IKemampuan electricShock = new SeranganListrik();
        IKemampuan superShield = new PertahananSuper();

        robot1.CetakInformasi();
        robot2.CetakInformasi();
        bos.CetakInformasi();

        robot1.Serang(bos);
        bos.GunakanKemampuan(electricShock, robot2);
        robot2.GunakanKemampuan(repair, robot2);
        bos.Diserang(robot1);

        if (bos.Mati())
        {
            Console.WriteLine("Pertarungan selesai.");
        }
        Console.ReadLine();
    }
}

