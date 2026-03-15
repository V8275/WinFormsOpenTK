namespace WinFormsOpenTK
{
    public class DbModel
    {
        private string presetName;
        private string modelPath;
        private string texturePath;
        private string modelFormat;

        public string PresetName { get { return presetName; } }
        public string ModelPath { get { return modelPath; } }
        public string TexturePath { get { return texturePath; } }
        public string ModelFormat { get { return modelFormat; } }

        public DbModel() { }

        public void SetData((string?, string?, string?, string?) modelData)
        {
            presetName = modelData.Item1;//String.IsNullOrEmpty(modelData.Item1) ? modelData.Item1 : "Error";
            modelPath = modelData.Item2; //String.IsNullOrEmpty(modelData.Item2) ? modelData.Item2 : "Error";
            texturePath = modelData.Item3; //String.IsNullOrEmpty(modelData.Item3) ? modelData.Item3 : "Error";
            modelFormat = modelData.Item4; //String.IsNullOrEmpty(modelData.Item3) ? modelData.Item3 : "Error";
        }
    }
}
