using JeremyAnsel.Media.WavefrontObj;
using SharpGLTF.Schema2;
using System.Numerics;

namespace OpenTKProject
{
    public enum ModelFormat
    {
        Obj,
        Gltf
    }

    public class VisualModel
    {
        private string path;
        private ModelFormat format;
        public string PathToModel { get { return path; } }
        public ModelFormat Format { get { return format; } }

        private List<float> vertices = new List<float>();
        private List<uint> indices = new List<uint>();

        public VisualModel(string modelPath, ModelFormat modelFormat = ModelFormat.Obj)
        {
            path = modelPath;
            format = modelFormat;

            switch (format)
            {
                case ModelFormat.Obj:
                    LoadObjModel();
                    break;
                case ModelFormat.Gltf:
                    LoadGltfModel();
                    break;
            }
        }

        private void LoadObjModel()
        {
            var model = ObjFile.FromFile(path);

            foreach (var face in model.Faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    var position = model.Vertices[vertex.Vertex - 1].Position;
                    vertices.Add(position.X);
                    vertices.Add(position.Y);
                    vertices.Add(position.Z);

                    if (vertex.Texture > 0 && model.TextureVertices.Count > 0)
                    {
                        var texCoord = model.TextureVertices[vertex.Texture - 1];
                        vertices.Add(texCoord.X);
                        vertices.Add(texCoord.Y);
                    }
                    else
                    {
                        vertices.Add(0f);
                        vertices.Add(0f);
                    }

                    if (vertex.Normal > 0 && model.VertexNormals.Count > 0)
                    {
                        var normal = model.VertexNormals[vertex.Normal - 1];
                        vertices.Add(normal.X);
                        vertices.Add(normal.Y);
                        vertices.Add(normal.Z);
                    }
                    else
                    {
                        vertices.Add(0f);
                        vertices.Add(1f);
                        vertices.Add(0f);
                    }
                }
            }

            uint index = 0;
            foreach (ObjFace face in model.Faces)
            {
                for (int i = 0; i < face.Vertices.Count; i++)
                {
                    indices.Add(index++);
                }
            }
        }

        private void LoadGltfModel()
        {
            var model = ModelRoot.Load(path);

            uint vertexOffset = 0;

            foreach (var node in model.DefaultScene.VisualChildren)
            {
                if (node.Mesh == null) continue;

                var mesh = node.Mesh;
                var transform = node.WorldMatrix;

                foreach (var primitive in mesh.Primitives)
                {
                    var positionAccessor = primitive.GetVertexAccessor("POSITION");
                    var normalAccessor = primitive.GetVertexAccessor("NORMAL");
                    var texCoordAccessor = primitive.GetVertexAccessor("TEXCOORD_0");

                    if (positionAccessor == null) continue;

                    var positions = positionAccessor.AsVector3Array();

                    var normals = normalAccessor?.AsVector3Array();

                    var texCoords = texCoordAccessor?.AsVector2Array();

                    for (int i = 0; i < positions.Count; i++)
                    {
                        var worldPos = Vector3.Transform(positions[i], transform);
                        vertices.Add(worldPos.X);
                        vertices.Add(worldPos.Y);
                        vertices.Add(worldPos.Z);

                        if (texCoords != null && i < texCoords.Count)
                        {
                            vertices.Add(texCoords[i].X);
                            vertices.Add(texCoords[i].Y);
                        }
                        else
                        {
                            vertices.Add(0f);
                            vertices.Add(0f);
                        }

                        if (normals != null && i < normals.Count)
                        {
                            var worldNormal = Vector3.TransformNormal(normals[i], transform);
                            worldNormal = Vector3.Normalize(worldNormal);
                            vertices.Add(worldNormal.X);
                            vertices.Add(worldNormal.Y);
                            vertices.Add(worldNormal.Z);
                        }
                        else
                        {
                            vertices.Add(0f);
                            vertices.Add(1f);
                            vertices.Add(0f);
                        }
                    }

                    var indicesAccessor = primitive.GetIndexAccessor();
                    if (indicesAccessor != null)
                    {
                        var indicesArray = indicesAccessor.AsIndicesArray();
                        foreach (var idx in indicesArray)
                        {
                            indices.Add(vertexOffset + (uint)idx);
                        }
                    }

                    vertexOffset += (uint)positions.Count;
                }
            }
        }

        public List<float> Vertices { get { return vertices; } }
        public List<uint> Indices { get { return indices; } }
    }
}