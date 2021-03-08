// Effect converts pixels in a texture to be grayscale by using the dot operation

float Percentage;

sampler TextureSampler : register(s0);
float4 PixelShaderFunction(float2 Tex: TEXCOORD0) : COLOR0
{
	float4 Color = tex2D(TextureSampler, Tex);
	float r = Color.r;
	float g = Color.g;
	float b = Color.b;
	Color.rgb = dot(Color.rgb, float3(0.7 * Percentage, 0.59 * Percentage, 0.11 * Percentage));
	//r = r - (r - Color.rgb) * Percentage;
	g = g - (g - Color.rgb) * Percentage;
	b = b - (b - Color.rgb) * Percentage;
	Color.r = r;
	Color.g = g;
	Color.b = b;

	return Color;
}

technique hit
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}