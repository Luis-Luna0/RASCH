using Microsoft.AspNetCore.Mvc.Rendering;

namespace RASCH.API.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboBusiness();


    }
}