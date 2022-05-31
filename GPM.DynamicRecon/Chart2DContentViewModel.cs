using System;
using System.Collections.Generic;
using System.Linq;
using Cameca.CustomAnalysis.Interface;

namespace GPM.CustomAnalysis.DynamicRecon;

internal class Chart2DContentViewModel : IDisposable
{
	public string Title { get; }

	public string AxisXLabel { get; }

	public string AxisYLabel { get; }

	public ICollection<IRenderData> RenderData { get; }

	public Chart2DContentViewModel(string title, string axisXLabel, string axisYLabel, IEnumerable<IRenderData> content)
	{
		Title = title;
		AxisXLabel = axisXLabel;
		AxisYLabel = axisYLabel;
		RenderData = content.ToList();
	}

	public void Dispose()
	{
		foreach (var item in RenderData)
		{
			if (item is IDisposable disposable)
				disposable.Dispose();
		}
	}
}
