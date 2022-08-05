using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Classes
{
    public class OrderedList<T> where T : class
    {
        private Dictionary<int, T> _dict = new Dictionary<int, T>();
        
        public OrderedList(int size)
        {
            for (int i = 0; i < size; i++)
            {
                _dict.Add(i, null);
            }
        }
        
        public T this[int index]
        {
            get
            {
                return null;
            }
            set
            {
                _dict[index] = value;
            }
        }
        
        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }
        
        public void Set(int index, T value)
        {
            _dict[index] = value;
        }
        
        public T Get(int index)
        {
            return _dict[index];
        }
        
        public void Remove(int index)
        {
            _dict[index] = default(T);
        }
        
        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                _dict[i] = default(T);
            }
        }
        
        public void Swap(int index1, int index2)
        {
            (_dict[index1], _dict[index2]) = (_dict[index2], _dict[index1]);
        }
        
        public bool IsEmpty(int index)
        {
            return _dict[index] == null;
        }
        
        public List<T> ToList()
        {
            return _dict.Values.ToList();
        }
    }
}