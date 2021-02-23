using System;
using System.Windows.Forms;

namespace Opulos.Core.UI {

public interface IToolTips : IDisposable {
	void Add(Control control, String text);
	void Batch(Control control, String text);
	void ApplyBatched();
	void RemoveAll();
}
}