using Fusion;

namespace Core.UtilityItems.Abstract
{
    public class UtilityItem : NetworkBehaviour
    {
        [Networked] protected string ItemId { get; set; }
    }
}