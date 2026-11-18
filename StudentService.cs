using System;

namespace StudentRecordsSystem
{
    // İş mantığı katmanı.
    // MANTIK: Önce doğrulama, sonra BST işlemi.
    // Böylece geçersiz veri ve mükerrer ID ağaca ulaşmadan yakalanır.
    public class StudentService
    {
        // Ana veri yapısı: öğrenci kayıtları BST'de tutulur.
        private readonly BinarySearchTree<Student> _tree;

        public StudentService()
        {
            _tree = new BinarySearchTree<Student>();
        }

        // Kayıt sayısı.
        public int GetCount()
        {
            return _tree.Count;
        }

        // Yeni öğrenci ekler.
        // BIG-O: Doğrulama O(1) + Insert Average O(log n) / Worst O(n).
        public bool AddStudent(int studentId, string fullName, string department, double gpa, out string message)
        {
            // Önce alan doğrulaması.
            if (!Student.TryValidate(studentId, fullName, department, gpa, out message))
            {
                return false;
            }

            Student student = new Student(studentId, fullName.Trim(), department.Trim(), gpa);
            bool inserted = _tree.Insert(student);

            // Insert false dönerse aynı ID zaten vardır.
            if (!inserted)
            {
                message = "Mükerrer StudentId: bu ID zaten kayıtlı.";
                return false;
            }

            message = "Öğrenci başarıyla eklendi.";
            return true;
        }

        // ID ile öğrenci arar.
        // BIG-O: Average O(log n), Worst O(n).
        public Student FindById(int studentId)
        {
            Student key = CreateKey(studentId);
            return _tree.Search(key);
        }

        // Var olan öğrenciyi günceller (ID değişmez).
        // MANTIK: Aynı nesne üzerinde alanlar güncellenir → ağaç bağlantıları bozulmaz.
        // BIG-O: Search + O(1) alan ataması.
        public bool UpdateStudent(int studentId, string fullName, string department, double gpa, out string message)
        {
            if (!Student.TryValidate(studentId, fullName, department, gpa, out message))
            {
                return false;
            }

            Student existing = FindById(studentId);
            if (existing == null)
            {
                message = "Güncellenecek öğrenci bulunamadı.";
                return false;
            }

            existing.ApplyUpdate(fullName.Trim(), department.Trim(), gpa);
            _tree.Update(existing);

            message = "Öğrenci başarıyla güncellendi.";
            return true;
        }

        // Öğrenciyi siler.
        // BIG-O: Average O(log n), Worst O(n).
        public bool DeleteStudent(int studentId, out string message)
        {
            if (studentId <= 0)
            {
                message = "StudentId 0'dan büyük olmalıdır.";
                return false;
            }

            Student key = CreateKey(studentId);
            bool deleted = _tree.Delete(key);

            if (!deleted)
            {
                message = "Silinecek öğrenci bulunamadı.";
                return false;
            }

            message = "Öğrenci başarıyla silindi.";
            return true;
        }

        // Tüm öğrencileri ID'ye göre artan sırada listeler (In-Order).
        // BIG-O: O(n).
        public void ListAllStudents()
        {
            if (_tree.IsEmpty)
            {
                Console.WriteLine("Kayıtlı öğrenci yok.");
                return;
            }

            Console.WriteLine("--- Öğrenci Listesi (In-Order / ID artan) ---");
            _tree.TraverseInOrder(delegate (Student s)
            {
                Console.WriteLine(s.ToString());
            });
        }

        // Testler için In-Order dolaşımı dışarı açar.
        public void TraverseInOrder(Action<Student> action)
        {
            _tree.TraverseInOrder(action);
        }

        // Demo için kurgusal (fictional) örnek veriler yükler.
        // MANTIK: Ortadaki ID'ler önce eklenerek ağaç daha dengeli başlar.
        public void LoadDemoData()
        {
            TrySilentAdd(1005, "Ayşe Yılmaz", "Computer Science", 3.45);
            TrySilentAdd(1002, "Mehmet Demir", "Electrical Engineering", 2.90);
            TrySilentAdd(1008, "Elif Kara", "Mathematics", 3.80);
            TrySilentAdd(1001, "Can Öztürk", "Physics", 3.10);
            TrySilentAdd(1004, "Zeynep Arslan", "Computer Science", 3.65);
            TrySilentAdd(1007, "Burak Çelik", "Business", 2.75);
            TrySilentAdd(1010, "Deniz Aydın", "Chemistry", 3.20);
            TrySilentAdd(1003, "Selin Koç", "Architecture", 3.55);
        }

        // Mesajı yutarak ekleme dener (demo yükleme için).
        private void TrySilentAdd(int id, string name, string dept, double gpa)
        {
            string msg;
            AddStudent(id, name, dept, gpa, out msg);
        }

        // Search/Delete için yalnızca ID taşıyan geçici anahtar nesnesi.
        // MANTIK: CompareTo sadece ID'ye baktığı için diğer alanlar önemli değildir.
        private static Student CreateKey(int studentId)
        {
            return new Student(studentId, "_", "_", 0.0);
        }
    }
}
