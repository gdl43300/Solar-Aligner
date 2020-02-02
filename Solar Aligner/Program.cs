using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {


        string solarPanelsGroup = "Ciapa-Solar Panels";
        string oxygenFarmsGroup = "Ciapa-Oxygen Farms";
        string rotorsGroup = "Ciapa-Solar Rotors";

        float searchSpeed = 0.1f;
        float alignSpeed = 0.05f;



        List<IMySolarPanel> solarPanels;
        List<IMyMotorStator> rotors;
        List<IMyOxygenFarm> oxygenFarms;
        float oxygenProduced = 0;
        float oxygenProducedLast;
        float energyProduced = 0;
        float energyProducedLast;
        IEnumerator<bool> state;

        public Program()
        {

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            solarPanels = new List<IMySolarPanel>();
            rotors = new List<IMyMotorStator>();
            oxygenFarms = new List<IMyOxygenFarm>();

            InitLists();
            ProductionCalcul();
            state = AlignRotors();
        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            
            ProductionCalcul();
            PrintLists();
            if (!state.MoveNext())
            {
                state.Dispose();
                state = AlignRotors();
            }

        }

        public void PrintLists()
        {
            Echo("Rotors(" + rotors.Count +  ") : ");
            foreach (var rotor in rotors)
            {
                Echo(rotor.DisplayNameText);
            }

            Echo("\nSolar Panels (" + solarPanels.Count + ") : " + energyProduced + "/" + energyProducedLast + " MW");
            foreach (var solar in solarPanels)
            {
                Echo(solar.DisplayNameText + " : " + solar.MaxOutput + "MW");
            }

            Echo("\nOxygen Farms (" + oxygenFarms.Count + ") : " + oxygenProduced + "/" + oxygenProducedLast + " L");
            foreach (var oxy in oxygenFarms)
            {
                Echo(oxy.DisplayNameText + " : " + oxy.GetOutput() + " L");
            }

        }

        public void InitLists()
        {
            IMyBlockGroup temp = GridTerminalSystem.GetBlockGroupWithName(solarPanelsGroup);
            if (temp != null)
            {
                temp.GetBlocksOfType(solarPanels);
            }
            else
            {
                solarPanels.Clear();
            }
            temp = GridTerminalSystem.GetBlockGroupWithName(oxygenFarmsGroup);
            if (temp != null)
            {
                temp.GetBlocksOfType(oxygenFarms);
            }
            else
            {
                oxygenFarms.Clear();
            }
            temp = GridTerminalSystem.GetBlockGroupWithName(rotorsGroup);
            if (temp != null)
            {
                temp.GetBlocksOfType(rotors);
            }
            else
            {
                rotors.Clear();
            }
        }

        public void CheckLists()
        {
            bool majNedded = false;

            // Check if the lists are valid



            if (majNedded)
            {
                InitLists();
            }
        }

        public void ProductionCalcul()
        {
            energyProducedLast = energyProduced;
            oxygenProducedLast = oxygenProduced; 
            energyProduced = 0;
            oxygenProduced = 0;

            foreach(var solar in solarPanels)
            {
                energyProduced += solar.MaxOutput;
            }
            foreach(var oxy in oxygenFarms)
            {
                oxygenProduced += oxy.GetOutput();
            }

        }

        public IEnumerator<bool> AlignRotors()
        {
            foreach(var rotor in rotors)
            {
                float angleOpti = rotor.Angle;
                rotor.TargetVelocityRPM = searchSpeed;
                yield return true;
                while (GetDifference() > 0)
                {
                    angleOpti = rotor.Angle;
                    yield return true;
                }
                rotor.TargetVelocityRPM = -searchSpeed;
                yield return true;
                while (GetDifference() > 0)
                {
                    angleOpti = rotor.Angle;
                    yield return true;
                }

                while (SetAngle(rotor, angleOpti))
                {
                    yield return true;
                }


            }
            yield return false;
        }

        private bool SetAngle(IMyMotorStator rotor, float angleOpti)
        {
            float delta = 0.5f;
            float dif = angleOpti - rotor.Angle;
            if (Math.Abs(dif) < delta)
            {
                rotor.TargetVelocityRPM = 0;
                return false;
            }
            if (dif < 0)
            {
                rotor.TargetVelocityRPM = -alignSpeed;
            }
            else
            {
                rotor.TargetVelocityRPM = alignSpeed;
            }
            return true;
        }

        public float GetDifference()
        {
            return (energyProduced - energyProducedLast) / energyProduced + (oxygenProduced - oxygenProducedLast) / oxygenProduced;
        }
    }
}
