using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Winndos;
    using Data.Save;

    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(DSGraphView DSGraphView, Vector2 position)
        {
            base.Initialize(DSGraphView, position);
            DialogueType = DSDialogueType.MultipleChoice;
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "New Choice"
            };
            Choices.Add(choiceData);
        }


        public override void Draw()
        {
            base.Draw();

            /* MAIN CONTAINER */
            Button addChoiceButton = DSElementUtility.CreateButton("Add Choice", () =>
            {
                DSChoiceSaveData choiceData = new DSChoiceSaveData()
                {
                    Text = "New Choice"
                };
                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);
                outputContainer.Add(choicePort);
            });
            addChoiceButton.AddToClassList("ds-node_button");
            mainContainer.Insert(1, addChoiceButton);

            /* OUTPUT PORT */
            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }

        #region Elements  Creation

        private Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();
            //Link Data
            choicePort.userData = userData;
            DSChoiceSaveData choiceData = (DSChoiceSaveData)userData;

            //Create Button for Delete the Port and the Call Back Funtion
            Button deleteChoiceButton = DSElementUtility.CreateButton("X", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                //delete all edge
                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                //for Port Lit Remove the Port and use ID to Remove the Port form the Graph View
                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });
            deleteChoiceButton.AddToClassList("ds-node_button");

           
            //Create the Text
            TextField choiceTextField = DSElementUtility.CreateTextField(choiceData.Text,null,callback =>
            {
                choiceData.Text = callback.newValue;
            });
            choiceTextField.AddClasses
            (
                "ds-node_textfield",
                "ds-node_choice-textfield",
                "ds-node_textfield_hidden"
            );

            choiceTextField.style.flexDirection = FlexDirection.Column;

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);

            return choicePort;
        }

        #endregion
    }
}