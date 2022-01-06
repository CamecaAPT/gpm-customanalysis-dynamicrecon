using Prism.Mvvm;

namespace GPM.CustomAnalysis.DynamicRecon
{
    public class DynamicReconOptions : BindableBase
    {
        private const float DefaultInitialKF = 5.6f;
        private const float DefaultInitialKSI = 1.6f;
        private const string DefaultInputVoltageCSV = @"D:\Cameca\R73_03003 Voltage History.csv";
        private const string DefaultOutputParameterData = @"D:\Cameca\Parameters_Evolution.dat";

        private float initialKF = DefaultInitialKF;
        public float InitialKF
        {
            get => initialKF;
            set => SetProperty(ref initialKF, value);
        }

        private float initialKSI = DefaultInitialKSI;
        public float InitialKSI
        {
            get => initialKSI;
            set => SetProperty(ref initialKSI, value);
        }

        private string inputVoltageCSV = DefaultInputVoltageCSV;
        public string InputVoltageCSV
        {
            get => inputVoltageCSV;
            set => SetProperty(ref inputVoltageCSV, value);
        }

        private string outputParameterData = DefaultOutputParameterData;
        public string OutputParameterData
        {
            get => outputParameterData;
            set => SetProperty(ref outputParameterData, value);
        }
	}
}
