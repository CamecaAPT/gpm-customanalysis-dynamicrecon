using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Cameca.CustomAnalysis.Interface;
using Cameca.CustomAnalysis.Utilities;
using CommunityToolkit.Mvvm.Input;

namespace GPM.CustomAnalysis.DynamicRecon;

internal class DynamicReconParamsViewModel : AnalysisViewModelBase<DynamicReconParamsNode>
{
	public const string UniqueId = "GPM.CustomAnalysis.DynamicRecon.DynamicReconParamsViewModel";


	private readonly IRenderDataFactory renderDataFactory;
	private bool optionsChanged = false;

	private readonly AsyncRelayCommand runCommand;
	public ICommand RunCommand => runCommand;

	public DynamicReconParamsOptions Options => Node!.Options;

	public ObservableCollection<object> Tabs { get; } = new();

	private object? selectedTab;
	public object? SelectedTab
	{
		get => selectedTab;
		set => SetProperty(ref selectedTab, value);
	}

	public DynamicReconParamsViewModel(IAnalysisViewModelBaseServices services,
		IRenderDataFactory renderDataFactory) : base(services)
	{
		this.renderDataFactory = renderDataFactory;
		runCommand = new AsyncRelayCommand(OnRun, UpdateSelectedEventCountsEnabled);
	}

	protected override void OnCreated(ViewModelCreatedEventArgs eventArgs)
	{
		base.OnCreated(eventArgs);
		if (Node is { } node)
		{
			node.Options.PropertyChanged += OptionsOnPropertyChanged;
		}
	}

	private async Task OnRun()
	{
		foreach (var item in Tabs)
		{
			if (item is IDisposable disposable)
				disposable.Dispose();
		}
		Tabs.Clear();

		var data = await Node!.Run();
		if (data is null) return;
		
		// Build render data output
		Tabs.Add(new Chart2DContentViewModel(
			"Voltage", "Number of Impacts", "Voltage (V)",
			new IRenderData[]
			{
				renderDataFactory.CreateLine(data.Voltage, Colors.Red, name: "Voltage"),
			}));
		Tabs.Add(new Chart2DContentViewModel(
			"Recon Parameters", "Number of Impacts", "Field Factor and ICF",
			new IRenderData[]
			{
				renderDataFactory.CreateLine(data.FieldFactor, Colors.Red, name: "Field Factor"),
				renderDataFactory.CreateLine(data.ICF, Colors.Blue, name: "ICF"),
			}));
		Tabs.Add(new Chart2DContentViewModel(
			"Normalized Recon Parameters", "Number of Impacts", "Normalized Field Factor and ICF",
			new IRenderData[]
			{
				renderDataFactory.CreateLine(data.NormalizedFieldFactor, Colors.Red, name: "Normalized Field Factor"),
				renderDataFactory.CreateLine(data.NormalizedICF, Colors.Blue, name: "Normalized ICF"),
			}));
		SelectedTab = Tabs.FirstOrDefault();
	}

	private void OptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		optionsChanged = true;
		runCommand.NotifyCanExecuteChanged();
	}


	private bool UpdateSelectedEventCountsEnabled() => !Tabs.Any() || optionsChanged;
}
