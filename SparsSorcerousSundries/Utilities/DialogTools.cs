using SparsSorcerousSundries.Utilities;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Localization;
using static SparsSorcerousSundries.Main;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using BlueprintCore.Conditions.Builder;
using Kingmaker.DialogSystem;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils;

namespace SparsSorcerousSundries
{
    public class DialogTools
    {
        public static readonly Kingmaker.ElementsSystem.ActionList EmptyActionList =
                                new Kingmaker.ElementsSystem.ActionList();
        public static readonly Kingmaker.DialogSystem.CueSelection EmptyCueSelection =
                                    new Kingmaker.DialogSystem.CueSelection();
        public static readonly Kingmaker.UnitLogic.Alignments.AlignmentShift EmptyAlignmentShift =
                                    new Kingmaker.UnitLogic.Alignments.AlignmentShift();
        public static readonly Kingmaker.ElementsSystem.ConditionsChecker EmptyConditionChecker =
                                    new Kingmaker.ElementsSystem.ConditionsChecker();
        public static readonly Kingmaker.DialogSystem.DialogSpeaker EmptyDialogSpeaker =
                                    new Kingmaker.DialogSystem.DialogSpeaker();
        public static readonly Kingmaker.DialogSystem.Blueprints.ShowCheck EmptyShowCheck =
                                    new Kingmaker.DialogSystem.Blueprints.ShowCheck();
        public static readonly Kingmaker.DialogSystem.CharacterSelection EmptyCharSelect =
                                    new Kingmaker.DialogSystem.CharacterSelection();
        public static BlueprintDialog CreateDialog(string name, string key, BlueprintCue firstcue)
        {
            var result = DialogConfigurator.New(name, key)
                    .SetFirstCue(firstCue: new()
                    {
                        Cues = new List<BlueprintCueBaseReference>() { firstcue.ToReference<BlueprintCueBaseReference>() },
                        Strategy = 0
                    })
                    .SetConditions(ConditionsBuilder.New())
                    .SetStartActions(ActionsBuilder.New())
                    .SetFinishActions(ActionsBuilder.New())
                    .SetReplaceActions(ActionsBuilder.New())
                   .SetTurnFirstSpeaker(true)
                   .SetTurnPlayer(true)
                   .Configure();
            return result;
        }

        public static CueConfigurator CreateCue(string name, string guid, string text, DialogSpeaker speaker = null)
        {
            var cue = CueConfigurator.New(name, guid)
                    .SetText(Helpers.CreateString(SSSContext, name + "text", text))
                    .SetTurnSpeaker(true)
                    .SetConditions(conditions: ConditionsBuilder.New())
                    .SetAlignmentShift(EmptyAlignmentShift)
                    .SetContinueValue(EmptyCueSelection)
                    .SetOnShow(ActionsBuilder.New())
                    .SetOnStop(ActionsBuilder.New());


            if (speaker != null)
            {
                cue.SetSpeaker(speaker);
            }
            else
            {
                cue.SetSpeaker(EmptyDialogSpeaker);
            }


            return cue;
        }
        public static CueConfigurator CueAddContinue(CueConfigurator cue, BlueprintCue nextcue = null, int pos = -1)
        {
            if (nextcue != null)
            {
                cue.ModifyContinueValue(action: init =>
                {
                    init.Cues.Add(BlueprintTool.GetRef<BlueprintCueBaseReference>(nextcue.AssetGuidThreadSafe));
                });
            }
            else
            {
                cue.SetContinueValue(new CueSelection()
                {
                    Strategy = Strategy.First,
                    Cues = new List<BlueprintCueBaseReference>()
                    {
                        //empty so Awnlist works
                    }
                });
            }
            return cue;
        }
        public static CueConfigurator CueAddAnswerList(CueConfigurator cue, BlueprintAnswersList nextawn = null)
        {
            if (nextawn != null)
            {
                cue.SetAnswers(new Blueprint<BlueprintAnswerBaseReference>[]
                     {
                    BlueprintTool.GetRef<BlueprintAnswerBaseReference>(nextawn.AssetGuidThreadSafe)
                     });
            }
            return cue;
        }

        public static AnswerConfigurator CreateAnswer(string name, string key, string text)
        {
            return AnswerConfigurator.New(name, key)
                        .SetText(Helpers.CreateString(SSSContext, name + "text", text))
                        .SetNextCue(new())
                        .SetAlignmentShift(EmptyAlignmentShift)
                        .SetShowConditions(ConditionsBuilder.New())
                        .SetSelectConditions(ConditionsBuilder.New())
                        .SetCharacterSelection(EmptyCharSelect)
                        .SetShowCheck(EmptyShowCheck)
                        .SetShowOnce(false)
                        .SetOnSelect(ActionsBuilder.New());

        }
        public static AnswerConfigurator AnswerAddNextCue(AnswerConfigurator awns, BlueprintCue nextcue)
        {

            awns.ModifyNextCue(action: init =>
            {
                init.Cues.Add(BlueprintTool.GetRef<BlueprintCueBaseReference>(nextcue.AssetGuidThreadSafe));
            });
            return awns;
        }


        public static AnswersListConfigurator CreateAnswerList(string name, string key)
        {
            return AnswersListConfigurator.New(name, key)
                .SetShowOnce(false)
                .SetAlignmentRequirement(Kingmaker.Enums.AlignmentComponent.None)
                .SetMythicRequirement(Mythic.None)
                .SetConditions(conditions: ConditionsBuilder.New());
        }


        public static BlueprintDialog CreateDialog(string name, Action<BlueprintDialog> init)
        {
            return Helpers.CreateBlueprint<BlueprintDialog>(SSSContext, name, dialog =>
            {
                dialog.Conditions = new();
                dialog.StartActions = new();
                dialog.FinishActions = new();
                dialog.ReplaceActions = new();

                dialog.FirstCue = new();
                dialog.FirstCue.Cues = new();

                init(dialog);
            });
        }
    }
}


