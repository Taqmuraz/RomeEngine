namespace RomeEngine
{
    public sealed class Layer
	{
		public string Name { get; }
		public int Index { get; }

		public static ReadOnlyArray<Layer> Layers { get; }
		public static Layer Default { get; }
		public static Layer Bone { get; }

		static Layer()
		{
			Layers = new Layer[] 
			{ 
				Default = new Layer("Default", 0),
				Bone = new Layer("Bone", 1),
			};
		}

        public Layer(string name, int index)
        {
            Name = name;
            Index = index;
        }
    }
}
