using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace ExampleMod;

public sealed class ExampleMod : IMod {

	// Name of this mod. It will be eventually shown to the player.
	public string Name => "Example mod";

	// Version, currently unused.
	public int Version => 1;

    public bool IsUiOnly => throw new System.NotImplementedException();


    // Mod constructor that lists mod dependencies as parameters.
    // This guarantee that all listed mods will be loaded before this mod.
    // It is a good idea to depend on both `Mafi.Core.CoreMod` and `Mafi.Base.BaseMod`.
    public ExampleMod(CoreMod coreMod, BaseMod baseMod) {
		// You can use Log class for logging. These will be written to the log file
		// and can be also displayed in the in-game console with command `also_log_to_console`.
		Log.Info("ExampleMod: constructed");
	}


	public void RegisterPrototypes(ProtoRegistrator registrator) {
		Log.Info("ExampleMod: registering prototypes");
        // Register all prototypes here.
        //Mafi.Core.Factory.Zippers.Zipper
        // Registers all products from this assembly. See ExampleModIds.Products.cs for examples.
        //registrator.RegisterAllProducts();
        //Mafi.Core.Entities.c
        // Use data class registration to register other protos such as machines, recipes, etc.
        ProtosDb prototypesDb = registrator.PrototypesDb;
        prototypesDb.Add(new ToolbarCategoryProto(ExampleModIds.ToolbarCategories.Balancers, Proto.CreateStr(ExampleModIds.ToolbarCategories.Balancers, "Balancers", null, ""), 110f, "Assets/Unity/UserInterface/Toolbar/Transports.svg", isTransportBuildAllowed: true, containsTransports: false, "BALANCER"));
        registrator.RegisterData<ExampleMachineData>();
          
		// Registers all research from this assembly. See ExampleResearchData.cs for examples.
		registrator.RegisterDataWithInterface<IResearchNodesData>();

        

    }

    public void ChangeConfigs(Lyst<IConfig> configs)
    {
        
    }

    public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded)
    {
        
    }

    public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        
    }
}