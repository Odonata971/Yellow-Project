using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Architect : MonoBehaviour
    {
        DialogueSystem ds;
        TextArchitect architect;

        public TextArchitect.BuildMethod bm = TextArchitect.BuildMethod.instant;

        int currentLine = 0;

        string[] lines = new string[5] {
            "Flies all green and buzzin'",
            "In this dungeon of despair",
            "Prisoners grumblin",
            "Piss they clothes",
            "Scratch their matted hair"
        };

        // Start is called before the first frame update
        void Start()
        {
            ds = DialogueSystem.instance;
            architect = new TextArchitect(ds.dialogueContainer.dialogueText);
            architect.buildMethod = TextArchitect.BuildMethod.fade;
            architect.speed = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (bm != architect.buildMethod) {
                architect.buildMethod = bm;
                architect.Stop();
            }

            if (Input.GetKeyDown(KeyCode.S)) {
                architect.Stop();
            }

            string longLine = "Mon nom est Ozymandias, Roi des Rois. Voyez mon œuvre, ô puissants, et désespérez ! Auprès, rien ne demeure. Autour des ruines De cette colossale épave, infinis et nus, Les sables monotones et solitaires s’étendent au loin.";
            
            if (Input.GetKeyDown(KeyCode.Space)) {

                if (architect.isBuilding) {

                    if (!architect.hurryUp) {
                        architect.hurryUp = true;
                    } else {
                        architect.ForceComplete();
                    }

                } else {

/*                    architect.Build(lines[currentLine]);
                    currentLine++;

                    if (currentLine >= lines.Length) {
                        currentLine = 0;
                    }*/

                    architect.Build(longLine);
                    architect.speed = 0.5f;

                }

            } else if (Input.GetKeyDown(KeyCode.A)) {

                architect.Append(longLine);

/*                architect.Append(lines[currentLine]);
                currentLine++;

                if (currentLine >= lines.Length) {
                    currentLine = 0;
                }*/
            }
        }
    }
}
