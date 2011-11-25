// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

// vertex shader input, http://forums.create.msdn.com/forums/p/71866/438467.aspx
float4x4 MatrixTransform : register(vs, c0);

/// variables
float Time = 0.0;

// tex sampler
sampler TextureSampler : register(s0) = 
sampler_state
{
    AddressU = Wrap; // can use Clamp, or Wrap for different fx
    AddressV = Wrap;
};

// pixshader function
float4 PS_Draw(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 x = texCoord + noise(Time);
	return tex2D(TextureSampler, x); 
}

// see http://forums.create.msdn.com/forums/p/71866/438467.aspx
void SpriteVertexShader(inout float4 color    : COLOR0, 
                        inout float2 texCoord : TEXCOORD0, 
                        inout float4 position : SV_Position) 
{ 
    position = mul(position, MatrixTransform); 
} 


technique Draw
{
    pass Pass1
    {
		VertexShader = compile vs_2_0 SpriteVertexShader();
        PixelShader = compile ps_2_0 PS_Draw();
    }
}
