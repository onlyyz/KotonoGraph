namespace Kotono.Code
{
    [System.Serializable]
    public class CodeGraphConnection
    {
        public CodeGraphConnectionPort inputPort;
        public CodeGraphConnectionPort outputPort;

        public CodeGraphConnection(CodeGraphConnectionPort input, CodeGraphConnectionPort output)
        {
            this.inputPort = input;
            this.outputPort = output;
        }

        public CodeGraphConnection(string inputPortID, int inputIndex, string outputPortID, int outputIndex)
        {
            this.inputPort = new CodeGraphConnectionPort(inputPortID, inputIndex);
            this.outputPort = new CodeGraphConnectionPort(outputPortID, outputIndex);
        }
    }
    [System.Serializable]
    public struct CodeGraphConnectionPort
    {
        public string nodeID;
        public int portIndex;

        public CodeGraphConnectionPort(string nodeID, int portIndex)
        {
            this.nodeID = nodeID;
            this.portIndex = portIndex;
        }
    }
}
