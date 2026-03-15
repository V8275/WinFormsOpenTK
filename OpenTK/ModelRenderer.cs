using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenTKProject
{
    public class ModelRenderer
    {
        private readonly Dictionary<Model, (int vao, int vbo, int ebo)> _modelBuffers;

        public ModelRenderer()
        {
            _modelBuffers = new Dictionary<Model, (int, int, int)>();
        }

        public void RenderModel(SceneObject sceneObj, CameraController camera, Light light, 
                               int shadowMapTexture, Vector2i windowSize)
        {
            EnsureBuffersExist(sceneObj.Model);

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f),
                (float)windowSize.X / (float)windowSize.Y, 0.1f, 100.0f);

            Matrix4 modelMatrix = sceneObj.GetModelMatrix();
            Matrix4 lightSpaceMatrix = light.GetLightSpaceMatrix();

            sceneObj.Model.Shader.Use();

            // Setup textures
            SetupTextures(sceneObj.Model, shadowMapTexture);

            // Setup matrices
            sceneObj.Model.Shader.SetMatrix4("model", modelMatrix);
            sceneObj.Model.Shader.SetMatrix4("view", view);
            sceneObj.Model.Shader.SetMatrix4("projection", projection);
            sceneObj.Model.Shader.SetMatrix4("lightSpaceMatrix", lightSpaceMatrix);

            // Setup lighting
            SetupLighting(sceneObj.Model.Shader, camera, light);

            // Draw
            var buffers = _modelBuffers[sceneObj.Model];
            GL.BindVertexArray(buffers.vao);
            GL.DrawElements(PrimitiveType.Triangles, sceneObj.Model.VModel.Indices.Count,
                           DrawElementsType.UnsignedInt, 0);

            GL.BindVertexArray(0);
        }

        private void SetupTextures(Model model, int shadowMapTexture)
        {
            if (model.Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, model.Texture.Handle);
                model.Shader.SetInt("texture0", 0);
            }

            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, shadowMapTexture);
            model.Shader.SetInt("shadowMap", 1);
        }

        private void SetupLighting(Shader shader, CameraController camera, Light light)
        {
            shader.SetVector3("viewPos", camera.Position);
            shader.SetVector3("material.ambient", light.ColorToVec3());
            shader.SetVector3("material.diffuse", new Vector3(1.0f, 0.5f, 0.31f));
            shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetFloat("material.shininess", 32.0f);

            shader.SetVector3("light.ambient", light.Ambient);
            shader.SetVector3("light.diffuse", light.Diffuse);
            shader.SetVector3("light.specular", light.Specular);
            shader.SetVector3("light.position", light.Position);
        }

        private void EnsureBuffersExist(Model model)
        {
            if (!_modelBuffers.ContainsKey(model))
            {
                SetupModelBuffers(model);
            }
        }

        private void SetupModelBuffers(Model model)
        {
            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();
            int ebo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

            SetupModelData(model, vbo, ebo);
            _modelBuffers[model] = (vao, vbo, ebo);
        }

        private void SetupModelData(Model model, int vbo, int ebo)
        {
            int stride = 8;
            int uvOffset = 3 * sizeof(float);
            int normalOffset = 5 * sizeof(float);
            int strideSize = stride * sizeof(float);

            GL.BufferData(BufferTarget.ArrayBuffer, model.VModel.Vertices.Count() * sizeof(float),
                          model.VModel.Vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BufferData(BufferTarget.ElementArrayBuffer, model.VModel.Indices.Count() * sizeof(uint),
                          model.VModel.Indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, strideSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, strideSize, uvOffset);

            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, strideSize, normalOffset);
        }

        public void Dispose()
        {
            foreach (var buffers in _modelBuffers.Values)
            {
                GL.DeleteBuffer(buffers.vbo);
                GL.DeleteVertexArray(buffers.vao);
                GL.DeleteBuffer(buffers.ebo);
            }
        }
    }
}