using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    using Data.Save;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView DSGraphView, Vector2 position)
        {
            base.Initialize(DSGraphView,position);
            DialogueType = DSDialogueType.SingleChoice;
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Next Dialogue"
            };
            Choices.Add(choiceData);
        }

        /* OUTPUT PORT */
        public override void Draw()
        {
            base.Draw();
            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);
                choicePort.userData = choice;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}

