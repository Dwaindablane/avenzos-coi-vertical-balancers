using Mafi.Base;
using Mafi.Core.Prototypes;

namespace ExampleMod;

public partial class ExampleModCosts
{
	public partial class Balancers
    {
        public static readonly EntityCostsTpl VerticalLoose = Costs.Build.CP2(4).Priority(0);
        public static readonly EntityCostsTpl VerticalFlat = Costs.Build.CP2(4).Priority(0);
        public static readonly EntityCostsTpl VerticalPipe = Costs.Build.CP2(4).Priority(0);
    }

}