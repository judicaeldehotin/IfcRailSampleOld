﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Common;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IfcRail.SharedBldgElements;

namespace SampleCreator
{
    class TypeChanger
    {
        public static void ChangeBuildingToRailway(IModel model)
        {
            foreach (var building in model.Instances.OfType<IfcBuilding>().ToList())
            {
                var railway = ModelHelper.InsertCopy<IfcRailway>(model, building);
                ModelHelper.Replace(model, building, railway);
                model.Delete(building);
            }

            foreach (var storey in model.Instances.OfType<IfcBuildingStorey>().ToList())
            {
                var railwayPart = ModelHelper.InsertCopy<IfcRailwayPart>(model, storey);
                ModelHelper.Replace(model, storey, railwayPart);
                model.Delete(storey);
            }

            // sleepers
            foreach (var railings in model.Instances.Where<IfcRailing>(r => r.Name.ToString().StartsWith("Betonschwelle")).ToList())
            {
                var sleeper = ModelHelper.InsertCopy<IfcRailElement>(model, railings);
                sleeper.PredefinedType = IfcRailElementTypeEnum.SLEEPER;
                ModelHelper.Replace(model, railings, sleeper);
                model.Delete(railings);

                var type = sleeper.IsTypedBy.FirstOrDefault()?.RelatingType;
                if (type != null && !(type is IfcRailElementType))
                {
                    var sleeperType = ModelHelper.InsertCopy<IfcRailElementType>(model, type);
                    sleeperType.PredefinedType = IfcRailElementTypeEnum.SLEEPER;
                    ModelHelper.Replace(model, type, sleeperType);
                    model.Delete(type);
                }
            }

            // rail joints
            foreach (var railings in model.Instances.Where<IfcRailing>(r => r.Name.ToString().Contains("_Herzstück_")).ToList())
            {
                var sleeper = ModelHelper.InsertCopy<IfcRailElement>(model, railings);
                sleeper.PredefinedType = IfcRailElementTypeEnum.JOINT;
                ModelHelper.Replace(model, railings, sleeper);
                model.Delete(railings);

                var type = sleeper.IsTypedBy.FirstOrDefault()?.RelatingType;
                if (type != null && !(type is IfcRailElementType))
                {
                    var sleeperType = ModelHelper.InsertCopy<IfcRailElementType>(model, type);
                    sleeperType.PredefinedType = IfcRailElementTypeEnum.JOINT;
                    ModelHelper.Replace(model, type, sleeperType);
                    model.Delete(type);
                }
            }

            // sleepers
            foreach (var railings in model.Instances.Where<IfcRailing>(r => r.Name.ToString().StartsWith("Schienen")).ToList())
            {
                var sleeper = ModelHelper.InsertCopy<IfcRailElement>(model, railings);
                sleeper.PredefinedType = IfcRailElementTypeEnum.RAIL;
                ModelHelper.Replace(model, railings, sleeper);
                model.Delete(railings);

                var type = sleeper.IsTypedBy.FirstOrDefault()?.RelatingType;
                if (type != null && !(type is IfcRailElementType))
                {
                    var sleeperType = ModelHelper.InsertCopy<IfcRailElementType>(model, type);
                    sleeperType.PredefinedType = IfcRailElementTypeEnum.RAIL;
                    ModelHelper.Replace(model, type, sleeperType);
                    model.Delete(type);
                }
            }

            // terrain
            foreach (var proxy in model.Instances.Where<IfcBuildingElementProxy>(r => r.Name.ToString().StartsWith("DirectShape(Geometry = Surface, Category = Topography")).ToList())
            {
                var terrain = ModelHelper.InsertCopy<IfcGeographicElement>(model, proxy);
                terrain.PredefinedType = IfcGeographicElementTypeEnum.TERRAIN;
                ModelHelper.Replace(model, proxy, terrain);
                model.Delete(proxy);

                var type = terrain.IsTypedBy.FirstOrDefault()?.RelatingType;
                if (type != null && !(type is IfcRailElementType))
                {
                    var sleeperType = ModelHelper.InsertCopy<IfcGeographicElementType>(model, type);
                    sleeperType.PredefinedType = IfcGeographicElementTypeEnum.TERRAIN;
                    ModelHelper.Replace(model, type, sleeperType);
                    model.Delete(type);
                }
            }
        }
    }
}