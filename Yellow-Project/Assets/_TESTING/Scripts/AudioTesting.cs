using CHARACTERS;
using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING {

    public class AudioTesting : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }

        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        IEnumerator Running2() {

            Character_Sprite FS = CreateCharacter("FS as Female Student 2") as Character_Sprite;
            Character Me = CreateCharacter("Me");
            FS.Show();

            AudioManager.instance.PlaySoundEffect("Audio/SFX/RadioStatic", loop: true);

            yield return Me.Say("Please turn off the radio.");

            AudioManager.instance.StopSoundEffect("RadioStatic");
            AudioManager.instance.PlayVoice("Audio/Voices/wakeup");

            FS.Say("Okay!");
        }

        IEnumerator Running() {

            yield return new WaitForSeconds(1);

            Character_Sprite FS = CreateCharacter("FS as Female Student 2") as Character_Sprite;
            Character Me = CreateCharacter("Me");
            FS.Show();

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/villagenight");

            AudioManager.instance.PlayTrack("Audio/Ambience/RainyMood", 0);
            AudioManager.instance.PlayTrack("Audio/Music/Calm", 1, pitch: 0.7f);


            yield return FS.Say("We can have multiple channels for playing ambiance as well as music!");

            AudioManager.instance.StopTrack(1);

            yield return null;
        }
    }
}
