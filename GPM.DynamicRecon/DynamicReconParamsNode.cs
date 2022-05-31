using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using Cameca.CustomAnalysis.Interface;
using Cameca.CustomAnalysis.Utilities;

namespace GPM.CustomAnalysis.DynamicRecon;

[DefaultView(DynamicReconParamsViewModel.UniqueId, typeof(DynamicReconParamsViewModel))]
internal class DynamicReconParamsNode : AnalysisNodeBase
{
	public class NodeDisplayInfo : INodeDisplayInfo
	{
		public string Title { get; } = "GPM Dynamic Recon Params";
		public ImageSource? Icon { get; } = null;
	}

	public static NodeDisplayInfo DisplayInfo { get; } = new();

	public const string UniqueId = "GPM.CustomAnalysis.IsopositionFiltering.IsopositionFilteringNode";

	private readonly DynamicReconParams dynamicReconParam;

	public DynamicReconParamsOptions Options { get; private set; } = new();

	public DynamicReconParamsNode(IAnalysisNodeBaseServices services) : base(services)
	{
		dynamicReconParam = new DynamicReconParams();
	}

	public async Task<DynamicReconResults?> Run()
	{
		if (await Services.IonDataProvider.GetIonData(InstanceId) is not { } ionData)
			return null;

		return dynamicReconParam.Run(ionData, Options);
	}

	protected override byte[]? GetSaveContent()
	{
		var serializer = new XmlSerializer(typeof(DynamicReconParamsOptions));
		using var stringWriter = new StringWriter();
		serializer.Serialize(stringWriter, Options);
		return Encoding.UTF8.GetBytes(stringWriter.ToString());
	}

	protected override void OnLoaded(NodeLoadedEventArgs eventArgs)
	{
		if (eventArgs.Data is not { } data) return;
		var xmlData = Encoding.UTF8.GetString(data);
		var serializer = new XmlSerializer(typeof(DynamicReconParamsOptions));
		using var stringReader = new StringReader(xmlData);
		if (serializer.Deserialize(stringReader) is DynamicReconParamsOptions loadedOptions)
		{
			Options = loadedOptions;
		}
	}

	protected override void OnInstantiated(INodeInstantiatedEventArgs eventArgs)
	{
		base.OnInstantiated(eventArgs);
		Options.PropertyChanged += OptionsOnPropertyChanged;
	}

	private void OptionsOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (CanSaveState is { } canSaveState)
		{
			canSaveState.CanSave = true;
		}
	}
}
