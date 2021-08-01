Shader "Custom/AlphaBlend"
{
	Properties
	{
		_Color("Main Tint", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
	//ʹ���µ�����_AlphaScale������ԭ�ȵ�_Cutoff���ԣ����Կ���͸�����������͸����
	_AlphaScale("Alpha Scale", Range(0, 1)) = 1
	}
		SubShader
	{
		//����������ʾ����Ҫע�⣬͸���Ȼ���õ�����Ⱦ���к�����Ӧ����Transparent
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
		//��pass�У�Ϊ͸���Ȼ�Ͻ��к��ʵĻ��״̬����
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			Cull Off//˫��
			ZWrite Off //�ر����д��
			Blend SrcAlpha OneMinusSrcAlpha //����Blend��ϣ�����Դ��ɫ��Ŀ����ɫ�������

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _AlphaScale;

			struct a2v {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};
			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}
			//�Ƴ�͸���Ȳ��ԵĴ��룬�����ø�ƬԪ��ɫ������ֵ�е�͸��ͨ��ֵΪ��������͸��ͨ����ֵ������ʲ���_AlphaScale�ĳ˻�
			fixed4 frag(v2f i) : SV_Target {
				fixed3 worldNormal = normalize(i.worldNormal);
				fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

				fixed4 texColor = tex2D(_MainTex, i.uv);

				//ʹ������ȥ������������ɫ
				fixed3 albedo = texColor.rgb * _Color.rgb;
				// ������
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
				//������
				fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir)); // ��ɫ�ں��ó˷�

				// ������ɫ = ������ + ������ + �߹ⷴ��
				return fixed4(diffuse + ambient, texColor.a * _AlphaScale);
			}
			ENDCG
		}
	}
		FallBack "Transparent/VertexLit"
}