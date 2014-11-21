using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Unicode
{
	public struct UnicodeNameAliasCollection : IList<UnicodeNameAlias>
	{
		public struct Enumerator : IEnumerator<UnicodeNameAlias>
		{
			private readonly UnicodeNameAlias[] items;
			private int index;

			internal Enumerator(UnicodeNameAlias[] items)
			{
				this.items = items;
				this.index = -1;
			}

			public void Dispose() { }

			public UnicodeNameAlias Current { get { return items[index]; } }
			object IEnumerator.Current { get { return Current; } }

			public bool MoveNext() { return index < items.Length && ++index < items.Length; }

			void IEnumerator.Reset() { this.index = -1; }
		}

		private readonly UnicodeNameAlias[] items;

		public UnicodeNameAliasCollection() { items = UnicodeNameAlias.EmptyArray; }
		internal UnicodeNameAliasCollection(UnicodeNameAlias[] items) { this.items = items ?? UnicodeNameAlias.EmptyArray; }

		public UnicodeNameAlias this[int index] { get { return items[index]; } }

		UnicodeNameAlias IList<UnicodeNameAlias>.this[int index]
		{
			get { return items[index]; }
			set { throw new NotSupportedException(); }
		}

		public int Count { get { return items.Length; } }

		bool ICollection<UnicodeNameAlias>.IsReadOnly { get { return true; } }

		public void Add(UnicodeNameAlias item) { throw new NotSupportedException(); }
		public void Insert(int index, UnicodeNameAlias item) { throw new NotSupportedException(); }

		public bool Remove(UnicodeNameAlias item) { throw new NotSupportedException(); }
		public void RemoveAt(int index) { throw new NotSupportedException(); }

		public void Clear() { throw new NotSupportedException(); }

		public int IndexOf(UnicodeNameAlias item) { return Array.IndexOf(items, item); }
		public bool Contains(UnicodeNameAlias item) { return IndexOf(item) >= 0; }

		public void CopyTo(UnicodeNameAlias[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public Enumerator GetEnumerator() { return new Enumerator(items); }

		IEnumerator<UnicodeNameAlias> IEnumerable<UnicodeNameAlias>.GetEnumerator() { return GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
	}
}
