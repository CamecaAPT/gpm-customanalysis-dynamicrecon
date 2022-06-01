using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using Cameca.CustomAnalysis.Interface;

namespace GPM.CustomAnalysis.DynamicRecon;

internal class DynamicReconParams
{

	public DynamicReconResults? Run(IIonData ionData, DynamicReconParamsOptions options)
	{

		// Initial parameters : Field factor and ICF (ksi)
		double kf0 = options.InitialKF;     // Field factor
		double ksi0 = options.InitialKSI;   // ICF

		// Conversion FR-US
		Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
		Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

		// Read scv file and store Nat and V in Data.structure (Nat and V)
		List<stcData> Data = new List<stcData>(); Data.Clear();
		int counter_Vertex = 0;

		FileStream fileStream;
		try
		{
			fileStream = File.OpenRead(options.InputVoltageCSV);
		}
		catch (IOException)
		{
			return null;
		}
		StreamReader Potential = new StreamReader(fileStream);
		if (Potential is null) return null;
		string[] array_Vertex = new string[4];
		stcData Data_temp = new stcData();
		while (Potential.ReadLine() is { } line_Vertex)
		{
			if (counter_Vertex > 0)
			{
				array_Vertex = line_Vertex.Split(',');
				if (array_Vertex.Length <= 2)
				{
					Data_temp.Nat = int.Parse(array_Vertex[0]);
					Data_temp.V = double.Parse(array_Vertex[1]);
					Data.Add(Data_temp);
				}
				else
				{
					Data_temp.Nat = int.Parse(array_Vertex[0]);
					Data_temp.V = double.Parse(array_Vertex[1]) + double.Parse(array_Vertex[2]) / 100;
					Data.Add(Data_temp);
				}
			}
			counter_Vertex++;
		}
		Potential.Close();

		// Evolution of the reconstruction parameters using the Lambert function
		double[] kf = new double[Data.Count()];
		double[] ksi = new double[Data.Count()];
		double[] Vratio = new double[Data.Count()];
		double[] Lu = new double[Data.Count()];
		double omega = LambertW(1);
		for (int i = 0; i < Data.Count(); i++)
		{
			Vratio[i] = Data[0].V / Data[i].V;
			Lu[i] = Math.Exp(omega) / (Math.Exp(LambertW(Vratio[i])) - Math.Log(Vratio[i]));
			kf[i] = kf0 * Lu[i];
			ksi[i] = ksi0 * Math.Pow(kf[i] / kf0, (1.0 / 3.0));
		}

		// Normalized evolution
		double[] kf_N = new double[Data.Count()];
		double[] ksi_N = new double[Data.Count()];
		double ksi_max = ksi.Max();
		double kf_max = kf.Max();
		for (int i = 0; i < Data.Count(); i++)
		{
			kf_N[i] = kf[i] / kf_max;
			ksi_N[i] = ksi[i] / ksi_max;

		}

		// Save data in a .dat file
		//StreamWriter Data_Res = new StreamWriter(@"D:\Cameca\Parameters_Evolution.dat");
		StreamWriter Data_Res = new StreamWriter(options.OutputParameterData);
		for (int i = 0; i < Data.Count(); i++)
		{
			Data_Res.WriteLine(Data[i].Nat + " " + Data[i].V + " " + kf[i] + " " + ksi[i] + " " + kf_N[i] + " " + ksi_N[i]);
		}
		Data_Res.Close();

		// Message box to indicate the computation end
		//MessageBox.Show("Computation done !! Press OK to close!");

		// Display the results
		float[] natDat = new float[Data.Count()];
		float[] Voltage = new float[Data.Count()];
		for (int i = 0; i < Data.Count(); i++)
		{
			natDat[i] = (float)(Data[i].Nat);
			Voltage[i] = (float)(Data[i].V);
		}

		float[] Field_Factor = new float[Data.Count()];
		float[] ICF = new float[Data.Count()];
		float[] Field_FactorN = new float[Data.Count()];
		float[] ICFN = new float[Data.Count()];
		for (int i = 0; i < Data.Count(); i++)
		{
			Field_Factor[i] = (float)(kf[i]);
			ICF[i] = (float)(ksi[i]);
			Field_FactorN[i] = (float)(kf_N[i]);
			ICFN[i] = (float)(ksi_N[i]);
		}

		return new DynamicReconResults
		{
			Voltage = natDat.Zip(Voltage).Select(x => new Vector3(x.First, 0f, x.Second)).ToArray(),
			FieldFactor = natDat.Zip(Field_Factor).Select(x => new Vector3(x.First, 0f, x.Second)).ToArray(),
			ICF = natDat.Zip(ICF).Select(x => new Vector3(x.First, 0f, x.Second)).ToArray(),
			NormalizedFieldFactor = natDat.Zip(Field_FactorN).Select(x => new Vector3(x.First, 0f, x.Second)).ToArray(),
			NormalizedICF = natDat.Zip(ICFN).Select(x => new Vector3(x.First, 0f, x.Second)).ToArray(),
		};
	}

	// Data structure
	public struct stcData
	{
		public int Nat;
		public double V;

	}

	// Lambert function
	public static double LambertW(double x)
	{
		// LambertW is not defined in this section
		if (x < -Math.Exp(-1))
			throw new Exception("The LambertW-function is not defined for " + x + ".");

		// computes the first branch for real values only

		// amount of iterations (empirically found)
		int amountOfIterations = Math.Max(4, (int)Math.Ceiling(Math.Log10(x) / 3));

		// initial guess is based on 0 < ln(a) < 3
		double w = 3 * Math.Log(x + 1) / 4;

		// Halley's method via eqn (5.9) in Corless et al (1996)
		for (int i = 0; i < amountOfIterations; i++)
			w = w - (w * Math.Exp(w) - x) / (Math.Exp(w) * (w + 1) - (w + 2) * (w * Math.Exp(w) - x) / (2 * w + 2));

		return w;
	}
}
