namespace StudentRecordsSystem
{
    // BST'nin tek bir düğümünü temsil eder.
    // Sol çocuk daha küçük, sağ çocuk daha büyük anahtar tutar.
    public class Node<T>
    {
        // Bu düğümde saklanan veri (örneğin bir Student nesnesi).
        public T Data { get; set; }

        // Sol alt ağacın kökü (daha küçük anahtarlar).
        public Node<T> Left { get; set; }

        // Sağ alt ağacın kökü (daha büyük anahtarlar).
        public Node<T> Right { get; set; }

        // Yeni düğüm oluşturur.
        // MANTIK: İlk eklendiğinde yapraktır; çocukları henüz yoktur.
        public Node(T data)
        {
            Data = data;
            // Null = çocuk yok demektir.
            Left = null;
            Right = null;
        }
    }
}
