using Prism.Mvvm;

namespace GPM.CustomAnalysis.DynamicRecon;

public class DynamicReconParamsOptions : BindableBase
{
	private float initialKF = 5.6f;
	private float initialKSI = 1.6f;

	private string inputVoltageCSV = @"D:\Cameca\R73_03003 Voltage History.csv";
	private string outputParameterData = @"D:\Cameca\Parameters_Evolution.dat";

	public float InitialKF
	{
		get => initialKF;
		set => SetProperty(ref initialKF, value);
	}

	public float InitialKSI
	{
		get => initialKSI;
		set => SetProperty(ref initialKSI, value);
	}

	public string InputVoltageCSV
	{
		get => inputVoltageCSV;
		set => SetProperty(ref inputVoltageCSV, value);
	}
	public string OutputParameterData
	{
		get => outputParameterData;
		set => SetProperty(ref outputParameterData, value);
	}
}
