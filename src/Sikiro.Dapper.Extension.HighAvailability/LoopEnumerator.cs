using System;
using System.Collections;

namespace Sikiro.Dapper.Extension.HighAvailability
{
    public class LoopEnumerator : IEnumerator
    {
        private readonly int[] _list;

        private int _position = -1;

        public LoopEnumerator(int[] list)
        {
            _list = list;
        }

        public bool MoveNext()
        {
            _position++;
            
            if (_position >= _list.Length)
                _position = 0;
            
            return _position < _list.Length;
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current => Current;

        private int Current
        {
            get
            {
                try
                {
                    return _list[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public static class LoopEnumeratorExtension
    {
        public static IEnumerator AsLoopEnumerator(this int[] list)
        {
            return new LoopEnumerator(list);
        }
    }
}