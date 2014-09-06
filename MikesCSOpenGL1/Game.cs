using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MikesCSOpenGL1
{
	public class Game : GameWindow
	{

		private List<Vector3[]> faces = new List<Vector3[]>();

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			Title = "Hello OpenTK!";
			GL.ClearColor (Color.CornflowerBlue);

			string[] lines = System.IO.File.ReadAllLines(@"cube.raw");
			foreach (var line in lines) {
				string[] points = line.Split(' ');
				faces.Add(new Vector3[3] {
					new Vector3(Convert.ToSingle(points[0]), Convert.ToSingle(points[1]), Convert.ToSingle(points[2])),
					new Vector3(Convert.ToSingle(points[3]), Convert.ToSingle(points[4]), Convert.ToSingle(points[5])),
					new Vector3(Convert.ToSingle(points[6]), Convert.ToSingle(points[7]), Convert.ToSingle(points[8]))
				});
			}
		}

		private float rotX = 0f;
		private float rotY = 0f;
		private float rotZ = 0f;
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
			OpenTK.Input.GamePadState state = OpenTK.Input.GamePad.GetState (0);
			//Console.WriteLine (state.ToString ());
			float x = state.ThumbSticks.Right.X;
			float y = state.ThumbSticks.Right.Y;
			rotX = 30 * x;
			rotY = 30 * y;
			rotZ = 30 * state.ThumbSticks.Left.X;
			Console.WriteLine ("rotX: "+rotX+", rotY:"+rotX);
		}

		private float z = 4.0f;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);

			GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// place camera
			Matrix4 modelview = Matrix4.LookAt (Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
			GL.MatrixMode (MatrixMode.Modelview);
			GL.LoadMatrix (ref modelview);
			GL.Rotate(rotX, 0f, -1f, 0f);
			GL.Rotate(rotY, -1f, 0f, 0f);
			GL.Rotate(rotZ, 0f, 0f, -1f);
			GL.Translate (rotX, rotY, rotZ);
			GL.Translate (0, 0, 7f);

			GL.Color3 (1.0f, 1.0f, 1.0f);

			// draw triangles
			foreach (var v3 in faces) {				
				GL.Begin (PrimitiveType.Triangles);
				GL.Vertex3 (v3[0]);
				GL.Vertex3 (v3[1]);
				GL.Vertex3 (v3[2]);
				GL.End ();
			}
			SwapBuffers ();
		}

		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
			GL.Viewport (ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView ((float)Math.PI / 4, Width / (float)Height, 1.0f, 64.0f);
			GL.MatrixMode (MatrixMode.Projection);
			GL.LoadMatrix (ref projection);
		}
	}
}
