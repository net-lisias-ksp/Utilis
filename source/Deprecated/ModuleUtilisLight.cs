//
//  ModuleTestModules.cs
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utilis
{
	public class ModuleUtilisLight : PartModule, IResourceConsumer
	{
		[KSPField]
		public string moduleID = "ModuleUtilisLight";

		[KSPField]
		public string lightName;

		[KSPField]
		public FloatCurve redCurve = new FloatCurve();
		
		[KSPField]
		public FloatCurve greenCurve = new FloatCurve();
		
		[KSPField]
		public FloatCurve blueCurve = new FloatCurve();

		[KSPField(isPersistant=true)]
		public bool canMove = true;

		[KSPField(isPersistant=true)]
		public bool isOn = false;
		
		[KSPField(isPersistant=true)]
		public bool isTurningOn = false;

		[KSPField(isPersistant=true)]
		public bool isTurningOff = false;

		[KSPField]
		public bool drawRsources = true;

		private List<PartResourceDefinition> consumedResources;
		
		//needed by IResourceConsumer interface
		public List<PartResourceDefinition> GetConsumedResources ()
		{
			return consumedResources;
		}

		[KSPField]
		public float dimSpeed = 2f;

		[KSPField]
		public bool persistent = true;

		[KSPField(isPersistant=true)]
		public bool operational = true;

		[KSPField]
		public string axis = "y";
		
		[KSPField]
		public string deployAnimName;
		
		[KSPField]
		public string transformName;

		[KSPField(isPersistant=true)]
		public float last;

		[KSPField]
		public bool deployableAnimation = false;

		[KSPField]
		public bool proceduralDeploy = false;

		[KSPField]
		public bool manualDeploy = true;

		[KSPField]
		public string shaderProperty = "_EmissiveColor";

		[KSPField]
		public string statusText = "Nominal";

		[KSPEvent(guiActive = true,guiActiveEditor = true,guiName = "Light On")]
		public void lightOn()
		{
			int count = part.symmetryCounterparts.Count;
			while (count-- > 0)
			{
				if (part.symmetryCounterparts [count] != base.part)
				{
					part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).EnableLights(true);
					part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).isOn = true;
					part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).isTurningOn = true;
				}
			}
			lights.Enable();
			isOn = true;
			isTurningOn = true;
		}

		[KSPEvent(guiActive = true,guiActiveEditor = true,guiName = "Light Off")]
		public void lightOff()
		{
			int count = part.symmetryCounterparts.Count;
			while (count-- > 0)
			{
				if (part.symmetryCounterparts [count] != base.part)
				{
					part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).isOn = false;
				}
			}
			isOn = false;
		}

		[KSPAction ("Turn Light On")]
		public void LightOnAction (KSPActionParam param)
		{
			lightOn ();
		}


		[KSPAction ("Turn Light Off")]
		public void LightOffAction (KSPActionParam param)
		{
			lightOff ();
		}

		[KSPField(guiName = "Status",guiActive=true,guiActiveEditor=false)]
		public string status = "Nominal";

		private Color lightColor;

		public int shaderPropertyInt;
		
		private MaterialPropertyBlock mpb;

		private float dimCounter = 0;

		private UtilisLightList lights;

		private Transform pivot;

		private List<Renderer> renderers = new List<Renderer>();
		
		public List<string> usedRenderers;

		private void log(string msg)
		{
			string header = "[ModuleUtilisLight]";
			Debug.Log (header +" "+msg);
		}

		//checks to see if the available resources are present
		public void hasResources()
		{
			if (!resHandler.UpdateModuleResourceInputs (ref status,1f, 0.9, true, true))
			{
				lightOff();
				operational=false;
			}
			else if(!operational)
			{
				operational = true;
			}
		}

		public void EnableLights(bool state)
		{
			if(state)
			{
				lights.Enable();
			}
			else{
				lights.Disable();
			}
		}

		public void dimLight(bool state)
		{
			float r = redCurve.Evaluate(dimCounter)*lightColor.r;
			float g = greenCurve.Evaluate(dimCounter)*lightColor.g;
			float b = blueCurve.Evaluate(dimCounter)*lightColor.b;
			Color _color = new Color(r,g,b);
			lights.SetProp(_color);
			mpb.SetColor (shaderPropertyInt, _color);
			int count = renderers.Count;
			while (count-- > 0)
			{
				renderers [count].SetPropertyBlock (mpb);
			}
			if(state)
			{
				dimCounter += TimeWarp.fixedDeltaTime * dimSpeed;
			}else{
				dimCounter -= TimeWarp.fixedDeltaTime * dimSpeed;
			}
			if(dimCounter >= 1)
			{
				isTurningOn = false;
			}
			if(dimCounter <= 0)
			{
				lights.Disable();
			}
		}

		public void GetColor(Color c)
		{
			lightColor = c;
			mpb.SetColor (shaderPropertyInt, c);
			int count = this.renderers.Count;
			while (count-- > 0)
			{
				this.renderers [count].SetPropertyBlock (this.mpb);
			}
			lights.SetProp(c);
		}

		public void GetValue(float val)
		{
			int count = part.symmetryCounterparts.Count;

			Vector3 old = pivot.localEulerAngles;

			switch(axis)
			{
			case "x":
				last = old.x + val;

				pivot.localEulerAngles = new Vector3(last,old.y,old.z);

				while (count-- > 0)
				{
					if (part.symmetryCounterparts [count] != base.part)
					{
						part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).pivot.localEulerAngles = new Vector3(last,old.y,old.z);
					}
				}
				break;
			case "y":
				last = old.y + val;
				pivot.localEulerAngles = new Vector3(old.x,last,old.z);

				while (count-- > 0)
				{
					if (part.symmetryCounterparts [count] != base.part)
					{
						part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).pivot.localEulerAngles = new Vector3(last,old.y,old.z);
					}
				}
				break;
			case "z":
				last = old.z + val;

				pivot.localEulerAngles = new Vector3(old.x,old.y,last);

				while (count-- > 0)
				{
					if (part.symmetryCounterparts [count] != base.part)
					{
						part.symmetryCounterparts [count].Modules.GetModule<ModuleUtilisLight> (0).pivot.localEulerAngles = new Vector3(last,old.y,old.z);
					}
				}
				break;
			}
		}

		public override void OnStart (StartState state)
		{
			base.OnStart (state);

			BaseField statusField = base.Fields["status"];

			statusField.guiName = statusText;

			lights = new UtilisLightList( part.FindModelComponents<Light>() );

			if(proceduralDeploy || manualDeploy)
			{
				if(transformName !=  string.Empty)
				{
					pivot = part.FindModelTransform(transformName);
					if(persistent)
					{
						Vector3 old = pivot.localEulerAngles;
						switch(axis)
						{
						case "x":
							pivot.localEulerAngles = new Vector3(last,old.y,old.z);
							break;
						case "y":
							pivot.localEulerAngles = new Vector3(old.x,last,old.z);
							break;
						case "z":
							pivot.localEulerAngles = new Vector3(old.x,old.y,last);
							break;
						}
					}
				}else{
					log ("ERROR: <string> 'transformName' is empty. Check CFG file. 'proceduralDeploy' or 'manualDeploy' were set to True");
					throw new Exception("ERROR: Pivot Transform set to null. ");
				}
			}

			if(usedRenderers.Count<=1)
			{
				renderers = part.FindModelComponents<Renderer>();
			}else{
				int count = usedRenderers.Count;

				for(int i=0;i<count;i++)
				{
					renderers.Add(part.FindModelComponent<Renderer>(usedRenderers[i]));
				}
			}
			mpb = new MaterialPropertyBlock();

			if(string.IsNullOrEmpty(shaderProperty))
			{
				log ("shaderProperty returned empty or null. Check CFG file.");
			}else{
				shaderPropertyInt = Shader.PropertyToID (shaderProperty);
			}

		}

		public override void OnLoad (ConfigNode node)
		{
			base.OnLoad (node);
			if (node.HasValue ("usedRenderers"))
			{
				usedRenderers = new List<string>();

				usedRenderers.AddRange( node.GetValues ("usedRenderers") );
			}
		}

		public override void OnAwake ()
		{
			base.OnAwake ();
			if (this.consumedResources == null)
			{
				this.consumedResources = new List<PartResourceDefinition> ();
			}
			else
			{
				this.consumedResources.Clear ();
			}
			int i = 0;
			int count = this.resHandler.inputResources.Count;
			while (i < count)
			{
				this.consumedResources.Add (PartResourceLibrary.Instance.GetDefinition (this.resHandler.inputResources [i].name));
				i++;
			}
		}

		public void FixedUpdate()
		{
			if(!HighLogic.LoadedSceneIsEditor && !HighLogic.LoadedSceneIsFlight)return;

			//Deploy Animation

			//Procedural Deploy Animation

			if(operational)
			{
				if(isOn)
				{
					if(drawRsources)hasResources ();
					if(isTurningOn)
					{
						dimLight (isOn);
					}
				}else
				{
					dimLight (isOn);
				}
			}else{
				if(drawRsources)hasResources ();
			}
		}

		public override void OnStartFinished (StartState state)
		{
			base.OnStartFinished (state);
			ModuleSelectColor msc = part.FindModulesImplementing<ModuleSelectColor>()[0];
			ModuleTweakable mt = part.FindModulesImplementing<ModuleTweakable>()[0];
			//if(msc != null)msc.Add(GetColor);
			if(manualDeploy)if(mt != null)mt.Add(GetValue);
		}
	}
}

