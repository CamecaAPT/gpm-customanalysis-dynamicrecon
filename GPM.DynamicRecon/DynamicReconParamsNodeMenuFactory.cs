using Cameca.CustomAnalysis.Interface;
using Prism.Commands;
using Prism.Events;

namespace GPM.CustomAnalysis.DynamicRecon;

internal class DynamicReconParamsNodeMenuFactory : IAnalysisMenuFactory
{

	public const string UniqueId = "GPM.CustomAnalysis.DynamicRecon.DynamicReconParamsNodeMenuFactory";

	private readonly IEventAggregator _eventAggregator;

	public DynamicReconParamsNodeMenuFactory(IEventAggregator eventAggregator)
	{
		_eventAggregator = eventAggregator;
	}

	public IMenuItem CreateMenuItem(IAnalysisMenuContext context) => new MenuAction(
		DynamicReconParamsNode.DisplayInfo.Title,
		new DelegateCommand(() => _eventAggregator.PublishCreateNode(
			DynamicReconParamsNode.UniqueId,
			context.NodeId,
			DynamicReconParamsNode.DisplayInfo.Title,
			DynamicReconParamsNode.DisplayInfo.Icon)),
		DynamicReconParamsNode.DisplayInfo.Icon);

	public AnalysisMenuLocation Location { get; } = AnalysisMenuLocation.Analysis;
}
