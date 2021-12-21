using Microsoft.Extensions.DependencyInjection;
using static KittyHelper.KittyHelper.KittyViewHelper;

namespace KittyHelper
{
    public interface ICreateAVueComponent
    {
        public abstract VueComponent Create();
    }
}