using System;
using System.Numerics;

namespace GPM.CustomAnalysis.DynamicRecon;

internal class DynamicReconResults
{
	public Vector3[] Voltage { get; init; } = Array.Empty<Vector3>();
	public Vector3[] FieldFactor { get; init; } = Array.Empty<Vector3>();
	public Vector3[] ICF { get; init; } = Array.Empty<Vector3>();
	public Vector3[] NormalizedFieldFactor { get; init; } = Array.Empty<Vector3>();
	public Vector3[] NormalizedICF { get; init; } = Array.Empty<Vector3>();
}
