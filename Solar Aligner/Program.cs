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

        String solarPanelsGroup = "Ciapa-Solar Panels";
        String oxygenFarmsGroup = "Ciapa-Oxygen Farms";
        String rotorsGroup = "Ciapa-Solar Rotors";



        List<IMySolarPanel> solarPanels;
        List<IMyMotorStator> rotors;
        List<IMyOxygenFarm> oxygenFarms;

        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set Runtime.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            solarPanels = new List<IMySolarPanel>();
            rotors = new List<IMyMotorStator>();
            oxygenFarms = new List<IMyOxygenFarm>();

            initLists();

        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.

        }

        public void Main(string argument, UpdateType updateSource)
        {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from. Be aware that the
            // updateSource is a  bitfield  and might contain more than 
            // one update type.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.
        }

        public void initLists()
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

        public void checkLists()
        {
            bool majNedded = false;

            if (majNedded)
            {
                initLists();
            }
        }
    }
}
