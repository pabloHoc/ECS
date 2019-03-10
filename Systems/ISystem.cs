using System.Collections.Generic;

namespace ECS
{
    public interface ISystem
    {
        ECSEngine Engine { set; }
        EventBus EventBus { set; }
        Components Components { set; }
        void Execute(float delta);
    }
}