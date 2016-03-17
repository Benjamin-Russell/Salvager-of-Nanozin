uniform extern texture ScreenTexture;

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;
};

float fadeLevel;

float4 PixelShaderFunction(float2 curCoord: TEXCOORD0) : COLOR
{
	//get color
	float4 color = tex2D(ScreenS, curCoord);

	if (fadeLevel < 1)
	color.r *= fadeLevel;

	color.g *= fadeLevel;
	color.b *= fadeLevel;

	return color;
}
technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}