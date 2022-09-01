using Cameca.CustomAnalysis.Interface;
using Cameca.CustomAnalysis.Utilities;
using Prism.Ioc;
using Prism.Modularity;

namespace GPM.CustomAnalysis.DynamicRecon;

public class DynamicReconModule : IModule
{
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.AddCustomAnalysisUtilities();

        containerRegistry.Register<object, DynamicReconParamsNode>(DynamicReconParamsNode.UniqueId);
        containerRegistry.RegisterInstance<INodeDisplayInfo>(DynamicReconParamsNode.DisplayInfo, DynamicReconParamsNode.UniqueId);
        containerRegistry.Register<IAnalysisMenuFactory, DynamicReconParamsNodeMenuFactory>(DynamicReconParamsNodeMenuFactory.UniqueId);
        containerRegistry.Register<object, DynamicReconParamsViewModel>(DynamicReconParamsViewModel.UniqueId);
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
        var extensionRegistry = containerProvider.Resolve<IExtensionRegistry>();
        extensionRegistry.RegisterAnalysisView<DynamicReconParamsView, DynamicReconParamsViewModel>(AnalysisViewLocation.Top);
    }
}