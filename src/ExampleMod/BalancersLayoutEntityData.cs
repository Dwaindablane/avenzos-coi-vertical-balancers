using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Mods;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Avenzos.VerticalBalancers;

internal class BalancersLayoutEntityData : IModData
{
    private static EntityLayout BuildElevatorLayout(ProtosDb db, IoPortShapeProto portShape, int height)
    {
        if (portShape == null)
        {
            throw new InvalidOperationException("Invalid Port Shape char");
        }

        CustomLayoutToken[] customTokens = new CustomLayoutToken[] { new CustomLayoutToken("[0]", (EntityLayoutParams p, int h) => new LayoutTokenSpec(0, h)) };

        var layoutParams = new EntityLayoutParams(useNewLayoutSyntax: true, customTokens: customTokens, customPlacementRange: new ThicknessIRange(new ThicknessTilesI(0), new ThicknessTilesI(0)));

        var initLayout = new EntityLayoutParser(db).ParseLayoutOrThrow(layoutParams, $"[{height}]");

        IoPortTemplate[] ports = new IoPortTemplate[]
        {
        new IoPortTemplate(new PortSpec('A', IoPortType.Any, portShape, false), new RelTile3i(0, 0, 0), Direction90.PlusX),
        new IoPortTemplate(new PortSpec('B', IoPortType.Any, portShape, false), new RelTile3i(0, 0, 0), Direction90.MinusX),
        new IoPortTemplate(new PortSpec('C', IoPortType.Any, portShape, false), new RelTile3i(0, 0, height - 1), Direction90.PlusY),
        new IoPortTemplate(new PortSpec('D', IoPortType.Any, portShape, false), new RelTile3i(0, 0, height - 1), Direction90.MinusY),
        };

        return new EntityLayout("", initLayout.LayoutTiles, initLayout.TerrainVertices, ports.ToImmutableArray(), layoutParams, initLayout.CollapseVerticesThreshold);
    }

    private void AddZippers(ProtoRegistrator registrator, StaticEntityProto.ID id, string name, IoPortShapeProto portShape, EntityCostsTpl costs, int height, string prefabPath, Option<string> iconPath)
    {
        ProtosDb prototypesDb = registrator.PrototypesDb;
        Proto.Str strings = Proto.CreateStr(id, name, "Allows distributing and prioritizing products using any of its two input and output ports.", "small machine that allows splitting and merging of transports");

        Electricity requiredPower = (registrator.DisableAllProtoCosts ? Electricity.Zero : 1.Kw());
        EntityLayout layout = BuildElevatorLayout(registrator.PrototypesDb, portShape, height);

        ImmutableArray<ToolbarCategoryProto> categories = registrator.GetCategoriesProtos(Ids.ToolbarCategories.Balancers);
        var zipper = new ZipperProto(id, strings, layout, costs.MapToEntityCosts(registrator), requiredPower, new LayoutEntityProto.Gfx(prefabPath,  default(RelTile3f), iconPath, ColorRgba.White, hideBlockedPortsIcon: true, null, categories, useInstancedRendering: false, 2));

        prototypesDb.Add(zipper);
    }

    public void RegisterData(ProtoRegistrator registrator)
    {

        IoPortShapeProto shapeFlat = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Mafi.Base.Ids.IoPortShapes.FlatConveyor);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalFlatI1, Titles.Balancers.BalancerVerticalFlatI1, shapeFlat, Costs.Balancers.VerticalFlat, 1, Prefabs.Balancers.BalancerVerticalFlatI1, Icons.Balancers.BalancerVerticalFlatI1);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalFlatI2, Titles.Balancers.BalancerVerticalFlatI2, shapeFlat, Costs.Balancers.VerticalFlat, 2, Prefabs.Balancers.BalancerVerticalFlatI2, Icons.Balancers.BalancerVerticalFlatI2);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalFlatI3, Titles.Balancers.BalancerVerticalFlatI3, shapeFlat, Costs.Balancers.VerticalFlat, 3, Prefabs.Balancers.BalancerVerticalFlatI3, Icons.Balancers.BalancerVerticalFlatI3);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalFlatI4, Titles.Balancers.BalancerVerticalFlatI4, shapeFlat, Costs.Balancers.VerticalFlat, 4, Prefabs.Balancers.BalancerVerticalFlatI4, Icons.Balancers.BalancerVerticalFlatI4);

        IoPortShapeProto shapeLoose = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Mafi.Base.Ids.IoPortShapes.LooseMaterialConveyor);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalLooseI1, Titles.Balancers.BalancerVerticalLooseI1, shapeLoose, Costs.Balancers.VerticalLoose, 1, Prefabs.Balancers.BalancerVerticalLooseI1, Icons.Balancers.BalancerVerticalLooseI1);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalLooseI2, Titles.Balancers.BalancerVerticalLooseI2, shapeLoose, Costs.Balancers.VerticalLoose, 2, Prefabs.Balancers.BalancerVerticalLooseI2, Icons.Balancers.BalancerVerticalLooseI2);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalLooseI3, Titles.Balancers.BalancerVerticalLooseI3, shapeLoose, Costs.Balancers.VerticalLoose, 3, Prefabs.Balancers.BalancerVerticalLooseI3, Icons.Balancers.BalancerVerticalLooseI3);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalLooseI4, Titles.Balancers.BalancerVerticalLooseI4, shapeLoose, Costs.Balancers.VerticalLoose, 4, Prefabs.Balancers.BalancerVerticalLooseI4, Icons.Balancers.BalancerVerticalLooseI4);

        IoPortShapeProto shapePipe = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Mafi.Base.Ids.IoPortShapes.Pipe);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalPipeI1, Titles.Balancers.BalancerVerticalPipeI1, shapePipe, Costs.Balancers.VerticalPipe, 1, Prefabs.Balancers.BalancerVerticalPipeI1, Icons.Balancers.BalancerVerticalPipeI1);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalPipeI2, Titles.Balancers.BalancerVerticalPipeI2, shapePipe, Costs.Balancers.VerticalPipe, 2, Prefabs.Balancers.BalancerVerticalPipeI2, Icons.Balancers.BalancerVerticalPipeI2);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalPipeI3, Titles.Balancers.BalancerVerticalPipeI3, shapePipe, Costs.Balancers.VerticalPipe, 3, Prefabs.Balancers.BalancerVerticalPipeI3, Icons.Balancers.BalancerVerticalPipeI3);
        AddZippers(registrator, Ids.Balancers.BalancerVerticalPipeI4, Titles.Balancers.BalancerVerticalPipeI4, shapePipe, Costs.Balancers.VerticalPipe, 4, Prefabs.Balancers.BalancerVerticalPipeI4, Icons.Balancers.BalancerVerticalPipeI4);
    }
}