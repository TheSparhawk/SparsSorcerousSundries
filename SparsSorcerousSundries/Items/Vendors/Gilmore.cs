using BlueprintCore.Blueprints.Configurators.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.Blueprints;
using BlueprintCore.Utils;
using static SparsSorcerousSundries.Main;
using SparsSorcerousSundries.Utilities;
using BlueprintCore.Actions.Builder;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Localization;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Utility;
using Kingmaker.Visual.Sound;
using BlueprintCore.Blueprints.Configurators.DialogSystem;
using Kingmaker.DialogSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using BlueprintCore.Conditions.Builder;
using Kingmaker.UnitLogic.Interaction;
using Kingmaker.Blueprints.Area;
using UnityEngine;
using Kingmaker.PubSubSystem;

namespace SparsSorcerousSundries.Items
{
    static class Gilmore 
    {
        public static string Guid { get; } = "E3D22BFA-8C88-4A1E-9C06-7AD3096DFCFC";
        //Future Gilmores will be cloned here.
        public static BlueprintUnit GilmoreVendorBlueprint; //= BlueprintTool.Get<BlueprintUnit>("bf2d84c1de8ddd043b41a83add8afc33");
        public static BlueprintDialog GilmoreDialog;
        public static bool IsSpawned = false;

        public static StartTrade StartVending = new StartTrade();

        public static void GetGilmoreData()
        {

        }
        public static void CreateGilmore()
        {
             var gilmoreVendorList = SharedVendorTableConfigurator.New("GilmoresVendorList", "712c04b716fe41ef962139cd8394b62f")
                 .Configure();
            // var gilmoreDialogCue = CueConfigurator.New("", "").Configure();
            // //public static BlueprintDialog ServiceDialog;
            // var gilmoreDialog = DialogConfigurator.New("gilmore-service-dialog", "CBF8405C-B71D-47C4-9B51-B43C6AD925F6")
            //     .SetFirstCue(firstCue: new CueSelection()
            //     {
            //         Strategy = 0,
            //         Cues = new List<BlueprintCueBaseReference>()
            //         {
            //             //DialogTools.CreateCue(SSSContext,"","","",GilmoreVendorBlueprint).ToReference<BlueprintCueBaseReference>(),
            //             CueConfigurator.New("","")
            //             //.SetText("")
            //             //.ModifySpeaker()
            //             .Configure().ToReference<BlueprintCueBaseReference>()
            //         }
            //
            //     })
            //     .ModifyFirstCue(action: )
            //     .Configure();

            //dialog awnsers root
            var awn_shop = DialogTools.CreateAnswer("gil_shop_awns", "5a7edac7-8322-4756-a876-680a7f241d0d", "What are these exclusive goods, then?")
                        .SetOnSelect(ActionsBuilder.New().Add<StartTrade>(init =>
                        {
                            init.Vendor = new DialogFirstSpeaker();
                        })).Configure();



            var awn_exit = DialogTools.CreateAnswer("gil_shop_leave", "ddab5e20-3219-4cf7-aa12-7fdce5160393", "Be seeing ya");
            var awn_exit_cue = DialogTools.CreateCue("gil_exit_cue", "43469cf7-c655-4fdf-ae36-0ff6eac12074",
                "Be seeing ya")
                .SetContinueValue(new CueSelection()
                {
                    Strategy = Strategy.First,
                    Cues = new List<BlueprintCueBaseReference>()
                    {
                        //empty so Awnlist works
                    }
                })
                .Configure();
            awn_exit = DialogTools.AnswerAddNextCue(awn_exit, awn_exit_cue);
                        


            //dialog awnsers list root
            var awnlist_root = DialogTools.CreateAnswerList("gil_answerlist_root", "5802c241-bc4f-470d-9e94-ac19074b828b")
                .SetAnswers(answers: new Blueprint<BlueprintAnswerBaseReference>[]
                {
                    BlueprintTool.GetRef<BlueprintAnswerBaseReference>(awn_shop.AssetGuidThreadSafe),
                    BlueprintTool.GetRef<BlueprintAnswerBaseReference>(awn_exit.Configure().AssetGuidThreadSafe)
                }).Configure();

            //dialog root
            var cue_root = DialogTools.CreateCue("gil_root_cue", "3ae5ed0d-ba12-43e6-a8b7-ddd3e030bb6d", "Can I help you?");
            cue_root = DialogTools.CueAddAnswerList(cue_root, awnlist_root);
            cue_root = DialogTools.CueAddContinue(cue_root);
                
            var cue_root_done =  cue_root.Configure();

            //dialog greeting

            var cue_greeting_first_time = DialogTools.CreateCue("gil_first_time_cue", "df23645b-6921-4f35-933f-8060e5b0736b",
                "placeholder gilmore first time meeting text")
                .SetShowOnce(true)
                .SetSpeaker(new DialogSpeaker() { MoveCamera = true, NoSpeaker = false, SwitchDual = false });
            cue_greeting_first_time = DialogTools.CueAddContinue(cue_greeting_first_time, cue_root_done);
            var cue_greeting_first_time_done = cue_greeting_first_time.Configure();

            // var cue_greeting_first_time = CueConfigurator.New("gil_first_time_cue", "df23645b-6921-4f35-933f-8060e5b0736b")
            //      
            //     .SetText(Helpers.CreateString(SSSContext, "gil_first_time_cue_text", "peepeepoopoo"))
            //     .SetTurnSpeaker(true)
            //     .SetConditions(conditions:ConditionsBuilder.New())
            //     
            //     .SetShowOnce(true)
            //     
            //     .SetShowOnceCurrentDialog(false)
            //     .SetSpeaker(new DialogSpeaker() { MoveCamera = true, NoSpeaker = false, SwitchDual= false})
            //     .SetAnimation(DialogAnimation.None)
            //     
            //     .SetContinueValue(new CueSelection()
            //     {
            //         Strategy = Strategy.First,
            //         Cues = new List<BlueprintCueBaseReference>()
            //         {
            //             BlueprintTool.GetRef<BlueprintCueBaseReference>("3ae5ed0d-ba12-43e6-a8b7-ddd3e030bb6d")//root goes here
            //         }
            //     })
            //     .SetExperience(DialogExperience.NoExperience)
            //     .SetOnShow(ActionsBuilder.New())
            //     .SetOnStop(ActionsBuilder.New())
            //     .SetAlignmentShift(new() {Value=0,Direction=Kingmaker.UnitLogic.Alignments.AlignmentShiftDirection.TrueNeutral})
            //     .Configure();

            var cue_greeting_normal = DialogTools.CreateCue("gil_normal_greeting_cue", "e11259b7-9955-43a9-b200-ade2557451d7",
                 "Hello, you've come again? Not just to look at me I hope?");
            cue_greeting_normal = DialogTools.CueAddContinue(cue_greeting_normal);
            cue_greeting_normal = DialogTools.CueAddAnswerList(cue_greeting_normal, awnlist_root);
            var cue_greeting_normal_conf = cue_greeting_normal.Configure();

            //Dialog 
            GilmoreDialog = DialogConfigurator.New("gilmore-service-dialog", "CBF8405C-B71D-47C4-9B51-B43C6AD925F6")
                .SetFirstCue(firstCue: new()
                {
                    Strategy=0,
                    Cues = new List<BlueprintCueBaseReference>()
                    {
                        BlueprintTool.GetRef<BlueprintCueBaseReference>(cue_greeting_first_time_done.AssetGuidThreadSafe),//root goes here
                        BlueprintTool.GetRef<BlueprintCueBaseReference>(cue_greeting_normal_conf.AssetGuidThreadSafe),//root goes here
                        //BlueprintTool.GetRef<BlueprintCueBaseReference>("1677541e2c1bd1b42a6d815a2a30bb79")
                    }
                })
                .Configure();

















               //var gilmoreAwnsers = AnswersListConfigurator.New("gilmore-awnserlist-root", "8a22a68e-1d6f-4fad-83f7-7678322ea58d")
               //    .SetShowOnce(false)
               //    
               //    .SetConditions(ConditionsBuilder.New())
               //    .SetAnswers(answers: new()
               //    {
               //        AnswerConfigurator.New("gils-shop-awns", "9e4a9ff2-5a77-4bdd-9dfd-f2120c231436")                
               //        .SetText(Helpers.CreateString(SSSContext, "gils-shop-awns.text", "What are these exclusive goods you have for sale?"))
               //        .SetNextCue(new())
               //        .SetAlignmentShift(new())
               //        .SetShowConditions(ConditionsBuilder.New())
               //        .SetSelectConditions(ConditionsBuilder.New())
               //        .SetCharacterSelection(new())
               //        .SetShowCheck(new())
               //        .SetShowOnce(false)
               //        .SetOnSelect(ActionsBuilder.New().Add<StartTrade>(init =>
               //        {
               //            init.Vendor = new DialogFirstSpeaker();
               //        })).Configure(),
               //        AnswerConfigurator.New("gils-shop-exit", "e0f99279-b6fc-45d6-8660-30cd0bc21318")
               //        .SetText(Helpers.CreateString(SSSContext, "gils-shop-exit.text", "I'll see you later"))
               //        .SetAlignmentShift(new())
               //        .SetShowConditions(ConditionsBuilder.New())
               //        .SetSelectConditions(ConditionsBuilder.New())
               //        .SetCharacterSelection(new())
               //        .SetShowCheck(new())
               //        .SetShowOnce(false)
               //        .SetOnSelect(ActionsBuilder.New())
               //        .SetNextCue(nextCue: new()
               //        {
               //            Cues = new List<BlueprintCueBaseReference>()
               //            {
               //                //CueConfigurator.New("gilmore-exit","f28a3502-715e-4e7e-979d-47e19693c26f").Configure().ToReference<BlueprintCueBaseReference>()
               //                DialogTools.CreateCue("gilmore-exit","a49b3308-49a4-4f8a-aa83-4d4a1948d7cc","fuck off then").ToReference<BlueprintCueBaseReference>()
               //            }
               //        }).Configure()
               //    }).Configure();
               //
               // var firstCue = DialogTools.CreateCue("gilmore-first-cue", "7238bdfa-b8ef-40c5-a610-e601c926293c", "Gilmore looks at you");
               // DialogTools.CueAddAwnserList(firstCue, gilmoreAwnsers);
               //
               // // CueConfigurator.New("gilmore-first-cue", "9AB67031-9BEC-42ED-8926-EAC60F009C34")
               // // .SetText(Helpers.CreateString(SSSContext,"gilmore-first-cue-text","Gilmore looks at you"))
               // // .SetTurnSpeaker(true)
               //
               //
               // //.SetSpeaker()
               // //.Configure();
               //
               // GilmoreDialog = DialogTools.CreateDialog("gilmore-service-dialog", "CBF8405C-B71D-47C4-9B51-B43C6AD925F6", firstCue);

            // DialogConfigurator.New("gilmore-service-dialog", "CBF8405C-B71D-47C4-9B51-B43C6AD925F6")
            // .SetFirstCue(firstCue: new() {
            //     Cues = new List<BlueprintCueBaseReference>() { firstCue.ToReference<BlueprintCueBaseReference>() },
            //     Strategy = 0
            // })
            //.SetTurnFirstSpeaker(true)
            //.SetTurnPlayer(true)
            //.Configure();

            //
            // GilmoreDialog = DialogTools.CreateDialog("gilmore-service-dialog", dialog =>
            // {
            //     dialog.FirstCue.Cues.Add(servicebuilder.Build());
            //     dialog.TurnFirstSpeaker = true;
            // });
            //
            //
            // //gilmoreDialogCue.SetSpeaker(new DialogSpeaker() { 



            //var gilmoreDialogAwnsersList = AnswersListConfigurator.New("", "");

            //gilmoreDialog.SetTurnFirstSpeaker(true);
            //  gilmoreDialog.SetFirstCue()



           /// !!!!!Copy of Bubbles Dialog, should work but doesnt.
            //     var basicShop = new StartTrade();
            //     basicShop.Vendor = new DialogFirstSpeaker();
            //     var serviceBuilder = new DialogBuilder("service-dialog");
            //   
            //     var mainroot = serviceBuilder.Root("Gilmore looks at you");
            //     mainroot.Speaker(GilmoreVendorBlueprint.ToReference<BlueprintUnitReference>());
            //   
            //     mainroot.Answers()
            //         .Add("shop", "What do you have for sale")
            //             .AddAction(basicShop)
            //             .Commit()
            //         .Add("end", "Ima go meow")
            //             .Commit()
            //         .Commit();
            //     mainroot.Commit();
            //   
            //     GilmoreDialog = DialogTools.CreateDialog("gilmore-service-dialog", dialog =>
            //     {
            //         dialog.FirstCue.Cues.Add(serviceBuilder.Build());
            //         dialog.TurnFirstSpeaker = true;
            //     });
            var clone = ResourcesLibrary.TryGetBlueprint<BlueprintUnit>("0234cbc0cc844da4d9cb225d6ed76a18");

            //GilmoreVendorBlueprint = clone.CreateCopy<BlueprintUnit>(SSSContext, "GilmoreBlueprintUnit", BlueprintGuid.Parse(Guid));

            //var spawn = Helpers.CreateCopy<BlueprintUnit>(clone);

            //ItemToolExtensions.AddBlueprint(spawn, Guid);

            //GilmoreVendorBlueprint = UnitConfigurator.New("Gilmore_Vendor", Guid)
            GilmoreVendorBlueprint = UnitConfigurator.New("GilmoreBlueprintUnit", Guid)

            .SetType("947e4895ec8384d4b96accfcad71b9f1")
            .SetAllowNonContextActions(false)
            .SetLocalizedName(localizedName: new SharedStringAsset()
            {
                String = Helpers.CreateString(SSSContext, "gilmore_vendor", "Shaun Gilmore")
            })
            .SetGender(Gender.Male)
            .SetSize(Kingmaker.Enums.Size.Medium)
            .SetIsLeftHanded(false)
            .SetColor(color: new UnityEngine.Color(0.15f, 0.95f, 0.5f))
            .SetRace("0a5d473ead98b0646b94495af250fdc4")
            .SetAlignment(Kingmaker.Enums.Alignment.ChaoticNeutral)
            .SetPortrait("0ed2a5b9a479459f9e1f0188b3f1305c")//Random Till set up
            .SetPrefab(prefab: new Kingmaker.ResourceLinks.UnitViewLink() { AssetId = "fe0715907f6fbae46b71c19c2550ae58" })
            //.SetVisual(visual: new UnitVisualParams()
            //{
            //    BloodType = Kingmaker.Visual.HitSystem.BloodType.Common,
            //    FootprintType = Kingmaker.Enums.FootprintType.Humanoid,
            //    FootprintScale = 1.0f,
            //    ArmorFx = null,
            //    BloodPuddleFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "" },
            //    DismemberFx = ResourcesLibrary.TryGetResource<>
            //    RipLimbsApartFx = null,
            //    IsNotUseDismember = false,
            //    m_Barks = BlueprintTool.GetRef<BlueprintUnitAsksListReference>("482042afbea927e44a6917dd701d400c"),//temp4now
            //    ReachFXThresholdBonus = 0.0f,
            //    DefaultArmorSoundType = ArmorSoundType.Flesh,
            //    FootstepSoundSizeType = FootstepSoundSizeType.BootMedium,
            //    FootSoundType = FootSoundType.Boot,
            //    FootSoundSize = Kingmaker.Enums.Size.Medium,
            //    BodySoundSize = Kingmaker.Enums.Size.Medium,
            //    BodySoundType = BodySoundType.Flesh,
            //    ImportanceOverride = 0,
            //    NoFinishingBlow = false,
            //    SilentCaster = false
            //})
            .SetVisual(clone.Visual)
            .SetFaction("d8de50cc80eb4dc409a983991e0b77ad")
            .SetFactionOverrides(factionOverrides: new FactionOverrides())
            .SetStartingInventory()
            .SetBrain("87be6f0c7f8ab2245a0b9049bfb575fc")
            .SetAlternativeBrains()
            
            .SetBody(body: new BlueprintUnit.UnitBody()
            {
                m_EmptyHandWeapon = BlueprintTool.GetRef<BlueprintItemWeaponReference>("20375b5a0c9243d45966bd72c690ab74"),
                ActiveHandSet = 0,
                DisableHands = false,
                m_Armor = BlueprintTool.GetRef<BlueprintItemArmorReference>("559b0b6f194656c428c403a000ceee78")
            })

            .SetStrength(20)
            .SetIntelligence(20)
            .SetCharisma(20)
            .SetConstitution(20)
            .SetWisdom(20)
            .SetDexterity(20)
            .SetSpeed(30.Feet())
            .SetBaseAttackBonus(0)
            .SetSkills(skills: new BlueprintUnit.UnitSkills())
            .SetMaxHP(0)
            .SetAdditionalTemplates()


            //Components
            .AddClassLevels(characterClass: "6ab4526f94d2e3e439af0599a29b6675",
            levels: 2, raceStat: Kingmaker.EntitySystem.Stats.StatType.Strength, levelsStat: Kingmaker.EntitySystem.Stats.StatType.Strength,
            doNotApplyAutomatically: false,
            selections: new Kingmaker.Blueprints.Classes.SelectionEntry[]
                {
                        new Kingmaker.Blueprints.Classes.SelectionEntry()
                        {
                            IsParametrizedFeature = false,
                            m_Selection = BlueprintTools.GetBlueprintReference<BlueprintFeatureSelectionReference>("247a4068296e8be42890143f451b4b45"),
                            m_Features = new BlueprintFeatureReference[]
                            {
                                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>("d09b20029e9abfe4480b356c92095623")
                            },
                            ParamSpellSchool = Kingmaker.Blueprints.Classes.Spells.SpellSchool.None,
                            ParamWeaponCategory = Kingmaker.Enums.WeaponCategory.UnarmedStrike,
                            Stat =  Kingmaker.EntitySystem.Stats.StatType.Unknown
                        }
                })
            .AddExperience(cR: 0, encounter: Kingmaker.Blueprints.Classes.Experience.EncounterType.Mob, modifier: 1.0f, dummy: false)
            .AddSharedVendor(gilmoreVendorList)

            .AddFacts(facts: new List<Blueprint<BlueprintUnitFactReference>> { "281a1f606d92728409ee5cbf5599855d" })
            .SetPS4ChunkId(Kingmaker.Blueprints.Area.PS4ChunkId.Chapter3)
            .SetHasAssignedChunkId(true)

            .AddDialogOnClick(dialog:GilmoreDialog)
            //.AddDialogOnClick(dialog:GilmoreDialog, conditions:ConditionsBuilder.New(),noDialogActions:ActionsBuilder.New(),triggerOnApproach:false)

            .Configure();
            //GilmoreVendorBlueprint.AddComponent<DialogOnClick>(onClick =>
            //{
            //    onClick.m_Dialog = GilmoreDialog.ToReference<BlueprintDialogReference>();
            //    onClick.name = "startvendinggilmore";
            //    onClick.NoDialogActions = new();
            //    onClick.Conditions = new();
            //});
            //Set Portrait

           
            
        }

        public static void SpawnGilmore()
        {
            Main.Log("Adding Gilmore to Event Bus");
            ThousandDelightsVendor delightsVendor = new();
            EventBus.Subscribe(delightsVendor);
        }
    }
}
