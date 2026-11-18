using System;
using System.Globalization;

namespace StudentRecordsSystem
{
    // Konsol menüsü: kullanıcı etkileşiminin giriş noktası.
    public class Program
    {
        // Uygulama boyunca aynı servis örneği kullanılır (bellekte tek ağaç).
        private static StudentService _service = new StudentService();

        public static void Main(string[] args)
        {
            // Türkçe karakterlerin doğru görünmesi için.
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Öğrenci Kayıt Sistemi (BST Prototipi) ===");
            Console.WriteLine("CS0315 Algorithms and Data Structures — Case Study");
            Console.WriteLine();

            bool running = true;
            while (running)
            {
                PrintMenu();
                Console.Write("Seçiminiz: ");
                string choice = Console.ReadLine();

                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        LoadDemo();
                        break;
                    case "2":
                        AddStudentInteractive();
                        break;
                    case "3":
                        SearchInteractive();
                        break;
                    case "4":
                        UpdateInteractive();
                        break;
                    case "5":
                        DeleteInteractive();
                        break;
                    case "6":
                        _service.ListAllStudents();
                        Console.WriteLine("Toplam kayıt: " + _service.GetCount());
                        break;
                    case "7":
                        // Brief: en az 8 test + expected/actual kanıtı.
                        TestRunner.RunAll();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("Program sonlandırıldı.");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim. 0-7 arası bir değer girin.");
                        break;
                }

                Console.WriteLine();
            }
        }

        // Ana menüyü yazar.
        private static void PrintMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Varsayılan Kurgusal Verileri Yükle");
            Console.WriteLine("2. Yeni Öğrenci Ekle");
            Console.WriteLine("3. Öğrenci Ara (ID ile)");
            Console.WriteLine("4. Öğrenci Güncelle");
            Console.WriteLine("5. Öğrenci Sil");
            Console.WriteLine("6. Tüm Öğrencileri Listele (In-Order)");
            Console.WriteLine("7. Otomatik Test Suite'ini Çalıştır");
            Console.WriteLine("0. Çıkış");
            Console.WriteLine("----------------------------------------");
        }

        // Hazır kurgusal verileri yükler.
        private static void LoadDemo()
        {
            // İkinci kez yüklemede mükerrer ID'ler reddedilir.
            int before = _service.GetCount();
            _service.LoadDemoData();
            int after = _service.GetCount();
            Console.WriteLine("Demo veriler yüklendi. Önceki: " + before + ", Şimdi: " + after);
            Console.WriteLine("(Zaten yüklü ID'ler mükerrer olduğu için tekrar eklenmez.)");
        }

        // Kullanıcıdan veri alıp öğrenci ekler.
        private static void AddStudentInteractive()
        {
            int id;
            double gpa;
            if (!ReadInt("StudentId: ", out id))
            {
                return;
            }

            Console.Write("FullName: ");
            string name = Console.ReadLine();
            Console.Write("Department: ");
            string dept = Console.ReadLine();

            if (!ReadDouble("GPA (0.00-4.00): ", out gpa))
            {
                return;
            }

            string message;
            bool ok = _service.AddStudent(id, name, dept, gpa, out message);
            Console.WriteLine((ok ? "[OK] " : "[HATA] ") + message);
        }

        // ID ile arama yapar.
        private static void SearchInteractive()
        {
            int id;
            if (!ReadInt("Aranacak StudentId: ", out id))
            {
                return;
            }

            Student found = _service.FindById(id);
            if (found == null)
            {
                Console.WriteLine("Öğrenci bulunamadı.");
            }
            else
            {
                Console.WriteLine("Bulundu: " + found.ToString());
            }
        }

        // Var olan kaydı günceller.
        private static void UpdateInteractive()
        {
            int id;
            double gpa;
            if (!ReadInt("Güncellenecek StudentId: ", out id))
            {
                return;
            }

            Console.Write("Yeni FullName: ");
            string name = Console.ReadLine();
            Console.Write("Yeni Department: ");
            string dept = Console.ReadLine();

            if (!ReadDouble("Yeni GPA (0.00-4.00): ", out gpa))
            {
                return;
            }

            string message;
            bool ok = _service.UpdateStudent(id, name, dept, gpa, out message);
            Console.WriteLine((ok ? "[OK] " : "[HATA] ") + message);
        }

        // Kaydı siler.
        private static void DeleteInteractive()
        {
            int id;
            if (!ReadInt("Silinecek StudentId: ", out id))
            {
                return;
            }

            string message;
            bool ok = _service.DeleteStudent(id, out message);
            Console.WriteLine((ok ? "[OK] " : "[HATA] ") + message);
        }

        // Tam sayı okur; hatalıysa false döner.
        private static bool ReadInt(string prompt, out int value)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();
            if (!int.TryParse(text, out value))
            {
                Console.WriteLine("Geçersiz tam sayı.");
                return false;
            }

            return true;
        }

        // Ondalık sayı okur (hem 3.5 hem 3,5).
        private static bool ReadDouble(string prompt, out double value)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();

            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                || double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            {
                return true;
            }

            Console.WriteLine("Geçersiz ondalık sayı.");
            return false;
        }
    }
}
