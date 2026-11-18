using System;
using System.Text;

namespace StudentRecordsSystem
{
    // Otomatik test senaryoları.
    // Her test: [Test Adı], [Beklenen], [Gerçekleşen], [PASS/FAIL] yazar.
    // Not: Ana veri yapısı List değildir; StringBuilder yalnızca test çıktısı biriktirmek içindir.
    public static class TestRunner
    {
        private static int _passed;
        private static int _failed;

        // Tüm testleri sırayla çalıştırır.
        public static void RunAll()
        {
            _passed = 0;
            _failed = 0;

            Console.WriteLine();
            Console.WriteLine("========== OTOMATIK TEST SUITE ==========");
            Console.WriteLine();

            Test1_SuccessfulInserts();
            Test2_DuplicateIdRejected();
            Test3_InvalidInputRejected();
            Test4_SearchExisting();
            Test5_SearchUnknown();
            Test6_UpdateExistingAndMissing();
            Test7_DeleteLeafAndSingleChild();
            Test8_DeleteTwoChildrenAndRoot();
            Test9_InOrderAndSkewedTree();

            Console.WriteLine();
            Console.WriteLine("---------- ÖZET ----------");
            Console.WriteLine("PASS: " + _passed);
            Console.WriteLine("FAIL: " + _failed);
            Console.WriteLine("TOPLAM: " + (_passed + _failed));
            Console.WriteLine("=========================================");
            Console.WriteLine();
        }

        // Test 1: Başarılı ekleme.
        private static void Test1_SuccessfulInserts()
        {
            string name = "Test 1: Başarılı öğrenci ekleme";
            StudentService svc = new StudentService();
            string msg;
            bool a = svc.AddStudent(1001, "Ali Veli", "CS", 3.2, out msg);
            bool b = svc.AddStudent(1005, "Ayşe Nur", "EE", 3.5, out msg);
            bool c = svc.AddStudent(1003, "Can Demir", "MATH", 2.8, out msg);

            string expected = "3 ekleme true, Count=3";
            string actual = "eklemeler=" + a + "," + b + "," + c + " Count=" + svc.GetCount();
            bool pass = a && b && c && svc.GetCount() == 3;
            PrintResult(name, expected, actual, pass);
        }

        // Test 2: Mükerrer ID reddi.
        private static void Test2_DuplicateIdRejected()
        {
            string name = "Test 2: Mükerrer StudentId reddi";
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(2001, "First", "CS", 3.0, out msg);
            bool dup = svc.AddStudent(2001, "Second", "EE", 2.5, out msg);

            string expected = "duplicate=false, Count=1, mesaj mükerrer";
            string actual = "duplicate=" + dup + " Count=" + svc.GetCount() + " msg=" + msg;
            bool pass = !dup && svc.GetCount() == 1 && msg.IndexOf("Mükerrer", StringComparison.OrdinalIgnoreCase) >= 0;
            PrintResult(name, expected, actual, pass);
        }

        // Test 3: Geçersiz girdi.
        private static void Test3_InvalidInputRejected()
        {
            string name = "Test 3: Geçersiz girdi (negatif ID, boş isim, GPA>4)";
            StudentService svc = new StudentService();
            string msg1, msg2, msg3;
            bool r1 = svc.AddStudent(-5, "X", "CS", 3.0, out msg1);
            bool r2 = svc.AddStudent(3001, "", "CS", 3.0, out msg2);
            bool r3 = svc.AddStudent(3002, "Y", "CS", 4.5, out msg3);

            string expected = "üç ekleme de false, Count=0";
            string actual = "r1=" + r1 + " r2=" + r2 + " r3=" + r3 + " Count=" + svc.GetCount();
            bool pass = !r1 && !r2 && !r3 && svc.GetCount() == 0;
            PrintResult(name, expected, actual, pass);
        }

        // Test 4: Var olan ID arama.
        private static void Test4_SearchExisting()
        {
            string name = "Test 4: Var olan öğrenciyi ID ile arama";
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(4001, "Elif Kara", "Physics", 3.7, out msg);
            Student found = svc.FindById(4001);

            string expected = "bulunan ID=4001, Name=Elif Kara";
            string actual = found == null ? "null" : found.ToString();
            bool pass = found != null && found.StudentId == 4001 && found.FullName == "Elif Kara";
            PrintResult(name, expected, actual, pass);
        }

        // Test 5: Bilinmeyen ID arama.
        private static void Test5_SearchUnknown()
        {
            string name = "Test 5: Bilinmeyen ID ile arama";
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(5001, "Demo", "CS", 3.0, out msg);
            Student found = svc.FindById(9999);

            string expected = "null (bulunamadı)";
            string actual = found == null ? "null" : found.ToString();
            bool pass = found == null;
            PrintResult(name, expected, actual, pass);
        }

        // Test 6: Güncelleme (var / yok).
        private static void Test6_UpdateExistingAndMissing()
        {
            string name = "Test 6: Güncelleme (var olan + olmayan)";
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(6001, "Eski Ad", "CS", 2.0, out msg);

            bool ok = svc.UpdateStudent(6001, "Yeni Ad", "MATH", 3.9, out msg);
            Student after = svc.FindById(6001);
            bool missing = svc.UpdateStudent(6099, "Ghost", "CS", 3.0, out msg);

            bool dataOk = after != null
                && after.FullName == "Yeni Ad"
                && after.Department == "MATH"
                && Math.Abs(after.GPA - 3.9) < 0.0001;

            string expected = "ok=true, missing=false, veri güncellenmiş";
            string actual = "ok=" + ok + " missing=" + missing + " after=" + (after == null ? "null" : after.ToString());
            bool pass = ok && !missing && dataOk;
            PrintResult(name, expected, actual, pass);
        }

        // Test 7: Silme durum 1 (yaprak) ve durum 2 (tek çocuk).
        private static void Test7_DeleteLeafAndSingleChild()
        {
            string name = "Test 7: Silme Durum 1&2 (yaprak + tek çocuk)";
            // Ağaç (ekleme sırası):
            //       50
            //      /  \
            //    30    70
            //   /         \
            // 20           80
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(50, "Root", "CS", 3.0, out msg);
            svc.AddStudent(30, "L", "CS", 3.0, out msg);
            svc.AddStudent(70, "R", "CS", 3.0, out msg);
            svc.AddStudent(20, "LL", "CS", 3.0, out msg);
            svc.AddStudent(80, "RR", "CS", 3.0, out msg);

            // Durum 1: yaprak 20 silinir.
            bool delLeaf = svc.DeleteStudent(20, out msg);
            Student leafGone = svc.FindById(20);

            // Durum 2: 70 silinir; tek çocuk 80 yukarı bağlanır.
            bool delOne = svc.DeleteStudent(70, out msg);
            Student seventy = svc.FindById(70);
            Student eighty = svc.FindById(80);

            string order = CollectIds(svc);
            string expected = "leaf silindi, 70 silindi, 80 kaldı, sıralı ID: 30,50,80";
            string actual = "delLeaf=" + delLeaf + " delOne=" + delOne
                + " leafGone=" + (leafGone == null) + " seventy=" + (seventy == null)
                + " eighty=" + (eighty != null) + " order=[" + order + "]";
            bool pass = delLeaf && delOne && leafGone == null && seventy == null
                && eighty != null && order == "30,50,80";
            PrintResult(name, expected, actual, pass);
        }

        // Test 8: Silme durum 3 (iki çocuk / kök) + sıralı yapının korunması.
        private static void Test8_DeleteTwoChildrenAndRoot()
        {
            string name = "Test 8: Silme Durum 3 (iki çocuk / kök) + sıralı yapı";
            //        40
            //       /  \
            //     20    60
            //    / \   / \
            //  10  30 50 70
            StudentService svc = new StudentService();
            string msg;
            svc.AddStudent(40, "A", "CS", 3.0, out msg);
            svc.AddStudent(20, "B", "CS", 3.0, out msg);
            svc.AddStudent(60, "C", "CS", 3.0, out msg);
            svc.AddStudent(10, "D", "CS", 3.0, out msg);
            svc.AddStudent(30, "E", "CS", 3.0, out msg);
            svc.AddStudent(50, "F", "CS", 3.0, out msg);
            svc.AddStudent(70, "G", "CS", 3.0, out msg);

            // İki çocuklu kök silinir → successor (50) yerini alır.
            bool delRoot = svc.DeleteStudent(40, out msg);
            Student rootGone = svc.FindById(40);
            string order = CollectIds(svc);

            string expected = "kök 40 silindi, order=10,20,30,50,60,70";
            string actual = "delRoot=" + delRoot + " rootGone=" + (rootGone == null) + " order=[" + order + "]";
            bool pass = delRoot && rootGone == null && order == "10,20,30,50,60,70" && svc.GetCount() == 6;
            PrintResult(name, expected, actual, pass);
        }

        // Test 9: In-Order sıralama + BST'ye özel skewed (ardışık ID) durumu.
        // MANTIK: 1,2,3,4,5 sırayla eklenince ağaç sağa yatar; yine de In-Order artan sıra vermelidir.
        private static void Test9_InOrderAndSkewedTree()
        {
            string name = "Test 9: In-Order sıralama + sağa yatık (skewed) ağaç";
            StudentService svc = new StudentService();
            string msg;

            bool allOk = true;
            for (int id = 1; id <= 5; id++)
            {
                if (!svc.AddStudent(id, "S" + id, "CS", 2.5, out msg))
                {
                    allOk = false;
                }
            }

            string order = CollectIds(svc);
            string expected = "order=1,2,3,4,5 ve Count=5";
            string actual = "order=[" + order + "] Count=" + svc.GetCount();
            bool pass = allOk && order == "1,2,3,4,5" && svc.GetCount() == 5;
            PrintResult(name, expected, actual, pass);
        }

        // In-Order gezerek ID listesini "1,2,3" biçiminde üretir.
        private static string CollectIds(StudentService svc)
        {
            StringBuilder sb = new StringBuilder();
            bool first = true;
            svc.TraverseInOrder(delegate (Student s)
            {
                if (!first)
                {
                    sb.Append(",");
                }
                sb.Append(s.StudentId);
                first = false;
            });
            return sb.ToString();
        }

        // Tek test sonucunu konsola yazar.
        private static void PrintResult(string testName, string expected, string actual, bool pass)
        {
            if (pass)
            {
                _passed++;
            }
            else
            {
                _failed++;
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine("[Test Adı]      : " + testName);
            Console.WriteLine("[Beklenen]      : " + expected);
            Console.WriteLine("[Gerçekleşen]   : " + actual);
            Console.WriteLine("[DURUM]         : " + (pass ? "PASS" : "FAIL"));
        }
    }
}
