float4x4 View;
float4x4 Projection;
float2 ViewportScale;
float CurrentTime;
float Duration;

texture Texture;
sampler Sampler = sampler_state
{
    Texture = (Texture);
    
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Point;
    
    AddressU = Clamp;
    AddressV = Clamp;
};

struct VertexShaderInput
{
    float2 Corner : POSITION0;
    float3 Position : POSITION1;
    float3 Velocity : NORMAL0;
    float4 Random : COLOR0;
    float Time : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinate : COLOR1;
};


VertexShaderOutput ParticleVertexShader(VertexShaderInput input)
{
    VertexShaderOutput output;
    float age = CurrentTime - input.Time;
    float normalizedAge = saturate(age / Duration);
	
	input.Position += input.Velocity*normalizedAge;
    output.Position = mul(mul(float4(input.Position, 1), View), Projection);
	
	float scale = 1;
    output.Position.xy += input.Corner * ViewportScale*scale;
    output.Color = (0, 0, 0, 1);

    output.TextureCoordinate = (input.Corner + 1) / 2;
    
    return output;
}

float4 ParticlePixelShader(VertexShaderOutput input) : COLOR0
{
    return tex2D(Sampler, input.TextureCoordinate) * input.Color;
}

technique Particles
{
    pass P0
    {
        VertexShader = compile vs_2_0 ParticleVertexShader();
        PixelShader = compile ps_2_0 ParticlePixelShader();
    }
}
