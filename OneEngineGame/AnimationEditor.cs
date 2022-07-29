using System.Collections;
using System.IO;
using OneEngine;
using OneEngine.IO;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class AnimationEditor : Component
    {
        IEnumerator routine;

        IEnumerator Routine()
        {
            Canvas canvas = GameObject.AddComponent<Canvas>();
            FileSearchMenu fileSearch = new FileSearchMenu("./", "Select gameObject file");
            while (string.IsNullOrEmpty(fileSearch.File))
            {
                fileSearch.Draw(canvas);
                yield return null;
            }
            using (FileStream file = File.OpenRead(fileSearch.File))
            {
                GameObject skeletonRoot = (GameObject)new Serializer().Deserialize(new BinarySerializationStream(file));
            }
        }

        [BehaviourEvent]
        void Start()
        {
            routine = Routine();
        }
        [BehaviourEvent]
        void Update()
        {
            if (routine != null && !routine.MoveNext()) routine = null;
        }
    }
}