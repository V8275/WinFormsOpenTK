namespace WinFormsOpenTK
{
    public class DbModel
    {
        private string presetName;
        private string modelPath;
        private string texturePath;
        private string normalPath;
        private string metallicPath;
        private string modelFormat;

        public string PresetName { get { return presetName; } }
        public string ModelPath { get { return modelPath; } }
        public string TexturePath { get { return texturePath; } }
        public string NormalMapPath { get { return normalPath; } }
        public string MetallicMapPath { get { return metallicPath; } }
        public string ModelFormat { get { return modelFormat; } }

        public DbModel() { }

        public void SetData((string?, string?, string?, string?, string?, string?) modelData)
        {
            presetName = modelData.Item1;
            modelPath = modelData.Item2;
            texturePath = modelData.Item3;
            modelFormat = modelData.Item4;
            normalPath = modelData.Item5;
            metallicPath = modelData.Item6;
        }
    }
}
