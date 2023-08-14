using CHARACTERS;
using COMMANDS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE {

    public class ConversationManager {

        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect) {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next() {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<string> conversation) {

            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void StopConversation() {

            if (!isRunning) {
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation) {
            for (int i = 0; i < conversation.Count; i++) {

                // Don't show any black lines or try to run any logic on them
                if (string.IsNullOrWhiteSpace(conversation[i])) {
                    continue;
                }

                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);

                // Show dialogueData
                if (line.hasDialogue) {
                    yield return Line_RunDialogue(line);
                }

                // Run any commands
                if (line.hasCommands) {
                    yield return Line_RunCommands(line);
                }

                if (line.hasDialogue) {
                    // Wait for user input
                    yield return WaitForUserInput();

                    CommandManager.instance.StopAllProcesses();
                }
            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line) {

            // Show or hide the spaker name if there is one present.
            if (line.hasSpeaker) {
                HandleSpeakerLogic(line.speakerData);                
            }

            // Build dialogue
            yield return BuildLineSegments(line.dialogueData);
        }

        private void HandleSpeakerLogic(DL_SPEAKER_DATA speakerData) {

            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, createIfDoesNotExist: characterMustBeCreated);

            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing)) {
                character.Show();
            }

            // Add character name to the UI
            dialogueSystem.ShowSpeakerName(speakerData.displayname);

            // Now customize the dialogue for this character - if applicable
            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            // Cast position
            if (speakerData.isCastingPosition) {
                character.MoveToPosition(speakerData.castPosition);
            }

            // Cast Expression
            if (speakerData.isCastingExpressions) {
                foreach (var ce in speakerData.CastExpressions) {
                    character.OnReceiveCastingExpression(ce.layer, ce.expression);
                }
            }
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line) {

            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;

            foreach(DL_COMMAND_DATA.Command command in commands) {

                if (command.waitForCompletion || command.name == "wait") {

                    CoroutineWrapper cw = CommandManager.instance.Execute(command.name, command.arguments);
                    while(!cw.IsDone) {

                        if (userPrompt) {
                            CommandManager.instance.StopCurrentProcess();
                            userPrompt = false;
                        }

                        yield return null;
                    }

                } else {
                    CommandManager.instance.Execute(command.name, command.arguments);
                }
            }

            yield return null;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line) {
            for (int i = 0; i < line.segments.Count; i++) {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment) {

            switch(segment.startSignal) {

                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;

                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;

                default: 
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false) {

            // Build the dialogueData
            if (!append) {
                architect.Build(dialogue);
            } else {
                architect.Append(dialogue);
            }

            // Wait for the dialogueData to complete.
            while (architect.isBuilding) {

                if (userPrompt) {

                    if (!architect.hurryUp) {
                        architect.hurryUp = true;
                    } else {
                        architect.ForceComplete();
                    }

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput() {
            while (!userPrompt) {
                yield return null;
            }

            userPrompt = false;
        }
    }
}
