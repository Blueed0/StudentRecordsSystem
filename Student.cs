using System;

namespace StudentRecordsSystem
{
    // Öğrenci kayıt modeli.
    // BST içinde sıralama / arama anahtarı yalnızca StudentId'dir.
    public class Student : IComparable<Student>
    {
        // Benzersiz öğrenci numarası (ağaç anahtarı). private set: ID sonradan değişmesin.
        public int StudentId { get; private set; }

        // Öğrencinin adı soyadı.
        public string FullName { get; set; }

        // Bölüm adı.
        public string Department { get; set; }

        // Not ortalaması (0.00 - 4.00).
        public double GPA { get; set; }

        // Yeni öğrenci nesnesi oluşturur.
        public Student(int studentId, string fullName, string department, double gpa)
        {
            StudentId = studentId;
            FullName = fullName;
            Department = department;
            GPA = gpa;
        }

        // BST'nin sol/sağ kararını vermesi için karşılaştırma.
        // MANTIK: Sadece StudentId bakılır.
        // Böylece Ad/Bölüm/GPA güncellense bile ağaç bağlantıları bozulmaz.
        public int CompareTo(Student other)
        {
            // null ile karşılaştırılamaz; bu nesneyi "daha büyük" say.
            if (other == null)
            {
                return 1;
            }

            return StudentId.CompareTo(other.StudentId);
        }

        // Geçersiz veriyi ağaca girmeden önce reddeder.
        // MANTIK: Validation servis/UI katmanında yapılır; BST yalnızca geçerli kayıt görür.
        public static bool TryValidate(int studentId, string fullName, string department, double gpa, out string errorMessage)
        {
            if (studentId <= 0)
            {
                errorMessage = "StudentId 0'dan büyük olmalıdır.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                errorMessage = "FullName boş olamaz.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(department))
            {
                errorMessage = "Department boş olamaz.";
                return false;
            }

            if (gpa < 0.0 || gpa > 4.0)
            {
                errorMessage = "GPA 0.00 ile 4.00 arasında olmalıdır.";
                return false;
            }

            // Tüm kontroller geçti.
            errorMessage = null;
            return true;
        }

        // ID değiştirmeden diğer alanları günceller.
        // MANTIK: Anahtar aynı kaldığı için BST sırası korunur.
        public void ApplyUpdate(string fullName, string department, double gpa)
        {
            FullName = fullName;
            Department = department;
            GPA = gpa;
        }

        // Konsolda okunabilir metin üretir.
        public override string ToString()
        {
            return "ID=" + StudentId + ", Name=" + FullName + ", Dept=" + Department + ", GPA=" + GPA.ToString("F2");
        }
    }
}
