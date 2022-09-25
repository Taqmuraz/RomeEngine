using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaControllersParsingContext : ColladaParsingContext<ColladaControllersParsingContext, ColladaController>
    {
        ReadOnlyArrayList<ColladaRawMesh> previousStageMeshes;

        public ColladaControllersParsingContext(ReadOnlyArrayList<ColladaRawMesh> previousStageMeshes)
        {
            this.previousStageMeshes = previousStageMeshes;
        }

        public ColladaVisualSceneParsingContext CreateVisualSceneParsingContext()
        {
            return new ColladaVisualSceneParsingContext(Elements);
        }

        protected override IEnumerable<IColladaNodeHandler<ColladaControllersParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("controller", (context, node) => context.PushElement(new ColladaController(node.GetAttribute("id"), node.GetAttribute("name"))), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("skin", (context, node) => context.CurrentElement.PushElement(new ColladaSkin(node.GetAttribute("source"))), (context, node) => context.CurrentElement.PopElement());
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("Name_array", (context, node) => context.CurrentElement.CurrentElement.JointNames = node.GetValue(), null);
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("float_array", (context, node) =>
            {
                string name = node.GetAttribute("id").Replace($"{context.CurrentElement.Id}-", string.Empty);
                switch (name)
                {
                    case "bind_poses-array": context.CurrentElement.CurrentElement.BindingPositions = node.GetValue(); break;
                    case "weights-array": context.CurrentElement.CurrentElement.Weights = node.GetValue(); break;
                    default:return;
                }
            }, null);
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("vcount", (context, node) => context.CurrentElement.CurrentElement.VerticeWeightNumbers = node.GetValue(), null);
            yield return new ColladaDelegateHandler<ColladaControllersParsingContext>("v", (context, node) => context.CurrentElement.CurrentElement.JointWeightIndices = node.GetValue(), null);
        }

        protected override ColladaControllersParsingContext GetContext() => this;

        public override void FinalizeStage()
        {
            foreach (var controller in Elements)
            {
                foreach (var skin in controller.Elements)
                {
                    var rawMesh = previousStageMeshes.FirstOrDefault(m => m.Id.Equals(skin.SourceName.TrimStart('#')));
                    if (rawMesh != null)
                    {
                        AppendJointWeightIndices(rawMesh, skin);
                        AppendJointWeightBuffers(rawMesh, skin);
                        AppendJointsInfo(rawMesh, skin);
                    }
                }
            }
        }

        static void AppendJointWeightIndices(ColladaRawMesh mesh, ColladaSkin skin)
        {
            char separator = ' ';
            foreach (var submesh in mesh.TrianglesData.Elements)
            {
                string indices = submesh.Indices;
                string[] indicesArray = indices.Split(new char[] { separator }, System.StringSplitOptions.RemoveEmptyEntries);
                List<string> newIndicesList = new List<string>();
                int stride = mesh.Elements.Count;
                for (int i = 0; i < indicesArray.Length; i+=stride)
                {
                    string positionIndex = indicesArray[i];
                    newIndicesList.AddRange(indicesArray.Skip(i).Take(stride));
                    newIndicesList.Add(positionIndex);
                    newIndicesList.Add(positionIndex);
                }
                submesh.Indices = string.Join(separator.ToString(), newIndicesList);
            }
        }
        static void AppendJointWeightBuffers (ColladaRawMesh rawMesh, ColladaSkin skin)
        {
            char[] separators = new char[] { ' ' };
            int[] verticesJointNubers = skin.VerticeWeightNumbers.Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).ToArray();
            float[] weightsRawData = skin.Weights.Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToFloat()).ToArray();
            int[] jointWeightIndices = skin.JointWeightIndices.Split(separators, System.StringSplitOptions.RemoveEmptyEntries).Select(j => int.Parse(j)).ToArray();

            int jointPerVertex = 3;

            float[] weightsBuffer = new float[verticesJointNubers.Length * jointPerVertex];
            int[] jointIndices = new int[verticesJointNubers.Length * jointPerVertex];

            int bufferPosition = 0;

            for (int vertexIndex = 0; vertexIndex < verticesJointNubers.Length; vertexIndex++)
            {
                int joints = verticesJointNubers[vertexIndex];
                for (int jointIndex = 0; jointIndex < jointPerVertex; jointIndex++)
                {
                    int joint;
                    float weight;
                    if (jointIndex < joints)
                    {
                        joint = jointWeightIndices[bufferPosition];
                        weight = weightsRawData[jointWeightIndices[bufferPosition + 1]];
                        bufferPosition += 2;
                    }
                    else
                    {
                        joint = -1;
                        weight = 0f;
                    }
                    jointIndices[vertexIndex * jointPerVertex + jointIndex] = joint;
                    weightsBuffer[vertexIndex * jointPerVertex + jointIndex] = weight;
                }
            }

            var weightAttribute = new ColladaVertexAttribute("weights");
            weightAttribute.AddProperty("weight0", "float");
            weightAttribute.AddProperty("weight1", "float");
            weightAttribute.AddProperty("weight2", "float");
            var jointAttribute = new ColladaVertexAttribute("joints");
            jointAttribute.AddProperty("joint0", "int");
            jointAttribute.AddProperty("joint1", "int");
            jointAttribute.AddProperty("joint2", "int");

            /*for (int i = 0; i < weightsBuffer.Length; i += 3)
            {
                Vector3 vec = new Vector3(weightsBuffer[i], weightsBuffer[i + 1], weightsBuffer[i + 2]);
                float d = vec.x + vec.y + vec.z;
                if (d != 0) vec /= d;
                weightsBuffer[i] = vec.x;
                weightsBuffer[i + 1] = vec.y;
                weightsBuffer[i + 2] = vec.z;
            }*/

            rawMesh.PushElement(new ColladaVertexBuffer("weights")
            {
                Attribute = weightAttribute,
                Buffer = string.Join(" ", weightsBuffer)
            });
            rawMesh.PushElement(new ColladaVertexBuffer("joints")
            {
                Attribute = jointAttribute,
                Buffer = string.Join(" ", jointIndices)
            });
            rawMesh.PopElement();
            rawMesh.PopElement();
        }

        static void AppendJointsInfo(ColladaRawMesh rawMesh, ColladaSkin skin)
        {
            Matrix4x4[] matrices = skin.ReadJoints(out string[] joints);
            rawMesh.JointsInfo = Enumerable.Range(0, joints.Length).Select(i => new ColladaJointInfo(joints[i], i)).ToArray();
        }

        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {

        }

        public override string RootNodeName => "library_controllers";
    }
}
