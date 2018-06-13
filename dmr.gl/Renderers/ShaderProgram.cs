using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmr.gl.Renderers
{
    class ShaderProgram : IDisposable
    {
        private readonly int ID, VertexShaderID, FragmentShaderID;

        public ShaderProgram(string vertexcode, string fragmentcode, params (int loc, string name)[] attribs)
        {
            // vertex shader
            VertexShaderID = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexShaderID, vertexcode);
            GL.CompileShader(VertexShaderID);
            GL.GetShader(VertexShaderID, ShaderParameter.CompileStatus, out int compiled);
            if (compiled == 0)
                throw new InvalidOperationException(GL.GetShaderInfoLog(VertexShaderID));

            // fragment shader
            FragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentShaderID, fragmentcode);
            GL.CompileShader(FragmentShaderID);
            GL.GetShader(FragmentShaderID, ShaderParameter.CompileStatus, out compiled);
            if (compiled == 0)
                throw new InvalidOperationException(GL.GetShaderInfoLog(FragmentShaderID));

            // program
            ID = GL.CreateProgram();
            GL.AttachShader(ID, VertexShaderID);
            GL.AttachShader(ID, FragmentShaderID);

            foreach (var (loc, name) in attribs)
                GL.BindAttribLocation(ID, loc, name);

            GL.LinkProgram(ID);
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out compiled);
            if (compiled == 0)
                throw new InvalidOperationException(GL.GetProgramInfoLog(ID));
        }

        public void Use() => GL.UseProgram(ID);

        public int GetUniformLocation(string name) => GL.GetUniformLocation(ID, name);

        public int GetAttribLocation(string name) => GL.GetAttribLocation(ID, name);

        public void ProgramUniform(string name, ref Matrix4 mat, bool transpose) =>
            ProgramUniform(GetUniformLocation(name), ref mat, transpose);

        public void ProgramUniform(string name, double val) =>
            ProgramUniform(GetUniformLocation(name), val);

        public void ProgramUniform(int loc, ref Matrix4 mat, bool transpose) =>
            GL.ProgramUniformMatrix4(ID, loc, transpose, ref mat);

        public void ProgramUniform(int loc, double val) =>
            GL.ProgramUniform1(ID, loc, val);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // managed state 
                }

                GL.DetachShader(ID, VertexShaderID);
                GL.DetachShader(ID, FragmentShaderID);
                GL.DeleteProgram(ID);
                GL.DeleteShader(VertexShaderID);
                GL.DeleteShader(FragmentShaderID);

                disposedValue = true;
            }
        }

        ~ShaderProgram()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
