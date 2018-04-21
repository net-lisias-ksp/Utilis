//
//  ModuleEmissiveLight.cs
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
	public class ModuleAnimatedSpot : PartModule //, IResourceConsumer
	{
		[KSPField (isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "Intensity"), UI_FloatRange (minValue = 0f, maxValue = 8f, stepIncrement = 0.05f)]
		public float intensity = 0f;

		[KSPField (isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "Spot Angle"), UI_FloatRange (minValue = 0f, maxValue = 179f, stepIncrement = 0.05f)]
		public float spotAngle = 30f;

		[KSPField (isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "Range"), UI_FloatRange (minValue = 0f, maxValue = 1000f, stepIncrement = 0.1f)]
		public float range = 10f;

		[KSPField (guiActive = true, guiName = "Light Status")]
		public string status = "Nominal";

		[KSPEvent(guiName = "Lights On")]
		public void LightOnAction (KSPActionParam param)
		{
			LightsOn();
		}

		[KSPEvent(guiName = "Lights Off")]
		public void lightOffAction (KSPActionParam param)
		{
			LightsOff();
		}

		[KSPField(isPersistant=true)]
		public bool Enable = false;

		[KSPField]
		public bool colorTweakable = true;

		[KSPField]
		public bool intensityTweakable = true;

		[KSPField]
		public bool spotAngleTweakable = true;

		[KSPField]
		public bool rangeTweakable = true;

		[KSPField]
		public bool rotateXTweakable = true;

		[KSPField]
		public bool rotateYTweakable = true;

		[KSPField]
		public bool rotateZTweakable = true;

		[KSPField]
		public bool hasBillboard = true;

		[KSPField]
		public string billboardName;

		//name of the transform light component should be added to.
		//also acts as a pivot
		[KSPField]
		public string lightTransformName;

		[KSPField(isPersistant=true)]
		public bool isOn = false;

		[KSPField]
		public float dimLength = 1f;

		/*public Color spotColor
		{
			get
			{
				return new Color(lightR,lightG,lightB);
			}
		}
*/
		public List<Renderer> renderers;

		public Transform lightTransform;

		public Light spotLight;

		public Transform billboard;

		public FlightCamera flightCamera;

		private string header = "[ModuleAnimatedSpot]";

		private void log(string msg)
		{
			Debug.Log(header+" "+msg);
		}

		public void LightsOn()
		{
			float t = 0;
			while(t<dimLength)
			{

				t += TimeWarp.fixedDeltaTime;
			}
		}

		public void LightsOff()
		{

		}

		public override void OnStart (StartState state)
		{
			base.OnStart (state);
			if(lightTransformName != string.Empty)
			{
				lightTransform = part.FindModelTransform(lightTransformName);
				spotLight = lightTransform.gameObject.AddComponent<Light>();
				spotLight.type = LightType.Spot;
				spotLight.spotAngle = spotAngle;
				spotLight.intensity = intensity;
				spotLight.range = range;
				//spotLight.color = spotColor;
				spotLight.enabled = Enable;

			}else{
				log ("lightTransformName string is empty. Check CFG file.");
			}
			if(hasBillboard)
			{
				flightCamera = FlightCamera.fetch;
				billboard = part.FindModelTransform(billboardName);
			}
		}

		public void LateUpdate()
		{
			if(!HighLogic.LoadedSceneIsEditor || !HighLogic.LoadedSceneIsFlight)return;
			//keeps the billboard looking at camera
			if(billboard != null)billboard.LookAt(flightCamera.transform.position, part.transform.up);
		}
	}
}

