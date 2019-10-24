using System;
using System.Collections.Generic;
using System.Text;

using Tao.FreeGlut;
using Tao.OpenGl;

namespace WavParser2
{
    public class GlWindow
    {
        public Action InitGraphics { get; private set; }
        public Action Render { get; private set; }
        public Action<int, int> Reshape { get; private set; }

        public int FPS { get; private set; }

        public GlWindow(int width, int height, int posX, int posY, string name, Action initGraphics, Action render, Action<int, int> reshape, int FPS)
        {
            InitGraphics = initGraphics;
            Render = render;
            Reshape = reshape;
            this.FPS = FPS;

            InitGlSetting(width, height, posX, posY, name, FPS);
        }

        public void InitGlSetting(int width, int height, int posX, int posY, string name, int FPS)
        {
            Glut.glutInit();
            Glut.glutInitWindowSize(width, height);//setting window size
            Glut.glutInitWindowPosition(posX, posY);//setting window position
            Glut.glutCreateWindow(name);//creating a window with some name
            Gl.glEnable(Gl.GL_TEXTURE_2D);

            this.InitGraphSetting();

            Glut.glutDisplayFunc(Redraw);//rendering method setting
            Glut.glutTimerFunc(1000 / FPS, Timer, 0);//SET SPEED OF RENDERING HERE
            Glut.glutReshapeFunc(WindowReshape);//on window reshape method setting
        }

        public void InitGraphSetting()
        {
            Gl.glLoadIdentity();
            InitGraphics();
        }

        public void Start()
        {
            Glut.glutMainLoop();
        }

        public void Redraw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            Glu.gluLookAt(0, 0, 1, 0, 0, 0, 0, 1, 0);//setting looker

            Render();

            Glut.glutSwapBuffers();
        }

        public void Timer(int time)
        {
            Gl.glLoadIdentity();
            Glut.glutPostRedisplay();
            Glut.glutTimerFunc(1000 / FPS, Timer, 0);
        }

        public void WindowReshape(int width, int height)
        {
            int size = Math.Min(width, height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Gl.glViewport(0, 0, size, size);
            Glu.gluPerspective((360 * Math.Atan(size / 2) / Math.PI) % 90, 1, 1, 100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

            Reshape(width, height);
        }
    }
}
