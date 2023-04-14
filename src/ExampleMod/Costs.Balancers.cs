using Mafi.Base;
using Mafi.Core.Prototypes;

namespace Avenzos.VerticalBalancers;

public partial class Costs
{
	public partial class Balancers
    {
        public static readonly EntityCostsTpl VerticalLoose = Mafi.Base.Costs.Build.CP2(4).Priority(0);
        public static readonly EntityCostsTpl VerticalFlat = Mafi.Base.Costs.Build.CP2(4).Priority(0);
        public static readonly EntityCostsTpl VerticalPipe = Mafi.Base.Costs.Build.CP2(4).Priority(0);
    }

}