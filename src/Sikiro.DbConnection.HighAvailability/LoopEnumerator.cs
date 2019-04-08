using System;
using System.Collections;

namespace Sikiro.DbConnection.HighAvailability
{
    /// <summary>
    /// 循环迭代器(如果position超出长度则归0)
    /// </summary>
    internal class LoopEnumerator : IEnumerator
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

        public int Length => _list.Length;

        public object Current
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