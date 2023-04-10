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

namespace ExampleMod;

internal class ExampleMachineData : IModData
{


    //public static StaticEntityProto.ID GetZipperIdFor(int height, IoPortShapeProto.ID shapeId)
    //{
    //    return new StaticEntityProto.ID($"VerticalZipper_{height}_{shapeId.Value}");
    //}

    //    "   D?+   ", 
    //"E?+[2]+?B", 
    //"F?+[2]+?A", 
    //"   C?+   "));
    //setPortChar(portShape.LayoutChar,
    //    "   D?+C?+   ",
    //    "E?+[1][1]+?B",
    //    "F?+[1][1]+?A",
    //    "   G?+H?+   ")

    //"   [3][4][3][3][3][3]   ", 
    //"~3 >3A[4][3][3][3]X3> ~ ", 
    //"   [3][4][3][3][3][3]   ", 
    //"   [2][3][2][2]         "


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

    private void AddZippers(ProtoRegistrator registrator, StaticEntityProto.ID id, string name, IoPortShapeProto portShape, EntityCostsTpl costs, int height, string prefabPath)
    {
        ProtosDb prototypesDb = registrator.PrototypesDb;
        Proto.Str strings = Proto.CreateStr(id, name, "Allows distributing and prioritizing products using any of its two input and output ports.", "small machine that allows splitting and merging of transports");

        Electricity requiredPower = (registrator.DisableAllProtoCosts ? Electricity.Zero : 1.Kw());
        EntityLayout layout = BuildElevatorLayout(registrator.PrototypesDb, portShape, height);

        ImmutableArray<ToolbarCategoryProto> categories = registrator.GetCategoriesProtos(ExampleModIds.ToolbarCategories.Balancers);
        var zipper = new ZipperProto(id, strings, layout, costs.MapToEntityCosts(registrator), requiredPower, new LayoutEntityProto.Gfx(prefabPath, default(RelTile3f), default(Option<string>), ColorRgba.White, hideBlockedPortsIcon: true, null, categories, useInstancedRendering: true, 2));

        prototypesDb.Add(zipper);
    }

    public void RegisterData(ProtoRegistrator registrator)
    {

        IoPortShapeProto shapeFlat = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.FlatConveyor);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalFlatI1, ExampleModTitles.Balancers.BalancerVerticalFlatI1, shapeFlat, Costs.Transports.FlatZipper, 1, ExampleModPrefabs.Balancers.BalancerVerticalFlatI1);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalFlatI2, ExampleModTitles.Balancers.BalancerVerticalFlatI2, shapeFlat, Costs.Transports.FlatZipper, 2, ExampleModPrefabs.Balancers.BalancerVerticalFlatI2);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalFlatI3, ExampleModTitles.Balancers.BalancerVerticalFlatI3, shapeFlat, Costs.Transports.FlatZipper, 3, ExampleModPrefabs.Balancers.BalancerVerticalFlatI3);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalFlatI4, ExampleModTitles.Balancers.BalancerVerticalFlatI4, shapeFlat, Costs.Transports.FlatZipper, 4, ExampleModPrefabs.Balancers.BalancerVerticalFlatI4);

        IoPortShapeProto shapeLoose = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.LooseMaterialConveyor);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalLooseI1, ExampleModTitles.Balancers.BalancerVerticalLooseI1, shapeLoose, Costs.Transports.LooseZipper, 1, ExampleModPrefabs.Balancers.BalancerVerticalLooseI1);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalLooseI2, ExampleModTitles.Balancers.BalancerVerticalLooseI2, shapeLoose, Costs.Transports.LooseZipper, 2, ExampleModPrefabs.Balancers.BalancerVerticalLooseI2);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalLooseI3, ExampleModTitles.Balancers.BalancerVerticalLooseI3, shapeLoose, Costs.Transports.LooseZipper, 3, ExampleModPrefabs.Balancers.BalancerVerticalLooseI3);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalLooseI4, ExampleModTitles.Balancers.BalancerVerticalLooseI4, shapeLoose, Costs.Transports.LooseZipper, 4, ExampleModPrefabs.Balancers.BalancerVerticalLooseI4);

        IoPortShapeProto shapePipe = registrator.PrototypesDb.GetOrThrow<IoPortShapeProto>(Ids.IoPortShapes.Pipe);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalPipeI1, ExampleModTitles.Balancers.BalancerVerticalPipeI1, shapePipe, Costs.Transports.FluidZipper, 1, ExampleModPrefabs.Balancers.BalancerVerticalPipeI1);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalPipeI2, ExampleModTitles.Balancers.BalancerVerticalPipeI2, shapePipe, Costs.Transports.FluidZipper, 2, ExampleModPrefabs.Balancers.BalancerVerticalPipeI2);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalPipeI3, ExampleModTitles.Balancers.BalancerVerticalPipeI3, shapePipe, Costs.Transports.FluidZipper, 3, ExampleModPrefabs.Balancers.BalancerVerticalPipeI3);
        AddZippers(registrator, ExampleModIds.Balancers.BalancerVerticalPipeI4, ExampleModTitles.Balancers.BalancerVerticalPipeI4, shapePipe, Costs.Transports.FluidZipper, 4, ExampleModPrefabs.Balancers.BalancerVerticalPipeI4);




        // registrator.MachineProtoBuilder
        //     .Start("Example furnace", ExampleModIds.Machines.ExampleFurnace)
        //     .Description("Testing furnace")
        //     .SetCost(Costs.Build.CP(80).Workers(10))
        //     // For examples of layouts see `Mafi.Base.BaseMod` and `EntityLayoutParser`.
        //     .SetLayout(new EntityLayoutParams(useNewLayoutSyntax: true),
        //         "   [1]>~Y",
        //         "   [1]   ",
        //         "A~>[1]>'V",
        //         "B~>[1]>'W",
        //         "   [1]   ",
        //         "   [1]>@E")
        //     .SetCategories(Ids.ToolbarCategories.MachinesMetallurgy)
        //     .SetPrefabPath("Assets/ExampleMod/BlastFurnace.prefab")
        //     .SetAnimationParams(
        //         animParams: AnimationParams.RepeatTimes(Duration.FromKeyframes(360),
        //         times: 2,
        //         changeSpeedToFit: true))
        //     .BuildAndAdd();




        // //Example of a new furnace recipe.
        //registrator.RecipeProtoBuilder
        //    .Start(name: "Example smelting",
        //        recipeId: ExampleModIds.Recipes.ExampleSmelting,
        //        machineId: ExampleModIds.Machines.ExampleFurnace)
        //    .AddInput(8, ExampleModIds.Products.ExampleLooseProduct)
        //    .AddInput(2, Ids.Products.Coal)
        //    .SetDuration(20.Seconds())
        //    .AddOutput(8, ExampleModIds.Products.ExampleMoltenProduct)
        //    .AddOutput(24, Ids.Products.Exhaust, outputAtStart: true)
        //    .BuildAndAdd();

    }
}