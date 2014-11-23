using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeCrossReferenceCollection : IList<int>
	{
		private static int[] EmptyArray = new int[0];

		public struct Enumerator : IEnumerator<int>
		{
			private readonly int[] items;
			private int index;

			internal Enumerator(int[] items)
			{
				this.items = items;
				this.index = -1;
			}

			public void Dispose() { }

			public int Current { get { return items[index]; } }
			object IEnumerator.Current { get { return Current; } }

			public bool MoveNext() { return index < items.Length && ++index < items.Length; }

			void IEnumerator.Reset() { this.index = -1; }
		}

		private readonly int[] items;

		public UnicodeCrossReferenceCollection() { items = EmptyArray; }
		internal UnicodeCrossReferenceCollection(int[] items) { this.items = items ?? EmptyArray; }

		public int this[int index] { get { return items[index]; } }

		int IList<int>.this[int index]
		{
			get { return items[index]; }
			set { throw new NotSupportedException(); }
		}

		public int Count { get { return items.Length; } }

		bool ICollection<int>.IsReadOnly { get { return true; } }

		public void Add(int item) { throw new NotSupportedException(); }
		public void Insert(int index, int item) { throw new NotSupportedException(); }

		public bool Remove(int item) { throw new NotSupportedException(); }
		public void RemoveAt(int index) { throw new NotSupportedException(); }

		public void Clear() { throw new NotSupportedException(); }

		public int IndexOf(int item) { return Array.IndexOf(items, item); }
		public bool Contains(int item) { return IndexOf(item) >= 0; }

		public void CopyTo(int[] array, int arrayIndex) { items.CopyTo(array, arrayIndex); }

		public Enumerator GetEnumerator() { return new Enumerator(items); }

		IEnumerator<int> IEnumerable<int>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
