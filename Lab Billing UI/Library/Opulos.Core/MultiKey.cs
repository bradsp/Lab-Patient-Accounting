using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Opulos.Core.Utils {

public class MultiKey {

	public StringComparer Comparer { get; private set; }
	private IList<Object> keys2 = null;

	public MultiKey(IEnumerable e) { //ICollection col) {
		if (e is Array) {
			Array arr = (Array) e;
			keys2 = new Object[arr.Length];
			for (int i = keys2.Count - 1; i >= 0; i--)
				keys2[i] = arr.GetValue(i);
		}
		else if (e is IList) {
			IList list = (IList) e;
			keys2 = new Object[list.Count];
			for (int i = list.Count - 1; i >= 0; i--)
				keys2[i] = list[i];
		}
		else {
			//keys.AddRange(col);
			keys2 = new List<Object>();
			var e2 = e.GetEnumerator();
			while (e2.MoveNext())
				keys2.Add(e2.Current);
		}
	}

	public MultiKey(Object key1, Object key2, params Object[] keys) {
		keys2 = new Object[2 + keys.Length];
		keys2[0] = key1;
		keys2[1] = key2;
		for (int i = keys.Length - 1; i >= 0; i--)
			keys2[i+2] = keys[i];
	}

	public MultiKey(StringComparer comparer, params Object[] keys) {
		this.Comparer = comparer;
		keys2 = new Object[keys.Length];
		for (int i = keys.Length - 1; i >= 0; i--)
			keys2[i] = keys[i];
	}

	public Object[] Keys {
		get {
			return keys2.ToArray();
		}
	}

	public override bool Equals(Object obj) {
		if (!(obj is MultiKey))
			return false;

		MultiKey mk = (MultiKey) obj;
		if (mk.keys2.Count != keys2.Count)
			return false;

		var c = Comparer;
		if (c != null) {
			for (int i = 0; i < mk.keys2.Count; i++) {
				Object o1 = mk.keys2[i];
				Object o2 = keys2[i];
				if (o1 == null) {
					if (o2 == null)
						continue;
					return false;
				}

				if (o1 is String && o2 is String) {
					if (c.Compare((String) o1, (String) o2) != 0)
						return false;
				}
				else if (!o1.Equals(o2))
					return false;
			}
		}
		else {
			for (int i = 0; i < mk.keys2.Count; i++) {
				Object o1 = mk.keys2[i];
				Object o2 = keys2[i];
				if (o1 == null) {
					if (o2 == null)
						continue;
					return false;
				}
				if (!o1.Equals(o2))
					return false;
			}
		}
		return true;
	}

	public override int GetHashCode() {
		unchecked { // Overflow is fine, just wrap
			int hash = 17;
			var c = Comparer;
			if (c != null) {
				foreach (Object o in keys2) {
					if (o == null)
						continue;

					int hc;
					if (o is String)
						hc = c.GetHashCode((String) o);
					else
						hc = o.GetHashCode();

					hash = hash * 29 + hc;
				}
			}
			else {
				foreach (Object o in keys2) {
					if (o == null)
						continue;
					hash = hash * 29 + o.GetHashCode();
				}
			}

			return hash;
		}
	}

	public override String ToString() {
		StringBuilder sb = new StringBuilder();
		foreach (Object o in keys2) {
			if (sb.Length > 0)
				sb.Append(", ");

			if (o is DateTime)
				sb.Append(((DateTime) o).ToString("yyyy-MM-dd"));
			else
				sb.Append(o.ToString());
		}
		return sb.ToString();
	}
}
}