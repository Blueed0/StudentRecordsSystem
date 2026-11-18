using System;

namespace StudentRecordsSystem
{
    // Generic Binary Search Tree (ikili arama ağacı).
    // Yapısal kural (invariant):
    //   - Sol alt ağaçtaki tüm anahtarlar düğümden küçüktür.
    //   - Sağ alt ağaçtaki tüm anahtarlar düğümden büyüktür.
    //   - Mükerrer (aynı) anahtar eklenemez.
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        // Ağacın kök düğümü. null ise ağaç boştur.
        private Node<T> _root;

        // Kayıt sayısını O(1) bilmek için ayrı tutulur.
        private int _count;

        // Ağaçtaki eleman sayısı.
        public int Count
        {
            get { return _count; }
        }

        // Ağaç boş mu?
        public bool IsEmpty
        {
            get { return _root == null; }
        }

        // Yeni kayıt ekler.
        // MANTIK: Kökten başla, CompareTo ile sol/sağ seç; eşitlik = mükerrer → false.
        // BIG-O: Average O(log n), Worst O(n) (sıralı eklemede zincir / skewed).
        public bool Insert(T item)
        {
            if (item == null)
            {
                return false;
            }

            // İlk eleman doğrudan kök olur.
            if (_root == null)
            {
                _root = new Node<T>(item);
                _count = 1;
                return true;
            }

            Node<T> current = _root;
            Node<T> parent = null;

            // Uygun yaprak konumunu bulana kadar in.
            while (current != null)
            {
                parent = current;
                int cmp = item.CompareTo(current.Data);

                // Aynı anahtar zaten var → invariant bozulmasın diye ekleme.
                if (cmp == 0)
                {
                    return false;
                }

                if (cmp < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            // current null oldu; parent'ın altına yeni yaprak bağlanır.
            int parentCmp = item.CompareTo(parent.Data);
            if (parentCmp < 0)
            {
                parent.Left = new Node<T>(item);
            }
            else
            {
                parent.Right = new Node<T>(item);
            }

            _count++;
            return true;
        }

        // Anahtara göre arar; bulamazsa null/default döner.
        // MANTIK: Insert ile aynı yol mantığı; eşitlik bulununca dur.
        // BIG-O: Average O(log n), Worst O(n).
        public T Search(T key)
        {
            Node<T> node = FindNode(key);
            if (node == null)
            {
                return default(T);
            }

            return node.Data;
        }

        // Mevcut kaydı günceller (anahtar aynı kalmalıdır).
        // MANTIK: Düğüm bulunur, Data değiştirilir; sol/sağ bağlantılara dokunulmaz.
        // BIG-O: Average O(log n), Worst O(n).
        public bool Update(T updatedItem)
        {
            if (updatedItem == null)
            {
                return false;
            }

            Node<T> node = FindNode(updatedItem);
            if (node == null)
            {
                return false;
            }

            // Anahtar aynı olduğu sürece BST sırası bozulmaz.
            node.Data = updatedItem;
            return true;
        }

        // Kaydı siler. Üç durum:
        // 1) Yaprak (0 çocuk)
        // 2) Tek çocuk
        // 3) İki çocuk → In-Order Successor (sağ alt ağacın en küçüğü)
        // BIG-O: Average O(log n), Worst O(n).
        public bool Delete(T key)
        {
            if (_root == null || key == null)
            {
                return false;
            }

            Node<T> parent = null;
            Node<T> current = _root;

            // Silinecek düğümü bul.
            while (current != null)
            {
                int cmp = key.CompareTo(current.Data);
                if (cmp == 0)
                {
                    break;
                }

                parent = current;
                if (cmp < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            // Anahtar ağaçta yok.
            if (current == null)
            {
                return false;
            }

            // Durum 3: İki çocuk varsa successor ile veri değiştir.
            // MANTIK: Successor = sağ alt ağacın en solundaki düğüm (silinen değerden sonraki sıralı değer).
            if (current.Left != null && current.Right != null)
            {
                Node<T> successorParent = current;
                Node<T> successor = current.Right;

                while (successor.Left != null)
                {
                    successorParent = successor;
                    successor = successor.Left;
                }

                // Sıralı yapıyı korumak için veriyi kopyala.
                current.Data = successor.Data;

                // Artık asıl silinecek düğüm successor'dır (0 veya 1 çocuklu).
                current = successor;
                parent = successorParent;
            }

            // Durum 1 veya 2: 0 ya da 1 çocuk.
            // Varsa tek çocuğu al; yoksa null (yaprak).
            Node<T> child = current.Left != null ? current.Left : current.Right;

            if (parent == null)
            {
                // Kök siliniyor.
                _root = child;
            }
            else if (parent.Left == current)
            {
                parent.Left = child;
            }
            else
            {
                parent.Right = child;
            }

            _count--;
            return true;
        }

        // In-Order dolaşım: sol → düğüm → sağ.
        // MANTIK: Bu sıra anahtarları küçükten büyüğe üretir.
        // BIG-O: O(n) — her düğüm bir kez ziyaret edilir.
        public void TraverseInOrder(Action<T> action)
        {
            if (action == null)
            {
                return;
            }

            InOrderRecursive(_root, action);
        }

        // In-Order'un özyinelemeli (recursive) yardımcı metodu.
        private void InOrderRecursive(Node<T> node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            InOrderRecursive(node.Left, action);
            action(node.Data);
            InOrderRecursive(node.Right, action);
        }

        // Anahtara göre düğümü bulur; yoksa null.
        private Node<T> FindNode(T key)
        {
            Node<T> current = _root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Data);
                if (cmp == 0)
                {
                    return current;
                }

                if (cmp < 0)
                {
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return null;
        }
    }
}
