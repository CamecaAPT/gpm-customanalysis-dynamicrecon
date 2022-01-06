using System;
using Cameca.CustomAnalysis.Interface.CustomAnalysis;
using Prism.Ioc;
using Prism.Modularity;

namespace GPM.CustomAnalysis.DynamicRecon
{
    [ModuleDependency("IvasModule")]
    public class DynamicReconModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any additional dependencies with the Unity IoC container
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var customAnalysisService = containerProvider.Resolve<ICustomAnalysisService>();

            customAnalysisService.Register<DynamicReconCustomAnalysis, DynamicReconOptions>(
                new CustomAnalysisDescription("GPM_DynamicRecon", "GPM Dynamic Recon Params", new Version()));
        }
    }
}
