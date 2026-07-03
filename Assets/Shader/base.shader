// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "texiao/base"
{
	Properties
	{
		_TextureSample0("主题图", 2D) = "white" {}
		[HDR]_Color0("主题图颜色", Color) = (0,0,0,0)
		_Float0("主图对比度", Float) = 1
		[Toggle(_KEYWORD0_ON)] _Keyword0("是否有Alpha通道", Float) = 0
		_TextureSample1("遮罩图", 2D) = "white" {}
		_Float1("遮罩对比度", Float) = 1
		_Vector0("主图的流动速度", Vector) = (0,0,0,0)
		_Vector3("扭曲图的流动速度", Vector) = (0,0,0,0)
		_TextureSample3("多功能图", 2D) = "white" {}
		_Float2("多功能图的强度", Float) = 1
		_Float3("多功能图", Range( 0 , 1)) = 0
		_TextureSample4("额外遮罩图", 2D) = "white" {}
		_TextureSample5("扭曲图", 2D) = "white" {}
		_Float4("扭曲图强度", Float) = 0
		_Float6("x_流动速度", Range( 0 , 1)) = 0
		_Float7("Y_流动速度", Range( 0 , 1)) = 0
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD0_ON
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
		};

		uniform sampler2D _TextureSample0;
		uniform float2 _Vector0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample5;
		uniform float2 _Vector3;
		uniform float _Float4;
		uniform float4 _Color0;
		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform float _Float2;
		uniform float _Float3;
		uniform float _Float0;
		uniform sampler2D _TextureSample1;
		uniform float _Float6;
		uniform float _Float7;
		uniform float _Float1;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 panner25 = ( 1.0 * _Time.y * _Vector0 + uv0_TextureSample0);
			float2 panner46 = ( 1.0 * _Time.y * _Vector3 + i.uv_texcoord);
			float4 tex2DNode2 = tex2D( _TextureSample0, ( float4( panner25, 0.0 , 0.0 ) + ( tex2D( _TextureSample5, panner46 ) * _Float4 ) ).rg );
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			float4 lerpResult36 = lerp( ( ( i.vertexColor * tex2DNode2 ) * _Color0 ) , ( tex2D( _TextureSample3, uv_TextureSample3 ) * _Float2 ) , _Float3);
			o.Emission = lerpResult36.rgb;
			float3 desaturateInitialColor12 = tex2DNode2.rgb;
			float desaturateDot12 = dot( desaturateInitialColor12, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar12 = lerp( desaturateInitialColor12, desaturateDot12.xxx, 1.0 );
			float3 temp_cast_4 = (_Float0).xxx;
			float3 temp_cast_5 = (tex2DNode2.a).xxx;
			#ifdef _KEYWORD0_ON
				float3 staticSwitch16 = temp_cast_5;
			#else
				float3 staticSwitch16 = pow( desaturateVar12 , temp_cast_4 );
			#endif
			float4 appendResult51 = (float4(_Float6 , _Float7 , 0.0 , 0.0));
			float4 appendResult58 = (float4(0.0 , i.uv2_tex4coord2.z , 0.0 , 0.0));
			float2 uv_TexCoord29 = i.uv_texcoord + appendResult58.xy;
			float2 panner28 = ( 1.0 * _Time.y * appendResult51.xy + uv_TexCoord29);
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			o.Alpha = ( i.vertexColor.a * staticSwitch16 * pow( tex2D( _TextureSample1, panner28 ).r , _Float1 ) * tex2D( _TextureSample4, uv_TextureSample4 ).r ).x;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv2_tex4coord2;
				o.customPack2.xyzw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_tex4coord2 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
-2048;3.2;2048;1083;3040.082;435.7162;1;True;True
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-2637.873,141.6846;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;44;-2559.888,275.5306;Inherit;False;Property;_Vector3;扭曲图的流动速度;7;0;Create;False;0;0;False;0;0,0;1,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;46;-2033.535,216.2439;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-1585.059,398.268;Inherit;False;Property;_Float4;扭曲图强度;13;0;Create;False;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1680.046,-241.6513;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;41;-1728.392,188.2679;Inherit;True;Property;_TextureSample5;扭曲图;12;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;27;-1666.205,-111.5228;Inherit;False;Property;_Vector0;主图的流动速度;6;0;Create;False;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-1079.546,344.3697;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-966.1874,245.2206;Inherit;False;Constant;_Float5;Float 5;16;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;25;-1105.899,0.4591993;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1152.357,198.6946;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-973.467,745.0597;Inherit;False;Property;_Float7;Y_流动速度;15;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;58;-790.6316,319.4693;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-970.348,599.4603;Inherit;False;Property;_Float6;x_流动速度;14;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-883.7255,0.9344788;Inherit;False;2;2;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-571.1295,266.2697;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-685.9479,-27.75676;Inherit;True;Property;_TextureSample0;主题图;0;0;Create;False;0;0;False;0;-1;5f248743e14aec046b480c201ccf4137;dfb4b91533a4fa548a8692d466b36d08;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;51;-393.1473,450.7402;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-38.61079,32.56123;Inherit;False;Property;_Float0;主图对比度;2;0;Create;False;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;1;-796.9607,-400.2422;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;28;-67.67803,278.6096;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DesaturateOpNode;12;-284.3868,-117.1809;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;8;-47.16078,-343.5072;Inherit;False;Property;_Color0;主题图颜色;1;1;[HDR];Create;False;0;0;False;0;0,0,0,0;3.647799,0.3785447,0.3785447,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;248.5628,498.368;Inherit;False;Property;_Float1;遮罩对比度;5;0;Create;False;0;0;False;0;1;0.29;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;13;158.627,-114.7351;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;32;-60.64343,-782.95;Inherit;True;Property;_TextureSample3;多功能图;8;0;Create;False;0;0;False;0;-1;38e8ca1cfb88bc94f9a110cca804e21d;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;34;3.225244,-580.0242;Inherit;False;Property;_Float2;多功能图的强度;9;0;Create;False;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;141.7536,291.2664;Inherit;True;Property;_TextureSample1;遮罩图;4;0;Create;False;0;0;False;0;-1;9a356310dd85d6145a9f53edb5cd151d;645ed2b07caa05d44af57712f0aa8063;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-278.2381,-400.8535;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;391.6117,-595.4369;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;38;345.1807,645.3654;Inherit;True;Property;_TextureSample4;额外遮罩图;11;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;235.6189,-308.2234;Inherit;False;Property;_Float3;多功能图;10;0;Create;False;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;16;346.5699,43.61518;Inherit;True;Property;_Keyword0;是否有Alpha通道;3;0;Create;False;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;276.9446,-402.0958;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;18;469.9892,319.8686;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;646.4238,-400.7134;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;708.4084,-128.3529;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1010.878,-311.6959;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;texiao/base;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;45;0
WireConnection;46;2;44;0
WireConnection;41;1;46;0
WireConnection;25;0;26;0
WireConnection;25;2;27;0
WireConnection;43;0;41;0
WireConnection;43;1;42;0
WireConnection;58;0;57;0
WireConnection;58;1;56;3
WireConnection;40;0;25;0
WireConnection;40;1;43;0
WireConnection;29;1;58;0
WireConnection;2;1;40;0
WireConnection;51;0;52;0
WireConnection;51;1;53;0
WireConnection;28;0;29;0
WireConnection;28;2;51;0
WireConnection;12;0;2;0
WireConnection;13;0;12;0
WireConnection;13;1;15;0
WireConnection;17;1;28;0
WireConnection;5;0;1;0
WireConnection;5;1;2;0
WireConnection;35;0;32;0
WireConnection;35;1;34;0
WireConnection;16;1;13;0
WireConnection;16;0;2;4
WireConnection;9;0;5;0
WireConnection;9;1;8;0
WireConnection;18;0;17;1
WireConnection;18;1;19;0
WireConnection;36;0;9;0
WireConnection;36;1;35;0
WireConnection;36;2;37;0
WireConnection;10;0;1;4
WireConnection;10;1;16;0
WireConnection;10;2;18;0
WireConnection;10;3;38;1
WireConnection;0;2;36;0
WireConnection;0;9;10;0
ASEEND*/
//CHKSM=303BD82038F8E21D8E444AB2106C9363B97D2936