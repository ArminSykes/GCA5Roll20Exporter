﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCA5.Interfaces;
using GCA5Engine;

namespace ExportToRoll20
{
    public class ConvertToRoll20Character
    {
        public Roll20Character GetCharacter(GCACharacter currentCharacter)
        {
            Roll20Character roll20Character = new Roll20Character();

            if (currentCharacter != null)
            {
                roll20Character.CharacterName = currentCharacter.Name;
                roll20Character.Fullname = currentCharacter.FullName;
                roll20Character.Playername = currentCharacter.Player;
                roll20Character.Nickname = "";
                roll20Character.Race = currentCharacter.Race;
                roll20Character.RaceRef = "";
                roll20Character.TemplateNames = "";
                roll20Character.Gender = "";

                GCATrait traitSizeMod = currentCharacter.ItemByNameAndExt("Size Modifier", (int)TraitTypes.Attributes);
                int sizeMod = (int)traitSizeMod.Score;
                roll20Character.Size = sizeMod;
                roll20Character.ApplySizeModifier = sizeMod > 0;

                // TODO: Get reactions
                roll20Character.Reactions = "";

                if (int.TryParse(currentCharacter.Campaign.BaseTL, out int campaignTl))
                {
                    roll20Character.CampaignTl = campaignTl;
                }
                else
                {
                    roll20Character.CampaignTl = 0;
                }

                roll20Character.TotalPoints = (int)currentCharacter.Campaign.BasePoints;

                if (int.TryParse(currentCharacter.TL, out int techLevel))
                {
                    roll20Character.Tl = techLevel;
                }
                else
                {
                    roll20Character.Tl = 0;
                }
                GCATrait traitTechLevel = currentCharacter.ItemByNameAndExt("Tech Level", (int)TraitTypes.Attributes);

                roll20Character.TlPts = (int)traitTechLevel.Points;

                GCATrait traitStatus = currentCharacter.ItemByNameAndExt("Status", (int)TraitTypes.Attributes);
                roll20Character.Status = traitStatus.Score.ToString();

                roll20Character.Wealth = GetWealthLevel(currentCharacter);

                GCATrait traitMoney = currentCharacter.ItemByNameAndExt("Money", (int)TraitTypes.Attributes);
                roll20Character.Stash = traitMoney.Score;

                GCATrait traitMonthlyPay = currentCharacter.ItemByNameAndExt("Monthly Pay", (int)TraitTypes.Attributes);
                roll20Character.Income = traitMonthlyPay.Score;

                GCATrait traitCostOfLiving = currentCharacter.ItemByNameAndExt("Cost of Living", (int)TraitTypes.Attributes);
                roll20Character.CostOfLiving = (int)traitCostOfLiving.Score;

                roll20Character.Age = currentCharacter.Age;
                roll20Character.Height = currentCharacter.Height;
                if (double.TryParse(currentCharacter.Weight, out double weight))
                {
                    roll20Character.Weight = weight;
                }
                else
                {
                    roll20Character.Weight = 0;
                }

                roll20Character.Appearance = GetAppearanceScore(currentCharacter);
                roll20Character.GeneralAppearance = currentCharacter.Appearance;

            }

            return roll20Character;

        }

        public string GetWealthLevel(GCACharacter currentCharacter)
        {
            GCATrait advantageWealth = currentCharacter.ItemByNameAndExt("Wealth", (int)TraitTypes.Advantages);

            if (advantageWealth != null)
            {
                return advantageWealth.LevelName;
            }

            GCATrait disadvantageWealth = currentCharacter.ItemByNameAndExt("Wealth", (int)TraitTypes.Disadvantages);


            if (disadvantageWealth != null)
            { return disadvantageWealth.LevelName; }

            return "Average";
        }

        public int GetAppearanceScore(GCACharacter currentCharacter)
        {
            GCATrait advantageAppearance = currentCharacter.ItemByNameAndExt("Appearance", (int)TraitTypes.Advantages);

            if (advantageAppearance != null)
            {
                return (int)advantageAppearance.Score;
            }

            GCATrait disadvantageAppearance = currentCharacter.ItemByNameAndExt("Appearance", (int)TraitTypes.Disadvantages);

            if (disadvantageAppearance != null)
            {
                return (int)disadvantageAppearance.Score;
            }

            return 0;
        }

    }
}
