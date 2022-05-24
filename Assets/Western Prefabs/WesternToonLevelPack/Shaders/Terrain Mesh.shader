// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:True,lprd:True,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:31457,y:32717|diff-556-OUT,normal-504-RGB;n:type:ShaderForge.SFN_Tex2d,id:2,x:33303,y:31920,ptlb:Diffuse Map,ptin:_DiffuseMap,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3,x:32894,y:33120,ptlb:Detail Map,ptin:_DetailMap,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:22,x:33410,y:32449,ptlb:Splat Map,ptin:_SplatMap,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:28,x:33633,y:32670,ptlb:Splat Mask,ptin:_SplatMask,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Lerp,id:31,x:32553,y:32481|A-2-RGB,B-22-RGB,T-428-OUT;n:type:ShaderForge.SFN_Blend,id:141,x:31861,y:32604,blmd:6,clmp:True|SRC-31-OUT,DST-440-OUT;n:type:ShaderForge.SFN_Multiply,id:242,x:32390,y:32677|A-400-OUT,B-3-RGB;n:type:ShaderForge.SFN_OneMinus,id:400,x:33015,y:32821|IN-492-OUT;n:type:ShaderForge.SFN_Blend,id:428,x:32941,y:32563,blmd:10,clmp:True|SRC-22-RGB,DST-492-OUT;n:type:ShaderForge.SFN_Multiply,id:440,x:32181,y:32792|A-242-OUT,B-442-OUT;n:type:ShaderForge.SFN_Slider,id:442,x:32380,y:33129,ptlb:Detail Power,ptin:_DetailPower,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Slider,id:485,x:33661,y:32892,ptlb:Splat Power,ptin:_SplatPower,min:1,cur:1,max:5;n:type:ShaderForge.SFN_Divide,id:492,x:33347,y:32680|A-28-RGB,B-485-OUT;n:type:ShaderForge.SFN_Tex2d,id:504,x:32023,y:33302,ptlb:Normal Map,ptin:_NormalMap,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Fresnel,id:553,x:32317,y:31966|EXP-575-OUT;n:type:ShaderForge.SFN_Multiply,id:554,x:32054,y:32116|A-553-OUT,B-555-RGB;n:type:ShaderForge.SFN_Color,id:555,x:32361,y:32148,ptlb:Rim Color,ptin:_RimColor,glob:False,c1:0.06271627,c2:0.8529412,c3:0.7548443,c4:1;n:type:ShaderForge.SFN_Add,id:556,x:31700,y:32385|A-554-OUT,B-141-OUT;n:type:ShaderForge.SFN_Slider,id:575,x:32620,y:31959,ptlb:Rim Power,ptin:_RimPower,min:0,cur:4.275383,max:10;proporder:2-3-442-22-485-28-504-555-575;pass:END;sub:END;*/

Shader "Dark Anvil/Terrain Mesh" {
    Properties {
        _DiffuseMap ("Diffuse Map", 2D) = "white" {}
        _DetailMap ("Detail Map", 2D) = "white" {}
        _DetailPower ("Detail Power", Range(0, 1)) = 1
        _SplatMap ("Splat Map", 2D) = "black" {}
        _SplatPower ("Splat Power", Range(1, 5)) = 1
        _SplatMask ("Splat Mask", 2D) = "black" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _RimColor ("Rim Color", Color) = (0.06271627,0.8529412,0.7548443,1)
        _RimPower ("Rim Power", Range(0, 10)) = 4.275383
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform sampler2D _DetailMap; uniform float4 _DetailMap_ST;
            uniform sampler2D _SplatMap; uniform float4 _SplatMap_ST;
            uniform sampler2D _SplatMask; uniform float4 _SplatMask_ST;
            uniform float _DetailPower;
            uniform float _SplatPower;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float4 _RimColor;
            uniform float _RimPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #else
                    float3 shLight : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                #ifdef LIGHTMAP_OFF
                    o.shLight = ShadeSH9(float4(mul(unity_ObjectToWorld, float4(v.normal,0)).xyz * 1.0,1)) * 0.5;
                #endif
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_621 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_621.rg, _NormalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, normalLocal));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(lmtex);
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                #ifndef LIGHTMAP_OFF
                    float3 diffuse = lightmap.rgb;
                #else
                    float3 diffuse = max( 0.0, NdotL) * attenColor;
                #endif
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                #ifdef LIGHTMAP_OFF
                    diffuseLight += i.shLight; // Per-Vertex Light Probes / Spherical harmonics
                #endif
                float4 node_22 = tex2D(_SplatMap,TRANSFORM_TEX(node_621.rg, _SplatMap));
                float3 node_492 = (tex2D(_SplatMask,TRANSFORM_TEX(node_621.rg, _SplatMask)).rgb/_SplatPower);
                finalColor += diffuseLight * ((pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPower)*_RimColor.rgb)+saturate((1.0-(1.0-lerp(tex2D(_DiffuseMap,TRANSFORM_TEX(node_621.rg, _DiffuseMap)).rgb,node_22.rgb,saturate(( node_492 > 0.5 ? (1.0-(1.0-2.0*(node_492-0.5))*(1.0-node_22.rgb)) : (2.0*node_492*node_22.rgb) ))))*(1.0-(((1.0 - node_492)*tex2D(_DetailMap,TRANSFORM_TEX(node_621.rg, _DetailMap)).rgb)*_DetailPower)))));
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _DiffuseMap; uniform float4 _DiffuseMap_ST;
            uniform sampler2D _DetailMap; uniform float4 _DetailMap_ST;
            uniform sampler2D _SplatMap; uniform float4 _SplatMap_ST;
            uniform sampler2D _SplatMask; uniform float4 _SplatMask_ST;
            uniform float _DetailPower;
            uniform float _SplatPower;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float4 _RimColor;
            uniform float _RimPower;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_622 = i.uv0;
                float3 normalLocal = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_622.rg, _NormalMap))).rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_22 = tex2D(_SplatMap,TRANSFORM_TEX(node_622.rg, _SplatMap));
                float3 node_492 = (tex2D(_SplatMask,TRANSFORM_TEX(node_622.rg, _SplatMask)).rgb/_SplatPower);
                finalColor += diffuseLight * ((pow(1.0-max(0,dot(normalDirection, viewDirection)),_RimPower)*_RimColor.rgb)+saturate((1.0-(1.0-lerp(tex2D(_DiffuseMap,TRANSFORM_TEX(node_622.rg, _DiffuseMap)).rgb,node_22.rgb,saturate(( node_492 > 0.5 ? (1.0-(1.0-2.0*(node_492-0.5))*(1.0-node_22.rgb)) : (2.0*node_492*node_22.rgb) ))))*(1.0-(((1.0 - node_492)*tex2D(_DetailMap,TRANSFORM_TEX(node_622.rg, _DetailMap)).rgb)*_DetailPower)))));
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
