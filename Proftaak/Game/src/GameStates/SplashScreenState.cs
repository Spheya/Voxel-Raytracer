﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Engine.Rendering;
using Game.Engine.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Game.Engine.Maths;
using System.Threading;

namespace Game.GameStates
{
    class SplashScreenState : ApplicationState
    {
        SpriteRenderer renderer;
        Sprite sprite1;
        Transform transform1;

        public override void OnCreate()
        {
            try
            {
                Shader spriteVertexShader = new Shader(ShaderType.VertexShader, ShaderPreprocessor.Execute(@"res\shaders\ui\vertex.glsl"));
                Shader spriteFragmentShader = new Shader(ShaderType.FragmentShader, ShaderPreprocessor.Execute(@"res\shaders\ui\fragment.glsl"));
                renderer = new SpriteRenderer(new ShaderProgram(new[] { spriteVertexShader, spriteFragmentShader }));
                Vector3 vector1 = new Vector3(20, 30, 30);
                transform1 = new Transform(vector1, Vector3.Zero, new Vector3(window.Width, window.Height, 30));
                Texture texture1 = new Texture("res\\textures\\mini_yoda.png");
                sprite1 = new Sprite(texture1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Colour Color2 = new Colour(0, 255, 0, 1f);
        }


        public override void OnDestroy()
        {
        }
        public override void OnDraw(float deltatime)
        {
            renderer.Draw(window);
        }

        public override void OnFixedUpdate(float deltatime)
        {
        }

        const float beginTime = 2.0f;
        const float speedFade = 5.0f;
        float count = 0;
        float fade = 1.0f;
        public override void OnUpdate(float deltatime)
        {
            count += deltatime;
            if (count >= beginTime) {
                fade -= deltatime / speedFade;
            }
            Colour Color1 = new Colour(fade, fade, fade);
            sprite1.Colour = Color1;
            renderer.Add(sprite1);
            sprite1.Transform = transform1;
;        }
    }
}