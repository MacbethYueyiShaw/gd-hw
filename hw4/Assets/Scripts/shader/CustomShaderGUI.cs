using UnityEngine;
using UnityEditor;
using System;
public class CustomShaderGUI : ShaderGUI
{
    MaterialEditor editor;
    MaterialProperty[] properties;
    Material target;
    enum SpecularChoice
    {
        True, False
    }
    enum RenderingMode
    {
        BlinnPhong = 0,
        NormalTexture = 1,
        Normal = 2
    }

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        this.editor = editor;
        this.properties = properties;
        target = editor.target as Material;
        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        GUIContent mainTexLabel = new GUIContent(mainTex.displayName);
        editor.TextureProperty(mainTex, mainTexLabel.text);

        MaterialProperty bumpMap = FindProperty("_BumpMap", properties);
        GUIContent bumpMapLabel = new GUIContent(bumpMap.displayName);
        editor.TextureProperty(bumpMap, bumpMapLabel.text);

        //enable specular
        SpecularChoice specularChoice = SpecularChoice.False;
        if (target.IsKeywordEnabled("USE_SPECULAR"))
            specularChoice = SpecularChoice.True;

        EditorGUI.BeginChangeCheck();
        specularChoice = (SpecularChoice)EditorGUILayout.EnumPopup(new GUIContent("Use Specular?"), specularChoice);
        if (EditorGUI.EndChangeCheck())
        {
            if (specularChoice == SpecularChoice.True)
                target.EnableKeyword("USE_SPECULAR");
            else
                target.DisableKeyword("USE_SPECULAR");
        }
        if(specularChoice == SpecularChoice.True){
            MaterialProperty gloss = FindProperty("_Gloss", properties);
            GUIContent glossLabel = new GUIContent(gloss.displayName);
            editor.FloatProperty(gloss, "Gloss");
        }

        //rendering mode
        RenderingMode renderingMode = RenderingMode.NormalTexture;
        if (target.IsKeywordEnabled("RENDERING_MODE_BLINN"))
        {
            if(target.IsKeywordEnabled("RENDERING_MODE_NORMAL"))
                renderingMode = RenderingMode.Normal;
            else renderingMode = RenderingMode.BlinnPhong;
        }
        else renderingMode = RenderingMode.NormalTexture;


        EditorGUI.BeginChangeCheck();
        renderingMode = (RenderingMode)EditorGUILayout.EnumPopup(new GUIContent("Rendering Mode:"), renderingMode);
        if (EditorGUI.EndChangeCheck())
        {
            if (renderingMode == RenderingMode.Normal)
            {
                target.EnableKeyword("RENDERING_MODE_BLINN");
                target.EnableKeyword("RENDERING_MODE_NORMAL");
            }
            else if(renderingMode == RenderingMode.BlinnPhong)
            {
                target.EnableKeyword("RENDERING_MODE_BLINN");
                target.DisableKeyword("RENDERING_MODE_NORMAL");
            }else if(renderingMode == RenderingMode.NormalTexture)
            {
                target.DisableKeyword("RENDERING_MODE_BLINN");
                target.DisableKeyword("RENDERING_MODE_NORMAL");
            }           
        }

        if (renderingMode == RenderingMode.NormalTexture)
        {
            MaterialProperty bumpScale = FindProperty("_BumpScale", properties);
            GUIContent bumpScaleLabel = new GUIContent(bumpScale.displayName);
            editor.FloatProperty(bumpScale, "BumpScale");

            MaterialProperty specular = FindProperty("_Specular", properties);
            GUIContent specularLabel = new GUIContent(specular.displayName);
            editor.ColorProperty(specular, "Specular");

            MaterialProperty color = FindProperty("_Color", properties);
            GUIContent colorLabel = new GUIContent(color.displayName);
            editor.ColorProperty(color, "Color");
        }
    }
}
