using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeRadicalStrokeCountCollection : IList<UnicodeRadicalStrokeCount>
	{
		public struct Enumerator : IEnumerator<UnicodeRadicalStrokeCount>
		{
			private readonly UnicodeRadicalStrokeCount[] items;
			private int index;

			internal Enumerator(UnicodeRadicalStrokeCount[] items)
			{
				this.items = items;
				this.index = -1;
			}

			public void Dispose() { }

			public UnicodeRadicalStrokeCount Current { get { return items[index]; } }
			object IEnumerator.Current { get { return Current; } }

			public bool MoveNext() { return index < items.Length && ++index < items.Length; }

			void IEnumerator.Reset() { this.index = -1; }
		}

		private readonly UnicodeRadicalStrokeCount[] items;

		public UnicodeRadicalStrokeCountCollection() { items = UnicodeRadicalStrokeCount.EmptyArray; }
		internal UnicodeRadicalStrokeCountCollection(UnicodeRadicalStrokeCount[] items) { this.items = items ?? UnicodeRadicalStrokeCount.EmptyArray; }

		public UnicodeRadicalStrokeCount this[int index] { get { return items[index]; } }

		UnicodeRadicalStrokeCount IList<UnicodeRadicalStrokeCount>.this[int index]
		{
			get { return items[index]; }
			set { throw new NotSupportedException(); }
		}

		public int Count { get { return items.Length; } }

		bool ICollection<UnicodeRadicalStrokeCount>.IsReadOnly { get { return true; } }

		public void Add(UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }
		public void Insert(int index, UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }

		public bool Remove(UnicodeRadicalStrokeCount item) { throw new NotSupportedException(); }
		public void RemoveAt(int index) { throw new NotSupportedException(); }

		public void Clear() { throw new NotSupportedException(); }

		public int IndexOf(UnicodeRadicalStrokeCount item) { return Array.IndexOf(items, item); }
		public bool Contains(UnicodeRadicalStrokeCount item) { return IndexOf(item) >= 0; }

		public void CopyTo(UnicodeRadicalStrokeCount[] array, int arrayIndex) { items.CopyTo(array, arrayIndex); }

		public Enumerator GetEnumerator() { return new Enumerator(items); }

		IEnumerator<UnicodeRadicalStrokeCount> IEnumerable<UnicodeRadicalStrokeCount>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
