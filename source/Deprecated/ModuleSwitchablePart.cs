//
//  ModuleSwitchablePart.cs
//
//  Author:
//       Bonus Eventus <>
//
//  Copyright (c) 2016 Bonus Eventus
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Utilis
{
	public class ModuleSwitchablePart : PartModule
	{
		//ksp gui

		[KSPField(guiName="Last",guiActiveEditor=false,guiActive=false)]
		public string Last;

		//ksp fields

		[KSPField(isPersistant=true)]
		public bool setupComplete = false;

		[KSPField(isPersistant=true)]
		public string _destroyTransformNames;

		//properties

		private int currentIndex = 0;

		private bool foundDefault = false;

		private List<PartProfile> partProfiles = new List<PartProfile>();

		private PartProfile defaultProfile;

		private string header = "[ModuleSwitchablePart]";

		//methods

		private List<string> destroyTransformNames;

		private void partSwitch(string scrub)
		{
			if(scrub == "Last")
			{
				if(partProfiles[currentIndex-1] != null)
				{
					//get previous part profile

					//delete current part profile


				}
			}else{
				if(partProfiles[currentIndex+1] != null)
				{
					//get next part profile

					//delete previous part profile


				}
			}
		}

		private void SetDefaultMeshes()
		{

		}

		private void log(string msg)
		{
			Debug.Log(header+msg);
		}

		public override void OnLoad (ConfigNode node)
		{
			base.OnLoad (node);
			//loop through ConfigNode to find part profiles nodes
			if(!setupComplete)
			{
				int count = node.nodes.Count;
				for (int i = 0; i < count; i++)
				{
					ConfigNode configNode = node.nodes [i];
					string name = configNode.name;
					if (name == "PART_PROFILE") 
					{
						PartProfile partProfile = new PartProfile(part);
						partProfile.Load(configNode);
						partProfiles.Add(partProfile);

						//only one partProfile can be default
						if(!foundDefault)
						{
							if(partProfile.isDefault)
							{
								defaultProfile = partProfile;
								foundDefault = true;
							}
						}
					}else{
						log (" node was not a part profile node. i = "+i.ToString());
					}
				}
			}else{
				if(node.HasValue("meshes") && node.HasValue("mesh")) 
				{
					//load all saved mesh names to destroy
					if( node.GetValue("_destroyTransformNames") != string.Empty ) 
					{
						char[] sep = new char[]{';'};
						destroyTransformNames =  new List<string>(node.GetValue("_destroyTransformNames").Split(sep));
					}
				}
			}
		}
	}
}

