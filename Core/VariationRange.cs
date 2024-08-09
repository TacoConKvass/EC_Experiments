using Terraria;

namespace EC_Experiments.Core.DataStructures;

public record struct VariationRange {
	public VariationRange(float lower, float upper) {
		if (lower > upper) {
			Upper = lower;
			Lower = upper;
			return;
		}
		Lower = lower;
		Upper = upper;
	}
	public VariationRange() { }
	public float Lower { get; set; }
	public float Upper { get; set; }
	public float Random => Lower < Upper ? Main.rand.NextFloat(Lower, Upper) : Main.rand.NextFloat(Upper, Lower);
}